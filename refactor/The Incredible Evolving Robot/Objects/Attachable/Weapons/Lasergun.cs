using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Tier.Controls;
using Tier.Handlers;
using Tier.Misc;
using Tier.Objects.Attachable;
using Tier.Objects.Destroyable.Projectile;

namespace Tier.Objects.Attachable.Weapons
{
  public class LaserGun : Weapon
  {
    #region Properties
    //random spread in aim piramide
    private float spread = 1.0f;
    private float radSpread=0.0f;
    

    //x&y verschuiving van de  aim piramide
    private float xAim = 0.0f;
    private float yAim = 0.0f;

    //voor rotatie aim&fire
    private Vector3 upVector, rightVector;
    private Quaternion aim;
    #endregion

    public LaserGun(Game game, AttachableObject source)
      : base(game, source)
    {
      this.ModelName = "LaserGun";
      this.Model = TierGame.ContentHandler.GetModel(this.ModelName);
      this.Scale = 1f;
      this.RotationFix = Matrix.CreateFromAxisAngle(Vector3.UnitX, MathHelper.Pi);

      this.Initialize();
    }

    #region Weapon overrides
		public override void Fire()
		{
			if (base.canFire())
			{
				//TierGame.Audio.playSFX3D("photon", this, 82);
				GameHandler.ObjectHandler.AddObject(new LaserCluster(this.Game, this.Position, this.radSpread));
				base.Fire();
			}
		}

    public override void Update(GameTime gameTime)
    {
      this.Position = new Position(Source.Position); //wapen beweegt mee met zijn source
      this.upVector = Vector3.Transform(Vector3.Up, this.Position.Front);
      this.rightVector = Vector3.Transform(Vector3.Right, this.Position.Front);

      this.Aim(gameTime);
      base.Update(gameTime);
    }

    public override void Aim(GameTime gameTime)
    {
      float elapsedPartOfSecond = gameTime.ElapsedGameTime.Milliseconds * 0.001f;

      if (TierGame.Input.GetType() == typeof(InputXBOX))
      {// Right Shoulder
          if (TierGame.Input.checkKeyAction(GameAction.SPREAD_INCREASE, true))
          {
              this.spread += 8.0f * elapsedPartOfSecond;
              if (this.spread > 5)
                  this.spread = 5;
          }

          // Left Shoulder
          if (TierGame.Input.checkKeyAction(GameAction.SPREAD_DECREASE, true))
          {
              this.spread -= 8.0f * elapsedPartOfSecond;
              if (this.spread < 0.4f)
                  this.spread = 0.4f;
          }

          float fact = MathHelper.PiOver2 * 0.5f;

          // Right Stick - X-as
          //this.xAim = fact * (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X * -1);
          this.xAim = fact * (TierGame.InputXBOX.getStickVector2(Tier.Controls.GamePadStick.RIGHT).X * -1);

          // Right Stick - Y-as
          //this.yAim = fact * GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
          this.yAim = fact * TierGame.InputXBOX.getStickVector2(Tier.Controls.GamePadStick.RIGHT).Y;

          /*
          // Right Stick - X-as
          if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X != 0f)
          {
            this.xAim += 1.5f * elapsedPartOfSecond * GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X;
            if (this.xAim > MathHelper.PiOver4)
              this.xAim = MathHelper.PiOver4;
            if (this.xAim < -MathHelper.PiOver4)
              this.xAim = -MathHelper.PiOver4;
          }
       
          // Right Stick - Y-as
          if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y != 0f)
          {
            this.yAim += 1.5f * elapsedPartOfSecond * GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
            if (this.yAim > MathHelper.PiOver4)
              this.yAim = MathHelper.PiOver4;
            if (this.yAim < -MathHelper.PiOver4)
              this.yAim = -MathHelper.PiOver4;
          }
          */
      }
      else
      {
          //if (Keyboard.GetState().IsKeyDown(Keys.E))
          if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.SPREAD_INCREASE, true))
          {
              this.spread += 8.0f * elapsedPartOfSecond;
              if (this.spread > 5)
                  this.spread = 5;
          }
          if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.SPREAD_DECREASE, true))
          {
              this.spread -= 8.0f * elapsedPartOfSecond;
              if (this.spread < 0.4f)
                  this.spread = 0.4f;
          }

          if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.SPREAD_LEFT, true))
          {
              this.xAim += 1.5f * elapsedPartOfSecond;
              if (this.xAim > MathHelper.PiOver4)
                  this.xAim = MathHelper.PiOver4;
          }
          if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.SPREAD_RIGHT, true))
          {
              this.xAim -= 1.5f * elapsedPartOfSecond;
              if (this.xAim < -MathHelper.PiOver4)
                  this.xAim = -MathHelper.PiOver4;
          }
          if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.SPREAD_DOWN, true))
          {
              this.yAim += 1.5f * elapsedPartOfSecond;
              if (this.yAim > MathHelper.PiOver4)
                  this.yAim = MathHelper.PiOver4;
          }
          if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.SPREAD_UP, true))
          {
              this.yAim -= 1.5f * elapsedPartOfSecond;
              if (this.yAim < -MathHelper.PiOver4)
                  this.yAim = -MathHelper.PiOver4;
          }
      }

      this.aim = Quaternion.Concatenate(Quaternion.CreateFromAxisAngle(this.upVector, xAim),
        Quaternion.CreateFromAxisAngle(this.rightVector, yAim));

      //wapen richt niet meer hetzelfde als source, maar voegt eigen richting hier aan toe!
      this.Position.Front = Quaternion.Concatenate(this.Position.Front, this.aim);

      //0.9 standaard breedte 8.5 standaard lengte
      this.radSpread = (float)Math.Atan(((float)this.spread * 0.9f) / 50.5f);
    }

    /// <summary>
    /// override voor spread en wireframe!
    /// </summary>
    /// <param name="gameTime"></param>
    public override void Draw(GameTime gameTime)
    {
      TierGame.Device.RenderState.FillMode = FillMode.WireFrame;

      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        foreach (BasicEffect effect in mesh.Effects)
        {
          effect.View = GameHandler.Camera.View;
          effect.Projection = GameHandler.Camera.Projection;
          effect.World = Matrix.CreateScale(new Vector3(this.spread, this.spread, this.Scale)) *
            this.RotationFix * Matrix.CreateFromQuaternion(this.Position.Front) *
            Matrix.CreateTranslation(this.Position.Coordinate);

        }
        mesh.Draw();
      }

      TierGame.Device.RenderState.FillMode = FillMode.Solid;
    }
    #endregion
  }
}
