using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tier.Source.Misc;
using Tier.Source.Objects;
using Tier.Source.Helpers;
using BloomPostprocess;
using Tier.Source.Helpers.Cameras;

namespace Tier.Source.Handlers.BossBehaviours
{
  
  public class ChargeBehaviour : BossBehaviour
  {
    #region Privates
    private Vector2 dodgeDirection;
    private Vector3 chargeTo;    
    private Vector3 position;
    private float timeElapsed;    
    private ChargeBehaviourState state;    
    private Matrix rotateTo;
    private float timeSinceDirectionRotation;
    private int currentDirection;
    private Sprite spriteDirection;
    private bool playerDodged;
    #endregion
    
    private enum ChargeBehaviourState
    {
      CBS_ROTATE_TO_ORIGIN,
      CBS_ROTATE_TO_PLAYER,
      CBS_CHARGING,
      CBS_PERFECTDODGE,
      CBS_RETURNING
    };
    private Vector3 positionChargeTo;
    private Vector3 positionPerfectDodgeCamera;    
    private float timeElapsedSinceCharging;
    private PositionalCamera perfectDodgeCamera;
    private Camera previousCamera;

    public ChargeBehaviour(TierGame game, BossBehaviourHandler handler)
      : base(game, handler, BossBehaviourType.BBT_CHARGING)
    {            
      this.Transform = Matrix.Identity;
      this.rotateTo = Matrix.Identity;
      this.perfectDodgeCamera = new PositionalCamera(game);
    }
   
    #region ChangeState
    private void ChangeStateRotatingToPlayer()
    {
      this.game.SoundHandler.Play("RobotTurnforward");
      
      // Hide certain interface objects
      this.game.InterfaceHandler.Hide();

      Vector3 v1 = Vector3.Forward;
      Vector3 v2 = this.game.GameHandler.Player.Position;
      this.positionChargeTo = v2;
      v2.Normalize();

      this.rotateTo = PublicMethods.GetShortestRotationMatrixBetweenVectors(ref v1, ref v2);

      this.timeSinceDirectionRotation = 0; this.currentDirection = 0;
      // Initialize sprite
      this.spriteDirection = this.game.TextSpriteHandler.CreateSprite(
        this.game.ContentHandler.GetAsset<Texture2D>("Xbox-lts-Up"),
        new Vector2(10, this.game.GraphicsDevice.PresentationParameters.BackBufferHeight - 150));
    }

    private void ChangeStateCharging()
    {      
      this.position = Vector3.Zero;
      // Determine dodge direction for player
      determineDirectionVector();
      this.game.SoundHandler.Play("Charge");
    }

    private void ChangeStatePerfectDodge()
    {
      this.timeElapsedSinceCharging = this.timeElapsed;
      this.positionPerfectDodgeCamera =
        this.game.GameHandler.Player.Position +
        Vector3.Transform(
          new Vector3(0, 0, 12.5f),
          this.game.GameHandler.Player.Rotation
        );

      this.perfectDodgeCamera.Position = new Vector3(0, 0, 12.5f);
      this.perfectDodgeCamera.Target = this.game.GameHandler.Player.Position;

      this.previousCamera = this.game.GameHandler.Camera;
      this.game.GameHandler.Camera = this.perfectDodgeCamera;
    }

    private void ChangeStateReturning()
    {
      if (!playerDodged)
      {
        // Player was too late, lose health
        this.game.GameHandler.AddDamagedObject(
          this.game.GameHandler.Player, this.game.Options.BossChargeDamage);
      }

      this.game.GameHandler.Player.MovableModifier.Velocity = Vector3.Zero;
      this.game.Speed = 1.0f;
      this.game.TextSpriteHandler.RemoveSprite(this.spriteDirection);
    }

    private void ChangeState(ChargeBehaviourState newstate)
    {
      switch (newstate)
      {
        case ChargeBehaviourState.CBS_ROTATE_TO_PLAYER:
          ChangeStateRotatingToPlayer();
          break;
        case ChargeBehaviourState.CBS_CHARGING:
          ChangeStateCharging();
          break;
        case ChargeBehaviourState.CBS_RETURNING:
          ChangeStateReturning();          
          break;      
        case ChargeBehaviourState.CBS_PERFECTDODGE:
          ChangeStatePerfectDodge();
          break;
      }

      this.timeElapsed = 0;
      this.state = newstate;
    }

    private void determineDirectionVector()
    {
      Random r = new Random(System.DateTime.Now.Millisecond);

      this.chargeTo = this.game.GameHandler.Player.Position;

      switch (r.Next(4))
      {
        case 0:
          this.dodgeDirection = Vector2.UnitX;
          this.spriteDirection.Texture = this.game.ContentHandler.GetAsset<Texture2D>("Xbox-lts-Right");
          break;
        case 1:
          this.dodgeDirection = -Vector2.UnitX;
          this.spriteDirection.Texture = this.game.ContentHandler.GetAsset<Texture2D>("Xbox-lts-Left");
          break;
        case 2:
          this.dodgeDirection = Vector2.UnitY;
          this.spriteDirection.Texture = this.game.ContentHandler.GetAsset<Texture2D>("Xbox-lts-Up");
          break;
        case 3:
          this.dodgeDirection = -Vector2.UnitY;
          this.spriteDirection.Texture = this.game.ContentHandler.GetAsset<Texture2D>("Xbox-lts-Down");
          break;
      }
    }
    #endregion

    #region Update
    public override void Update(GameTime gameTime)
    {
      this.TTL -= gameTime.ElapsedGameTime.Milliseconds;
      timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      switch (state)
      {
        case ChargeBehaviourState.CBS_ROTATE_TO_PLAYER:
          this.game.GameHandler.Player.MovableModifier.Velocity = Vector3.Zero;
          UpdateRotatingToPlayer(gameTime);
          break;
        case ChargeBehaviourState.CBS_ROTATE_TO_ORIGIN:
          this.game.GameHandler.Player.MovableModifier.Velocity = Vector3.Zero;
          UpdateRotatingToOrigin(gameTime);
          break;
        case ChargeBehaviourState.CBS_CHARGING:
          UpdateCharging(gameTime);
          break;
        case ChargeBehaviourState.CBS_RETURNING:
          UpdateReturning(gameTime);
          break;
        case ChargeBehaviourState.CBS_PERFECTDODGE:
          UpdatePerfectDodge(gameTime);
          break;
      }
    }

    private void DeterminePlayerInput(GameTime gameTime)
    {
      // Determine if player has pressed the right direction
      Vector2 dir = GamePad.GetState(this.game.MainControllerIndex).ThumbSticks.Left;
      Vector2 check = dir * dodgeDirection;
      if ((check.X == 1 || check.Y == 1) && !playerDodged)
      {
        string scoreText;
        int score;
        this.playerDodged = true;
        this.game.SoundHandler.Play("Dodge");

        if (timeElapsed >= this.game.Options.Charge_JustInTime)
        {           
          scoreText = String.Format("Perfect dodge! {0}",
            this.game.Options.Charge_PerfectDodgePoints);
          score = this.game.Options.Charge_PerfectDodgePoints;

          ChangeState(ChargeBehaviourState.CBS_PERFECTDODGE);
        }
        else
        {
          scoreText = String.Format("Dodge! {0}", this.game.Options.Charge_DodgePoints);
          score = this.game.Options.Charge_DodgePoints;
        }

        VisualScore s = new VisualScore(scoreText, new Vector2(500, 500), Color.Yellow, this.game);
        s.Score = score;
        this.game.ScoreHandler.AddVisualScore(s);
      }
    }

    private void UpdateCharging(GameTime gameTime)
    {
      if (timeElapsed >= this.game.Options.Charge_TimeToCharge)
      {
        ChangeState(ChargeBehaviourState.CBS_RETURNING);        
        return;      
      }

      DeterminePlayerInput(gameTime);

      if (playerDodged)
      {        
        this.game.GameHandler.Player.MovableModifier.Velocity =
          new Vector3(dodgeDirection * new Vector2(0.5f, -0.5f), 0) * 2;
      }

      float amount = timeElapsed / this.game.Options.Charge_TimeToCharge;

      this.position = Vector3.Lerp(Vector3.Zero, this.positionChargeTo, amount);
      this.Transform =
        rotateTo * 
        Matrix.CreateTranslation(this.position);
    }

    private void UpdatePerfectDodge(GameTime gameTime)
    {
      if (this.timeElapsed >= 2000)
      {
        this.timeElapsed = this.timeElapsedSinceCharging;
        this.game.GameHandler.Camera = this.previousCamera;

        this.state = ChargeBehaviourState.CBS_CHARGING;
        this.game.Speed = 0.1f;
      }

      this.game.GameHandler.Player.MovableModifier.Velocity = Vector3.Zero;
      
      float amount = this.timeElapsedSinceCharging / this.game.Options.Charge_TimeToCharge;
      float amountCamRotation = this.timeElapsed / 2000.0f;

      this.perfectDodgeCamera.Position = Vector3.Transform(
        positionPerfectDodgeCamera,
        Quaternion.CreateFromAxisAngle(Vector3.Up, (MathHelper.Pi * 2) * amountCamRotation));

      this.Transform =
        rotateTo *
        Matrix.CreateTranslation(Vector3.Lerp(Vector3.Zero, this.positionChargeTo, amount));
    }
    
    private void UpdateReturning(GameTime gameTime)
    {
      if (timeElapsed >= this.game.Options.Charge_TimeToReturn)
      {
        /*int charges = this.game.Options.Charge_BaseNumberOfCharges +
                          (int)Math.Floor(this.game.GameHandler.CurrentLevel *
                            this.game.Options.Charge_ChargesPerLevel);

        if (++chargeCount >= charges)
        {
          chargeCount = 0;
          changeState(ChargeBehaviourState.RotateToOrigin);
        }
        else
        {
          playerDodged = false;
          changeState(ChargeBehaviourState.RotatingToPlayer);
        }
        */

        this.game.SoundHandler.Play("RobotTurnbackward");
        ChangeState(ChargeBehaviourState.CBS_ROTATE_TO_ORIGIN);
        return;
      }


      float amount = timeElapsed / this.game.Options.Charge_TimeToReturn;
      this.position = Vector3.Lerp(this.positionChargeTo, Vector3.Zero, amount);
      this.Transform =
        rotateTo *
        Matrix.CreateTranslation(this.position);
    }

    private void UpdateRotatingToPlayer(GameTime gameTime)
    {
      timeSinceDirectionRotation += gameTime.ElapsedGameTime.Milliseconds;

      if (timeElapsed >= this.game.Options.Charge_TimeToRotate)
      {
        this.Transform = this.rotateTo;
        ChangeState(ChargeBehaviourState.CBS_CHARGING);
        return;
      }

      // Rotate direction sprites in preparation for random direction
      if (timeSinceDirectionRotation >= 100.0f)
      {
        timeSinceDirectionRotation = 0;
        switch (++currentDirection)
        {
          case 0:
            this.spriteDirection.Texture = this.game.ContentHandler.GetAsset<Texture2D>("Xbox-lts-Up");
            break;
          case 1:
            this.spriteDirection.Texture = this.game.ContentHandler.GetAsset<Texture2D>("Xbox-lts-Right");
            break;
          case 2:
            this.spriteDirection.Texture = this.game.ContentHandler.GetAsset<Texture2D>("Xbox-lts-Down");
            break;
          case 3:
            this.spriteDirection.Texture = this.game.ContentHandler.GetAsset<Texture2D>("Xbox-lts-Left");
            break;
          default:
            currentDirection = -1;
            break;
        }
      }

      float amount = timeElapsed / this.game.Options.Charge_TimeToRotate;
      this.Transform = Matrix.Lerp(Matrix.Identity, this.rotateTo, amount);
    }

    private void UpdateRotatingToOrigin(GameTime gameTime)
    {
      if (this.timeElapsed >= this.game.Options.Charge_TimeToRotate)
      {
        this.Transform = Matrix.Identity;
        Disable();
        // Return interface to default
        this.game.InterfaceHandler.Show();
      }

      float amount = timeElapsed / this.game.Options.Charge_TimeToRotate;
      this.Transform = Matrix.Lerp(rotateTo, Matrix.Identity, amount);
    }
    #endregion

    public override void Disable()
    {
      base.Disable();
      this.handler.IsLocked = false;

      if (this.spriteDirection != null)
        this.spriteDirection.IsVisible = false;
    }

    public override void Enable()
    {
      this.handler.IsLocked = true;
      this.IsEnabled = true;
      this.playerDodged = false;
      this.TTL = game.Options.Charge_TimeTotal;
      this.handler.DisableBehaviour(BossBehaviourType.BBT_ROAMING);
      this.handler.ResetBehaviour(BossBehaviourType.BBT_ROAMING);
      this.handler.DisableBehaviour(BossBehaviourType.BBT_ROTATING);
      this.handler.ResetBehaviour(BossBehaviourType.BBT_ROTATING);
      ChangeState(ChargeBehaviourState.CBS_ROTATE_TO_PLAYER);
    }
  }
}