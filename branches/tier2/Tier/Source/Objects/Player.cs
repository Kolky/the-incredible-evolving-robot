using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Misc;
using pjEngine.Helpers;
using Tier.Source.Objects.Projectiles;
using Tier.Source.Handlers;
using Tier.Source.Objects.PlayerWeapons;
using Microsoft.Xna.Framework.Input;

namespace Tier.Source.Objects
{
  /// <summary>
  /// Modifies certain aspects of Player behaviour
  /// </summary>
  public abstract class PlayerModifier
  {
    protected TierGame game;
    protected Player player;

    public PlayerModifier(TierGame game, Player player)
    {
      this.player = player;
      this.game = game;
    }

    public abstract void Start();     
    public abstract void Stop();
    public abstract void Update(GameTime gameTime);
  }

  public class PlayerQuadDamageModifier : PlayerModifier
  {
    private int ttl;
    private int timeElapsed;

    public PlayerQuadDamageModifier(TierGame game, Player player)
      : base(game, player)
    {
    }

    public override void Start()
    {
      this.timeElapsed = 0;
      this.ttl = game.Options.QuadDamage_TTL;
      this.player.IsQuadDamage = true;

      this.game.ProjectileHandler.SetQuadDamageColor();
    }

    public override void Stop()
    {
      this.player.IsQuadDamage = false;

      // Reset all projectilecolors in ProjectileHandler
      this.game.ProjectileHandler.ResetColor();
    }

    public override void Update(GameTime gameTime)
    {
      timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      if (timeElapsed >= ttl)
      {
        player.RemovePlayerModifier(this);
      }
    }
  }

  public class Player : GameObject
  {
    #region Properties
    private float distance;
    private float speedCircle;
    private float speedZoom;
    private float spread;
    private float minSpread;
    private GameObject target;
    private bool isInvulnerable;
    private int weaponCooldown;
    private bool isQuadDamage;
    private Texture2D diffuseMap;
    private float timeElapsed;
    private PlayerWeapon weapon;
    private List<PlayerModifier> playerModifiers;
    private List<PlayerModifier> toBeRemovedModifiers;
    private bool isLifeGain;

    public bool IsQuadDamage
    {
      get { return isQuadDamage; }
      set { isQuadDamage = value; }
    }

    public int WeaponCooldown
    {
      get { return weaponCooldown; }
      set { weaponCooldown = value; }
    }

    public bool IsInvulnerable
    {
      get { return isInvulnerable; }
      set { isInvulnerable = value; }
    }

    public GameObject Target
    {
      get { return target; }
      set { target = value; }
    }

    /// <summary>
    /// Missile spread.
    /// </summary>
    public float Spread
    {
      get { return spread; }
      set
      {
        spread = value;
        minSpread = -spread / 2.0f;
      }
    }

    public float MovementSpeedZoom
    {
      get { return speedZoom; }
      set { speedZoom = value; }
    }

    public float MovementSpeedCircle
    {
      get { return speedCircle; }
      set { speedCircle = value; }
    }

    public float Distance
    {
      get { return distance; }
      set { distance = value; }
    }    
    
    public bool IsLifeGain
    {
      set { this.isLifeGain = value; }
      get { return isLifeGain; }
    }

    public PlayerWeapon PlayerWeapon
    {
      get { return this.weapon; }
    }
    #endregion

    public Player(TierGame game)
      : base(game)
    {
      this.distance = -40.0f;
      this.Rotation = Quaternion.Identity;
      this.timeElapsed = 0;
      this.playerModifiers = new List<PlayerModifier>();
      this.toBeRemovedModifiers = new List<PlayerModifier>();

      SwitchWeapon(new PlayerPlasmaWeapon(game));
    }

    public void AddHealth(int health)
    {
      this.DestroyableModifier.Health += health;

      if (this.DestroyableModifier.Health > 100)
        this.DestroyableModifier.Health = 100;
    }

    public void AddPlayerModifier(PlayerModifier modifier)
    {
      // Don't add modifiers of same type, just reset it
      foreach (PlayerModifier mod in this.playerModifiers)
      {
        if (mod.GetType() == modifier.GetType())
        {
          mod.Start();
          return;
        }
      }

      modifier.Start();
      this.playerModifiers.Add(modifier);
    }

    public void RemovePlayerModifier(PlayerModifier modifier)
    {
      modifier.Stop();

      this.toBeRemovedModifiers.Add(modifier);
    }

    private void UpdatePlayerModifiers(GameTime gameTime)
    {
      foreach (PlayerModifier mod in this.playerModifiers)
      {
        mod.Update(gameTime);
      }

      foreach (PlayerModifier mod in this.toBeRemovedModifiers)
      {
        this.playerModifiers.Remove(mod);
      }
      this.toBeRemovedModifiers.Clear();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      UpdatePlayerModifiers(gameTime);

      if (this.isLifeGain)
      {
        float amount =
          this.Game.Options.PlayerHealthGainPerSecond * 
          (gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

        this.DestroyableModifier.Health += amount;
        if (this.DestroyableModifier.Health > 100)
          this.DestroyableModifier.Health = 100;
        this.isLifeGain = false;
      }

      // Disable player input when certain behaviours are active
      if (!(this.Game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_LASERBEAMBATTLE) ||
        this.Game.BehaviourHandler.IsBehaviourEnabled(BossBehaviourType.BBT_CHARGING)))
      {
        GamePadState state = GamePad.GetState(this.Game.MainControllerIndex);
        this.MovableModifier.Velocity = new Vector3(state.ThumbSticks.Left.X, -state.ThumbSticks.Left.Y, 0.0f);

        timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

        if (this.weapon.GetType() == typeof(PlayerWeaponLaserbeam))
        {
          ((PlayerWeaponLaserbeam)this.weapon).ResetBeam();
        }
        this.weapon.Update(gameTime);

        float length = this.Game.BossCompositionHandler.GetLength();

        if (length <= 40.0f)
          length = 40.0f;

        this.Game.GameHandler.Player.Distance = length;
      }

      Vector3 normalizedMovement =
        this.MovableModifier.Velocity * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
      this.Rotation *=
        Quaternion.CreateFromYawPitchRoll(normalizedMovement.X, normalizedMovement.Y, 0);
      this.Position = Vector3.Transform(new Vector3(0, 0, this.distance), this.Rotation);
    }

    public void UpgradeWeapon()
    {
      this.weapon.Upgrade();
    }

    public void SwitchWeapon(PlayerWeapon newweapon)
    {
      if (this.weapon != null && 
        this.weapon.GetType() == newweapon.GetType())
        return;
      
      if(this.weapon != null)
      {
        this.weapon.RemoveWeapon();
      }

      this.weapon = newweapon;
      this.weapon.AttachWeapon();
    }

    /// <summary>
    /// Fires a Laser projectile in a random direction contained in the current spread.
    /// </summary>
    public void Shoot()
    {
      Vector3 farSource = new Vector3(this.Game.InterfaceHandler.GetCrosshairPosition(), 1f);
      Vector3 farPoint = this.Game.GraphicsDevice.Viewport.Unproject(farSource,
        this.Game.GameHandler.Projection,
        this.Game.GameHandler.View,
        Matrix.CreateTranslation(this.Game.GameHandler.Camera.Position));

      Vector3 dir = farPoint - this.Game.GameHandler.Camera.Position;
      dir.Normalize();

      weapon.Shoot(dir);
    }

    public override void Clone(GameObject obj)
    {
      base.Clone(obj);

      ((Player)obj).Effect = this.Effect.Clone(this.Game.GraphicsDevice);
      ((Player)obj).MovementSpeedCircle = this.MovementSpeedCircle;
      ((Player)obj).MovementSpeedZoom = this.MovementSpeedZoom;
      ((Player)obj).diffuseMap = this.diffuseMap;
      ((Player)obj).WeaponCooldown = this.WeaponCooldown;
      ((Player)obj).Effect.Parameters["Texture"].SetValue(diffuseMap);
      ((Player)obj).Effect.Parameters["colorOverlay"].SetValue(new Vector4(1));
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);

      if (this.Effect != null)
        this.Effect.Dispose();
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.IsVisible || this.Effect == null)
        return;

      this.Effect.Parameters["matView"].SetValue(this.Game.GameHandler.View);
      this.Effect.Parameters["matProj"].SetValue(this.Game.GameHandler.Projection);

      this.Effect.Begin();

      foreach (ModelMesh mesh in Model.Meshes)
      {
        this.Effect.Parameters["matWorld"].SetValue(
            this.Game.GameHandler.World *
            this.transforms[mesh.ParentBone.Index] *
            Matrix.CreateFromQuaternion(this.Rotation) *
            Matrix.CreateScale(this.Scale) *
            Matrix.CreateTranslation(this.Position));

        this.Effect.CurrentTechnique.Passes[0].Begin();

        foreach (ModelMeshPart meshPart in mesh.MeshParts)
        {
          this.GraphicsDevice.VertexDeclaration = meshPart.VertexDeclaration;
          this.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, meshPart.StreamOffset, meshPart.VertexStride);
          this.GraphicsDevice.Indices = mesh.IndexBuffer;
          this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
            meshPart.BaseVertex, 0,
            meshPart.NumVertices,
            meshPart.StartIndex,
            meshPart.PrimitiveCount);
        }
      }

      this.Effect.CurrentTechnique.Passes[0].End();
      this.Effect.End();
    }

    public override void ReadSpecificsFromXml(System.Xml.XmlNodeList xmlNodeList)
    {
      foreach (XmlNode node in xmlNodeList)
      {
        switch (node.Name.ToLower())
        {
          case "movementspeed_circle":
            this.MovementSpeedCircle = float.Parse(node.InnerXml);
            break;
          case "movementspeed_zoom":
            this.MovementSpeedZoom = float.Parse(node.InnerXml);
            break;
          case "player_weapon_cooldown":
            this.weaponCooldown = int.Parse(node.InnerXml);
            break;
          case "diffuse_map":
            diffuseMap = this.Game.ContentHandler.GetAsset<Texture2D>(node.InnerText);
            break;
        }
      }
    }
  }
}