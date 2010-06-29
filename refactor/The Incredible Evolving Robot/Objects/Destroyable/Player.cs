using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Tier.Controls;
using Tier.Handlers;
using Tier.Misc;
using Tier.Objects.Basic;
using Tier.Objects.Attachable;
using Tier.Objects.Attachable.Weapons;

namespace Tier.Objects.Destroyable
{
    public class Player : AttachableObject
    {
        #region Properties
        private Sphere sphere;
        private Weapon weapon;
        public Sphere Sphere
        {
            get { return sphere; }
            set { sphere = value; }
        }

        private float depthToShereRadius;

        public float DepthToShereRadius
        {
            get { return depthToShereRadius; }
            set { depthToShereRadius = value; }
        }

        private Vector3 upVector, forwardVector, rightVector, movement;

        #endregion

        public Player(Game game, Sphere sphere)
            : base(game, true, null)
        {
            this.ModelName = "Ship";
            this.Model = TierGame.ContentHandler.GetModel(this.ModelName);
            this.ModelMeta = TierGame.ContentHandler.GetModelMeta(ModelName); //(IsCollidable) ? TierGame.ContentHandler.GetModelMeta(ModelName) : null;
            this.Sphere = sphere;
            this.Scale = 0.003f;
            this.depthToShereRadius = 0.0f;
            this.Position.Coordinate = new Vector3(0f, 0f, this.Sphere.getRadius() + this.depthToShereRadius);
            this.Position.Front = Quaternion.CreateFromYawPitchRoll(0f, 0f, 0f);
            this.weapon = new LaserGun(game, this);
            GameHandler.ObjectHandler.AddObject(this.weapon);
            this.addBoundingShere(0.4f, new Vector3(0f, 0f, 0.0f));

            this.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.upVector = Vector3.Transform(Vector3.Up, this.Position.Front);
            this.forwardVector = Vector3.Transform(Vector3.Forward, this.Position.Front);
            this.rightVector = Vector3.Transform(Vector3.Right, this.Position.Front);

            float elapsedPartOfSecond = gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            this.movement = Vector3.Zero;
            if (TierGame.Input.GetType() == typeof(InputXBOX))
            {
                // Left Stick - Y-as
                if (TierGame.InputXBOX.getStickVector2(GamePadStick.LEFT).Y > 0f)
                    this.movement.Y = -Options.Player.Speed;
                else if (TierGame.InputXBOX.getStickVector2(GamePadStick.LEFT).Y < 0f)
                    this.movement.Y = Options.Player.Speed;

                // Left Trigger      
                if (TierGame.InputXBOX.getTriggerDepth(GamePadTrigger.LEFT) > Options.Controls.TriggerDepth)
                {
                    // Left Stick - X-as
                    if (TierGame.InputXBOX.getStickVector2(GamePadStick.LEFT).X < 0f)
                        this.movement.Z = -Options.Player.Speed * 4f;
                    else if (TierGame.InputXBOX.getStickVector2(GamePadStick.LEFT).X > 0f)
                        this.movement.Z = Options.Player.Speed * 4f;
                }
                else if (TierGame.InputXBOX.getTriggerDepth(GamePadTrigger.LEFT) < Options.Controls.TriggerDepth)
                {
                    // Left Stick - X-as
                    if (TierGame.InputXBOX.getStickVector2(GamePadStick.LEFT).X > 0f)
                        this.movement.X = Options.Player.Speed;
                    else if (TierGame.InputXBOX.getStickVector2(GamePadStick.LEFT).X < 0f)
                        this.movement.X = -Options.Player.Speed;
                }

                if (TierGame.InputXBOX.getTriggerDepth(GamePadTrigger.RIGHT) > Options.Controls.TriggerDepth)
                    this.weapon.Fire();
            }
            else
            {
                if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.MOVE_UP, true))
                    this.movement.Y = -Options.Player.Speed;
                else if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.MOVE_DOWN, true))
                    this.movement.Y = Options.Player.Speed;

                if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.MOVE_LEFT, true))
                    this.movement.X = -Options.Player.Speed;
                else if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.MOVE_RIGHT, true))
                    this.movement.X = Options.Player.Speed;

                if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.ROLL_LEFT, true))
                    this.movement.Z = Options.Player.Speed * 2f;
                else if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.ROLL_RIGHT, true))
                    this.movement.Z = -Options.Player.Speed * 2f;

                /*
                if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.MOVE_FORWARD, true))
                {
                    this.depthToShereRadius -= 2 * elapsedPartOfSecond;
                    this.Position.CoordinateZ = this.Sphere.getRadius() + this.depthToShereRadius;
                }
                else if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.MOVE_BACKWARD, true))
                {
                    this.depthToShereRadius += 2 * elapsedPartOfSecond;
                    this.Position.CoordinateZ = this.Sphere.getRadius() + this.depthToShereRadius;
                }
                */

                if (TierGame.Input.checkKeyAction(Tier.Controls.GameAction.FIRE, true))
                    this.weapon.Fire();
            }

            if (this.movement != Vector3.Zero)
            {
                this.movement *= elapsedPartOfSecond;
                this.Movement.Rotation = Quaternion.CreateFromAxisAngle(upVector, MathHelper.ToRadians(movement.X)) * Quaternion.CreateFromAxisAngle(rightVector, MathHelper.ToRadians(movement.Y)) * Quaternion.CreateFromAxisAngle(forwardVector, MathHelper.ToRadians(movement.Z));
                this.Position.Coordinate = Vector3.Transform(this.Position.Coordinate, this.Movement.Rotation);
                this.Position.Front = Quaternion.Concatenate(this.Position.Front, this.Movement.Rotation);
            }

            this.UpdateBoundingObjects();
            base.Update(gameTime);
        }

        public override void Initialize()
        {
            base.RotationFix = Matrix.CreateFromAxisAngle(Vector3.UnitY, MathHelper.Pi);

            foreach (ModelMesh mesh in this.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.DiffuseColor = new Vector3(0.45f, 1f, 0.45f);
                }
            }
            base.Initialize();
        }
    }
}
