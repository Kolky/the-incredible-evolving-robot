using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace Tier.Source.Objects.Powerups
{
  public enum PowerupState
  {
    PS_SPAWNING, PS_SPAWNED, PS_SPAWNED_LOCKED
  };

  public abstract class Powerup : GameObject
  {
    #region Properties    
    private PowerupState state;
    private Vector3 startPosition;
    private Vector3 newPosition;
    private float amount;
    protected Texture2D texture;
    protected Quaternion positionRotation;
    #endregion

    public Powerup(TierGame game)
      : base(game)
    {    }

    public void ChangeState(PowerupState newstate)
    {
      switch (newstate)
      {
        case PowerupState.PS_SPAWNING:
          DetermineNewPosition();
          break;
        case PowerupState.PS_SPAWNED:
          break;
      }

      this.state = newstate;
    }

    public override void Clone(GameObject obj)
    {
      base.Clone(obj);
      obj.Effect = this.Effect.Clone(this.Game.GraphicsDevice);
      obj.Effect.Parameters["colorOverlay"].SetValue(Vector4.One);
       obj.Effect.Parameters["Texture"].SetValue(this.texture);
    }

    private void DetermineNewPosition()
    {
      // - The powerup will be spawned on the circle the player moves on. 
      // - Start position is 0,0,40. This position will be rotated randomly across the X and Y axis.
      // - Powerup will move to this location in a given time frame.
      positionRotation =
        Quaternion.CreateFromAxisAngle(Vector3.Left, (MathHelper.Pi * 2) * (float)this.Game.Random.NextDouble()) *
        Quaternion.CreateFromAxisAngle(Vector3.Up, (MathHelper.Pi * 2) * (float)this.Game.Random.NextDouble());
      newPosition = Vector3.Transform(new Vector3(0, 0, this.Game.GameHandler.Player.Distance), positionRotation);
    }

    public abstract void DoPowerup();

    public void Spawn(Vector3 startPosition)
    {
      this.startPosition = startPosition;
      ChangeState(PowerupState.PS_SPAWNING);
    }

    #region Overrides       
    public override void ReadSpecificsFromXml(XmlNodeList xmlNodeList)
    {
      foreach (XmlNode node in xmlNodeList)
      {
        switch (node.Name.ToLower())
        {
          case "texture":
            texture = this.Game.ContentHandler.GetAsset<Texture2D>(node.Attributes["name"].Value);         
            break;
        }
      }
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      base.Update(gameTime);

      this.Effect.Parameters["matView"].SetValue(this.Game.GameHandler.View);
      this.Effect.Parameters["matProj"].SetValue(this.Game.GameHandler.Projection);

      switch (state)
      {
        case PowerupState.PS_SPAWNING:
          amount += 1.0f * (gameTime.ElapsedGameTime.Milliseconds / this.Game.Options.Powerup_SpawnSpeed);
          this.Position = Vector3.Lerp(startPosition, newPosition, amount);

          if(amount >= 1.0f)
          {
            this.Position = newPosition;
            ChangeState(PowerupState.PS_SPAWNED);
          }
          break;
        case PowerupState.PS_SPAWNED:
          // Check if player is near
          Vector3 diff = this.Position - this.Game.GameHandler.Player.Position;
          if (diff.Length() < 2.0f)
          {
            DoPowerup();
            this.Game.StatisticsHandler.AddStatistic(Tier.Source.Handlers.StatisticType.ST_POWERUPCOUNT);
            this.Game.GameHandler.RemoveObject(this, Tier.Source.Handlers.GameHandler.ObjectType.DefaultTextured);
          }

          this.Position = Vector3.Transform(new Vector3(0, 0, this.Game.GameHandler.Player.Distance), positionRotation);

          // Rotate powerup in two seconds across Y axis
          this.Rotation *= Quaternion.CreateFromAxisAngle(
            Vector3.Up,
            (MathHelper.Pi * 2) * gameTime.ElapsedGameTime.Milliseconds / 2000.0f);
          break;
        case PowerupState.PS_SPAWNED_LOCKED:
          // Rotate powerup in two seconds across Y axis
          this.Rotation *= Quaternion.CreateFromAxisAngle(
            Vector3.Up,
            (MathHelper.Pi * 2) * gameTime.ElapsedGameTime.Milliseconds / 2000.0f);
          break;
      }
    }
    #endregion
  }
}
