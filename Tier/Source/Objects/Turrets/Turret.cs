using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using Tier.Source.Misc;
using Tier.Source.Handlers;
using Tier.Source.Objects.Projectiles;
using Tier.Source.GameStates;
using Tier.Source.Handlers.BossBehaviours;

namespace Tier.Source.Objects.Turrets
{
  public enum TurretState
  {
    Idle, Shooting
  };

  public abstract class Turret : GameObject
  {
    #region Properties
    protected Quaternion baseRotation;
    protected float timeElapsed;
    protected Vector3 origin;
    protected TurretState state;
    protected float shootAngle;
    protected float cooldown;
    protected Color projectileColor;
    protected float turnSpeed;
    protected string projectileType;
    private float range;
    private Matrix rotToPlayer;
    private bool isRotating;
    private Quaternion rotateFrom, rotateTo;
    private float percentage;
    private float startCooldown;

    public float Range 
    { 
      get { return range; } 
      set { range = value; } 
    }

    public string ProjectileType
    {
      get { return projectileType; }
      set { projectileType = value; }
    }
	
    public float TurnSpeed
    {
      get { return turnSpeed; }
      set { turnSpeed = value; }
    }
	
    public Color ProjectileColor
    {
      get { return projectileColor; }
      set { projectileColor = value; }
    }
	
    public float Cooldown
    {
      get { return cooldown; }
      set { cooldown = value; }
    }
	
    public float ShootAngle
    {
      get { return shootAngle; }
      set { shootAngle = value; }
    }
	
    public TurretState State
    {
      get { return state; }
      set { state = value; }
    }
	
    public Vector3 Origin
    {
      get { return origin; }
      set { origin = value; }
    }
	
    public Quaternion BaseRotation
    {
      get { return baseRotation; }
      set { baseRotation = value; }
    }
    #endregion

    protected string textureName;
    public Texture2D texture;

    public Turret(Game game)
      : base(game)
    {
      this.rotToPlayer = Matrix.Identity;
    }

    #region Overrides    
    public override void Clone(GameObject obj)
    {
      base.Clone(obj);

      Vector3 myForward = -this.AttachableModifier.Connectors[0].Pivot;
      Vector3 cross = Vector3.Cross(myForward, Vector3.Forward);
      float angle = (float)Math.Acos(Vector3.Dot(myForward, Vector3.Forward));
      if (cross.Length() > 0)
        cross.Normalize();
      Quaternion q = Quaternion.CreateFromAxisAngle(cross, angle);

      ((Turret)obj).state = TurretState.Idle;
      ((Turret)obj).Cooldown = this.Cooldown;
      ((Turret)obj).Range = this.Range;
      // Make cooldown a little bit more random, otherwise all turrets will shoot at the same rate
      ((Turret)obj).startCooldown = this.cooldown + (int)(this.Game.Random.NextDouble() * 100);
      ((Turret)obj).Origin = this.origin;
      ((Turret)obj).ShootAngle = this.ShootAngle;
      ((Turret)obj).ProjectileColor = this.ProjectileColor;
      ((Turret)obj).BaseRotation = q;
      ((Turret)obj).TurnSpeed = this.TurnSpeed;
      ((Turret)obj).ProjectileType = this.projectileType;

      obj.Effect = this.Effect.Clone(this.Game.GraphicsDevice);
      ((Turret)obj).texture = this.Game.ContentHandler.GetAsset<Texture2D>(textureName);
      obj.Effect.Parameters["colorOverlay"].SetValue(new Vector4(1));
      obj.Effect.Parameters["Texture"].SetValue(((Turret)obj).texture);
    }

    public override void ReadSpecificsFromXml(XmlNodeList xmlNodeList)
    {
      this.TurnSpeed = 1.0f;
      foreach (XmlNode node in xmlNodeList)
      {
        switch (node.Name.ToLower())
        {
          case "shoot_angle":
            this.shootAngle = MathHelper.ToRadians(float.Parse(node.InnerXml));
            break;
          case "cooldown":
            this.Cooldown = int.Parse(node.InnerXml);
            break;
          case "range":
            this.Range = float.Parse(node.InnerXml);
            break;
          case "projectile":
            this.ProjectileType = node.InnerXml;
            break;
          case "turn_speed":
            this.TurnSpeed = float.Parse(node.InnerXml);
            break;
          case "projectile_color":
            this.projectileColor = new Color(PublicMethods.handleVector3(node));
            break;
          case "projectile_origin":
            this.origin = PublicMethods.handleVector3(node);
            break;
          case "texture":
            this.textureName = node.InnerXml;
            break;
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (!(this.Game.GameState.GetType() == typeof(MainGameState)) ||
        (((MainGameState)this.Game.GameState).Status == MainGameState.MainGameStateStatus.WAITING) ||
        this.Game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_ROCKETCOMBO) ||
        this.Game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_CHARGING) ||
        this.Game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_LASERBEAMBATTLE) ||
        !this.IsVisible)
        return;

      base.Update(gameTime);      
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
      this.cooldown = startCooldown;

      if (this.Game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_FRENZY))
      {
        // When frenzy behaviour is enabled shoot forward
        UpdateFrenzy(gameTime);
        return;
      }

      if (isRotating)
      {
        updateRotating(gameTime);
      }

      Vector3 diff = Vector3.Zero;
      if (IsPlayerInRange(out diff))
      {
        Shoot(
          Vector3.Transform(this.Position, this.Game.BehaviourHandler.Transform) +
          Vector3.Transform(new Vector3(0,0,this.Game.GameHandler.Player.Distance), this.Rotation));

        if (!isRotating)
        {
          isRotating = true;
          percentage = 0.0f;

          float angle = (float)Math.Acos(Vector3.Dot(Vector3.Backward, diff));
          Vector3 cross = Vector3.Cross(Vector3.Backward, diff);
          cross.Normalize();

          // Take inverse of current bossrotation 
          Quaternion bossRotation = Quaternion.Inverse(Quaternion.CreateFromRotationMatrix(this.Game.BehaviourHandler.Transform));          
          // Multiply rotation to player and inverse of boss rotation to rotate to player
          rotateTo = 
            bossRotation *
            Quaternion.CreateFromAxisAngle(cross, angle);
          // Rotate from current rotation
          rotateFrom = this.Rotation;
        }
      }
      else
      {
        this.isRotating = false;
        this.Rotation = BaseRotation;
      }
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.IsVisible)
        return;
      
      this.Effect.Begin();

      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        Matrix m1 = this.Game.GameHandler.World *
          this.transforms[mesh.ParentBone.Index] *
          Matrix.CreateFromQuaternion(this.Rotation) *
          Matrix.CreateScale(this.Scale) *
          Matrix.CreateTranslation(this.Position);
        Matrix m2 = this.Game.BehaviourHandler.Transform;

        this.Effect.Parameters["matWorld"].SetValue(m1 * m2);

        this.GraphicsDevice.Indices = mesh.IndexBuffer;

        foreach (ModelMeshPart part in mesh.MeshParts)
        {
          this.GraphicsDevice.VertexDeclaration = part.VertexDeclaration;
          this.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, 0, part.VertexStride);

          foreach (EffectPass pass in this.Effect.CurrentTechnique.Passes)
          {
            pass.Begin();
            this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0,
              part.NumVertices, part.StartIndex, part.PrimitiveCount);
            pass.End();
          }
        }
      }
      this.Effect.End();
    }
    #endregion

    private void UpdateFrenzy(GameTime gameTime)
    {     
      // Decrease cooldown of turret according to time multiplier in frenzy behaviour
      BossBehaviour behaviour;
      if (this.Game.BehaviourHandler.GetBehaviour(
        BossBehaviourType.BBT_FRENZY,
        out behaviour))
      {
        float multiplier = ((FrenzyBehaviour)behaviour).FireMultiplier;

        this.cooldown = startCooldown / multiplier;
      }
      
      Vector3 diff = Vector3.Zero;
      if (IsPlayerInRange(out diff))
      {
        // Shoot at player        
        Shoot(this.Game.GameHandler.Player.Position);
      }
      else
      {
        // Determine forward vector
        Vector3 myForward = -this.AttachableModifier.Connectors[0].Pivot;
        Vector3 transformedDir = Vector3.Transform(myForward, this.Game.BehaviourHandler.Transform);
        transformedDir *= this.Game.GameHandler.Player.Distance;
        // Shoot forward
        Vector3 myPosition = Vector3.Transform(this.Position, this.Game.BehaviourHandler.Transform);
        Shoot(myPosition + transformedDir);
      }
    }

    private void updateRotating(GameTime gameTime)
    {
      percentage += this.turnSpeed;

      this.Rotation = Quaternion.Lerp(rotateFrom, rotateTo, percentage);

      if (percentage >= 1.0f)
      {
        isRotating = false;
      }
    }

    protected Vector3 DetermineProjectileSpawnPosition()
    {
      return Vector3.Transform(this.Position +
              Vector3.Transform(this.Origin, this.Rotation),
              this.Game.BehaviourHandler.Transform);
    }

    protected bool IsPlayerInRange(out Vector3 diff)
    {
      // Calculate angle
      Vector3 myForward = -this.AttachableModifier.Connectors[0].Pivot;
      Vector3 transformedForward = Vector3.Transform(myForward, this.Game.BehaviourHandler.Transform);
      diff = this.Game.GameHandler.Player.Position - this.Position;
      diff.Normalize();

      float angle = (float)Math.Acos(Vector3.Dot(transformedForward, diff));

      // Calculate correct distance
      float lengthToPlayer = (this.Game.GameHandler.Player.Position - this.Position).Length();      

      // Is player within a certain angle and range?
      return (angle <= shootAngle) && lengthToPlayer <= range;
    }

    public abstract void Shoot(Vector3 position);
  }
}