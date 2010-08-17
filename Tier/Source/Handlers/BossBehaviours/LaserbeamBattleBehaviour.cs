using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.Helpers.Cameras;
using Microsoft.Xna.Framework;
using Tier.Source.Objects.Projectiles;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Helpers;
using Tier.Source.Objects;

namespace Tier.Source.Handlers.BossBehaviours
{
  public class LaserbeamBattleBehaviour : BossBehaviour
  {
    #region Privates
    private enum LaserbeamBattleState
    {
      LBBS_ROTATECAMERAFORWARD, LBBS_STARTING, LBBS_BATTLE, LBBS_STOPPING, LBBS_ROTATECAMERABACKWARD
    };
    private LaserbeamBattleState state;
    private Camera previousCamera, currentCamera;
    private BlendingCamera blendCamera;
    private Vector3 newPosition;
    private LaserbeamProjectile bossLaser, playerLaser;
    private Vector3 projectilesTarget;
    private Vector3 laserbeamDirection;
    private float timeElapsed;
    private float timeSinceLastBossProgress;
    private GamePadState previousGamepadState, currentGamepadState;
    private bool isLeftTriggerPressed;
    private Matrix rotationToPlayer;
    private Sprite spriteLeftTrigger, spriteRightTrigger;
    private bool isUpdateLeftTriggerAnimation;
    private bool isScaleDownAnimation;
    private float scale;
    private float animationTimeElapsed;
    private int timeSinceLeftTriggerPressed;
    #endregion

    public LaserbeamBattleBehaviour(TierGame game, BossBehaviourHandler handler)
      : base(game, handler, BossBehaviourType.BBT_LASERBEAMBATTLE)
    {
      currentCamera = new PositionalCamera(game);
      blendCamera = new BlendingCamera(game);
    }

    private void ChangeState(LaserbeamBattleState newstate)
    {
      this.timeElapsed = 0;

      switch (newstate)
      {
        case LaserbeamBattleState.LBBS_ROTATECAMERAFORWARD:
          ChangeStateRotateCameraForward();
          break;
        case LaserbeamBattleState.LBBS_ROTATECAMERABACKWARD:
          this.blendCamera.Start(this.currentCamera, this.previousCamera, this.game.Options.LBB_TimeToRotateCam);
          this.game.GameHandler.Camera = this.blendCamera;
          break;
        case LaserbeamBattleState.LBBS_BATTLE:
          ChangeStateBattle();
          break;
        case LaserbeamBattleState.LBBS_STARTING:
          ChangeStateStarting();
          break;
        case LaserbeamBattleState.LBBS_STOPPING:
          ChangeStateStopping();
          break;
      }

      this.state = newstate;
    }

    private void ChangeStateRotateCameraForward()
    {
      this.game.GameHandler.Player.MovableModifier.Velocity = Vector3.Zero;
      // Hide interface
      this.game.InterfaceHandler.Hide();
      // Remove all current projectiles from game
      //RemoveProjectilesFromGame();
      // Change camera and save current one
      previousCamera = this.game.GameHandler.Camera;
      // Determine new boss position and camera position
      newPosition = -this.game.GameHandler.Player.Position;
      // Determine boss rotation (rotate to player)
      rotationToPlayer = Matrix.CreateFromQuaternion( this.game.GameHandler.Player.Rotation);

      this.currentCamera.Target = Vector3.Zero;

      this.currentCamera.Position =
        Vector3.Transform(
        new Vector3(-newPosition.Length() * 2.5f, 0, 0),
        this.game.GameHandler.Player.Rotation);

      Vector3 cameraDiff = -this.currentCamera.Position;
      cameraDiff.Normalize();

      this.currentCamera.ForwardVector = Vector3.Transform(Vector3.Forward, this.game.GameHandler.Player.Rotation);
      this.currentCamera.UpVector = Vector3.Transform(Vector3.Up, this.game.GameHandler.Player.Rotation);
      this.currentCamera.CreateLookatMatrix();

      this.blendCamera.Start(this.previousCamera, this.currentCamera, this.game.Options.LBB_TimeToRotateCam);
      this.game.GameHandler.Camera = this.blendCamera;          
    }

    private void ChangeStateBattle()
    {
      spriteLeftTrigger.IsVisible = spriteRightTrigger.IsVisible = true;

      if (bossLaser == null)
      {
        bossLaser = new LaserbeamProjectile(game, false);
        playerLaser = new LaserbeamProjectile(game, false);

        this.game.ObjectHandler.InitializeFromBlueprint<Projectile>(bossLaser, "LaserBeamProjectile");
        this.game.ObjectHandler.InitializeFromBlueprint<Projectile>(playerLaser, "LaserBeamProjectile");

        // Set correct textures
        bossLaser.LaserbeamTexture = this.game.ContentHandler.GetAsset<Texture2D>("Laserbeam-boss");
        playerLaser.LaserbeamTexture = this.game.ContentHandler.GetAsset<Texture2D>("Laserbeam-player");
      }

      // Reset trigger animation settings
      isScaleDownAnimation = true;
      isUpdateLeftTriggerAnimation = true;
      scale = 1.0f;

      projectilesTarget = Vector3.Zero;
      bossLaser.Position = newPosition;
      bossLaser.Target = playerLaser.Target = projectilesTarget;
      playerLaser.Position = this.game.GameHandler.Player.Position;
      laserbeamDirection = this.game.GameHandler.Player.Position - newPosition;
      laserbeamDirection.Normalize();

      this.game.GameHandler.AddObject(bossLaser);
      this.game.GameHandler.AddObject(playerLaser);

      timeSinceLastBossProgress = 0;
      isLeftTriggerPressed = false;
    }

    private void ChangeStateStarting()
    {
      this.game.SoundHandler.Play("RobotTurnforward");

      if (spriteLeftTrigger == null)
      {
        spriteLeftTrigger = this.game.TextSpriteHandler.CreateSprite(
          this.game.ContentHandler.GetAsset<Texture2D>("xboxControllerLeftTrigger"),
          new Vector2(600, this.game.Options.LBB_TimeToRotateCam));
        spriteRightTrigger = this.game.TextSpriteHandler.CreateSprite(
          this.game.ContentHandler.GetAsset<Texture2D>("xboxControllerRightTrigger"),
          new Vector2(700, this.game.Options.LBB_TimeToRotateCam));

        spriteLeftTrigger.Color = spriteRightTrigger.Color = 
          new Color(Color.White, 0.85f);
      }

      spriteLeftTrigger.IsVisible = spriteRightTrigger.IsVisible = false;

      // Set current camera as active
      this.game.GameHandler.Camera = this.currentCamera;
    }

    private void ChangeStateStopping()
    {
      this.game.SoundHandler.Play("RobotTurnbackward");
      this.bossLaser.IsVisible = false;
      this.playerLaser.IsVisible = false;
      //this.game.GameHandler.RemoveObject(bossLaser, GameHandler.ObjectType.AlphaBlend);
      //this.game.GameHandler.RemoveObject(playerLaser, GameHandler.ObjectType.AlphaBlend);
      this.spriteLeftTrigger.IsVisible = false;
      this.spriteRightTrigger.IsVisible = false;
    }

    private void RemoveProjectilesFromGame()
    {
      List<GameObject> objects = null;
      this.game.GameHandler.GetObjects(out objects, GameHandler.ObjectType.AlphaBlend);

      if (objects != null)
      {
        foreach (GameObject obj in objects)
        {
          if (obj.GetType().IsSubclassOf(typeof(Projectile)))
          {
            this.game.GameHandler.RemoveObject(obj, GameHandler.ObjectType.AlphaBlend);
          }
        }
      }
    }

    private void UpdateTriggerButtonAnimation(GameTime gameTime)
    {
      animationTimeElapsed  += gameTime.ElapsedGameTime.Milliseconds;
      
      // Trigger animation update
      if (isScaleDownAnimation)
      {
        scale =
          1.0f -
          0.1f * (animationTimeElapsed / 100.0f);
      }
      else
      {
        // Scale up
        scale = 
          0.9f +
          0.1f * (animationTimeElapsed / 100.0f);
      }

      if(animationTimeElapsed >= 100.0f)
      {
        isScaleDownAnimation = !isScaleDownAnimation;
        animationTimeElapsed = 0;

        if (isScaleDownAnimation == true)
        {
          // Last button is back to original size, change to next button
          isUpdateLeftTriggerAnimation = !isUpdateLeftTriggerAnimation;
        }
      }

      if (isUpdateLeftTriggerAnimation)
      {
        spriteLeftTrigger.Scale = new Vector2(1) * scale;
      }
      else
      {
        spriteRightTrigger.Scale = new Vector2(1) * scale;
      }
    }

    private void UpdateLaserbeamProgress(GameTime gameTime)
    {
      UpdateTriggerButtonAnimation(gameTime);

      // Boss progress   
      float bossProgress = 
        this.game.Options.LLB_BossBaseProgressPerSecond +
        (this.game.Options.LLB_BossProgressPerSecond * this.game.GameHandler.CurrentLevel);
      projectilesTarget += 
        laserbeamDirection * 
        bossProgress * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

      UpdateLaserbeamProgressControls(gameTime);

      // Update targets of both the player and boss laser beam
      bossLaser.Target = playerLaser.Target = projectilesTarget;
      bossLaser.Color = Color.Red;
      playerLaser.Color = Color.Green;
    }

    private void UpdateLaserbeamProgressControls(GameTime gameTime)
    {
      currentGamepadState = GamePad.GetState(this.game.MainControllerIndex);

      if (isLeftTriggerPressed)
      {
        timeSinceLeftTriggerPressed += gameTime.ElapsedGameTime.Milliseconds;
      }

      if (
        currentGamepadState.Triggers.Left <= 0.1f &&
        currentGamepadState.Triggers.Right > 0.75f && isLeftTriggerPressed)
      {
        if (timeSinceLeftTriggerPressed > 1000)
        {
          timeSinceLeftTriggerPressed = 1000;
        }

        float progress = this.game.Options.LLB_PlayerProgressPerSecond
          - (timeSinceLeftTriggerPressed / 1000.0f) * this.game.Options.LLB_PlayerProgressPerSecond;

        // Update player progress
        projectilesTarget -=
          laserbeamDirection * progress;

        // Reset values
        isLeftTriggerPressed = false;
        timeSinceLeftTriggerPressed = 0;
      }
      if (currentGamepadState.Triggers.Left > 0.75f)
      {
        // Activate timer indicating how long it takes the player to press to other trigger
        isLeftTriggerPressed = true;        
      }
            
      previousGamepadState = currentGamepadState;
    }

    private bool IsBeamCloseToTarget(Vector3 laserbeamPostion, Vector3 targetPosition)
    {
      float length1 = laserbeamPostion.Length();
      float length2 = targetPosition.Length();

      return length1 >= length2;
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      switch (this.state)
      {        
        case LaserbeamBattleState.LBBS_ROTATECAMERAFORWARD:
          if (this.timeElapsed >= this.game.Options.LBB_TimeToRotateCam)
          {
            ChangeState(LaserbeamBattleState.LBBS_STARTING);
          }
          break;
        case LaserbeamBattleState.LBBS_ROTATECAMERABACKWARD:
          if (this.timeElapsed >= this.game.Options.LBB_TimeToRotateCam)
          {
            Disable();            
            this.game.GameHandler.Camera = previousCamera;
            this.game.InterfaceHandler.Show();

          }
          this.Transform = Matrix.Identity;
          break;
        case LaserbeamBattleState.LBBS_BATTLE:
          this.bossLaser.IsVisible = this.playerLaser.IsVisible = true;

          if (IsBeamCloseToTarget(projectilesTarget, this.game.GameHandler.Player.Position))
          {
            // Player loses health
            ChangeState(LaserbeamBattleState.LBBS_STOPPING);
            break;
          }
          else if (IsBeamCloseToTarget(projectilesTarget, newPosition))
          {
            // Boss lost battle
            ChangeState(LaserbeamBattleState.LBBS_STOPPING);
            break;
          }

          // Update the progress of the battle
          UpdateLaserbeamProgress(gameTime);

          this.Transform = 
            rotationToPlayer *
            Matrix.CreateTranslation(newPosition);
          break;
        case LaserbeamBattleState.LBBS_STARTING:
          {
            if (this.timeElapsed >= this.game.Options.LBB_TimeToStart)
            {
              ChangeState(LaserbeamBattleState.LBBS_BATTLE);
              this.Transform =
                rotationToPlayer *
                Matrix.CreateTranslation(newPosition);
              break;
            }

            float amount = this.timeElapsed / this.game.Options.LBB_TimeToStart;

            this.Transform =
              Matrix.Lerp(Matrix.Identity, rotationToPlayer, amount) *
              Matrix.Lerp(
                Matrix.CreateTranslation(Vector3.Zero),
                Matrix.CreateTranslation(newPosition),
                amount);
          }
          break;
        case LaserbeamBattleState.LBBS_STOPPING:
          {
            if (this.timeElapsed >= this.game.Options.LBB_TimeToStart)
            {
              ChangeState(LaserbeamBattleState.LBBS_ROTATECAMERABACKWARD);
              this.Transform =
                Matrix.Identity;
              break;
            }

            float amount = this.timeElapsed / this.game.Options.LBB_TimeToStart;

            this.Transform =
              Matrix.Lerp(rotationToPlayer, Matrix.Identity, amount) *
              Matrix.Lerp(
              Matrix.CreateTranslation(newPosition),
              Matrix.CreateTranslation(Vector3.Zero),
              amount);
          }
          break;
      }
    }

    public override void Disable()
    {
      base.Disable();
      this.handler.IsLocked = false;
      if (spriteRightTrigger != null)
      {
        spriteLeftTrigger.IsVisible = false;
        spriteRightTrigger.IsVisible = false;
      }
    }

    public override void Enable()
    {
      this.IsEnabled = true;
      ChangeState(LaserbeamBattleState.LBBS_ROTATECAMERAFORWARD);
    }
  }
}