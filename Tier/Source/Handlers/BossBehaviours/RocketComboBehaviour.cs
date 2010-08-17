using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Tier.Source.Helpers;
using Tier.Source.Objects.Projectiles;
using Tier.Source.Helpers.Cameras;

namespace Tier.Source.Handlers.BossBehaviours
{
  /*
   * TODO:
   * - Speler mag niet bewegen en schieten tijdens deze behaviour
   * - Polish van dynamiek van deze behaviour
   * - Aantal knoppen moet weer dynamisch
   * - Opschoning van code (staat nog oude meuk bij van vorige versie)
   * - Verplaatsen van private klasse en enum in losse files
   * - Baas moet naar speler toe draaien
   */
  public class RocketComboBehaviour : BossBehaviour
  {
    #region Privates
    private enum RocketComboBehaviourState
    {
      MOVING_FROM_PLAYER, FOLLOWING_PROJECTILE
    };

    private class RocketComboButton
    {
      public RocketComboButtonType type;
      public RocketComboButtonState state;
      public Sprite sprite;
      public RocketComboRocket rocket;

      public RocketComboButton(RocketComboButtonType type)
      {
        this.type = type;
        this.state = RocketComboButtonState.Idle;
        this.sprite = null;
        this.rocket = null;
      }
    }

    private enum RocketComboButtonState
    {
      Idle, Missed, Hit
    };

    private enum RocketComboButtonType
    {
      A, B, X, Y
    };

    private List<RocketComboButton> buttons;
    private int currentButton;
    private GamePadState previousGamepadState;
    private float timeElapsed;
    private RocketComboBehaviourState state; 
    private int numberOfButtons;
    #endregion

    private PositionalCamera camera;
    private Camera previousCamera;
    private Vector3 newPosition;
    private Projectile projectile;

    public RocketComboBehaviour(TierGame game, BossBehaviourHandler handler)
      : base(game, handler, BossBehaviourType.BBT_ROCKETCOMBO)
    {
      Transform = Matrix.Identity;
      buttons = new List<RocketComboButton>();
      previousGamepadState = new GamePadState();
      newPosition = Vector3.Zero;
      numberOfButtons = 3;
      camera = new PositionalCamera(game);
    }

    private bool buttonsReleased(GamePadState gamepadState)
    {
      return
        (gamepadState.Buttons.A == ButtonState.Released &&
        gamepadState.Buttons.B == ButtonState.Released &&
        gamepadState.Buttons.X == ButtonState.Released &&
        gamepadState.Buttons.Y == ButtonState.Released);
    }

    private bool buttonPressed(GamePadState gamepadState)
    {
      return
        (gamepadState.Buttons.A == ButtonState.Pressed ||
        gamepadState.Buttons.B == ButtonState.Pressed ||
        gamepadState.Buttons.X == ButtonState.Pressed ||
        gamepadState.Buttons.Y == ButtonState.Pressed);
    }
    
    private void changeState(RocketComboBehaviourState newstate)
    {
      this.timeElapsed = 0;

      switch (newstate)
      {
        case RocketComboBehaviourState.FOLLOWING_PROJECTILE:
          generateButtons();

          // Spawn the projectile
          projectile.Position = newPosition;
          Vector3 diff = this.game.GameHandler.Player.Position - newPosition;
          diff.Normalize();
          projectile.MovableModifier.Velocity = diff;
          projectile.Speed = 1;
          projectile.Scale = new Vector3(4);
          this.game.GameHandler.AddObject(projectile);
          projectile.MovableModifier.Velocity *= 25;
          break;
        case RocketComboBehaviourState.MOVING_FROM_PLAYER:
          // Change camera and save current one
          previousCamera = this.game.GameHandler.Camera;
          this.game.GameHandler.Camera = this.camera;
          // Determine new boss position and camera position
          newPosition = -this.game.GameHandler.Player.Position;
          //newPosition.Normalize();
          //newPosition *= 100;

          this.camera.Target = Vector3.Zero;
          this.camera.Position = Vector3.Transform(newPosition, Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.PiOver2)) * 2;
          this.camera.UpVector = Vector3.Up;
          break;
      }

      this.state = newstate;
    }

    private bool checkButton(GamePadState gamepadState)
    {
      if (buttons.Count > 0 && currentButton < buttons.Count)
      {
        bool value = false;

        switch (buttons[currentButton].type)
        {
          case RocketComboButtonType.A:
            value = gamepadState.Buttons.A == ButtonState.Pressed &&
                    previousGamepadState.Buttons.A == ButtonState.Released;
            break;
          case RocketComboButtonType.B:
            value = gamepadState.Buttons.B == ButtonState.Pressed &&
                    previousGamepadState.Buttons.B == ButtonState.Released;
            break;
          case RocketComboButtonType.X:
            value = gamepadState.Buttons.X == ButtonState.Pressed &&
                    previousGamepadState.Buttons.X == ButtonState.Released;
            break;
          case RocketComboButtonType.Y:
            value = gamepadState.Buttons.Y == ButtonState.Pressed &&
                    previousGamepadState.Buttons.Y == ButtonState.Released;
            break;
        }

        return value;
      }

      return false;
    }

    private void checkForHits()
    {
      foreach (RocketComboButton button in this.buttons)
      {
        if (button.state == RocketComboButtonState.Missed)
        {
          this.game.GameHandler.AddDamagedObject(
            this.game.GameHandler.Player,
            this.game.Options.RocketCombo_Damage);
        }

        this.game.TextSpriteHandler.RemoveSprite(button.sprite);
      }

      buttons.Clear();
    }

    public override void Disable()
    {
      base.Disable();
      this.Transform = Matrix.Identity;

      if (previousCamera != null)
      {
        this.game.GameHandler.Camera = previousCamera;
      }
      this.handler.IsLocked = false;

      foreach (RocketComboButton button in buttons)
      {
        this.game.TextSpriteHandler.RemoveSprite(button.sprite);

        if (button.rocket != null)
        {
          this.game.GameHandler.RemoveObject(button.rocket, button.rocket.Type);
          button.rocket.Dispose();
        }
      }
      buttons.Clear();
    }
    
    public override void Enable()
    {
      if (projectile == null)
      {
        projectile = new PlasmaProjectile(game);        
        this.game.ObjectHandler.InitializeFromBlueprint<Projectile>(projectile, "Plasma");
        projectile.TemporaryModifier.TTL = int.MaxValue;
      }

      this.handler.IsLocked = true;
      this.handler.Reset();
      this.handler.DisableBehaviour(BossBehaviourType.BBT_ROTATING);
      this.handler.ResetBehaviour(BossBehaviourType.BBT_ROTATING);
      this.handler.DisableBehaviour(BossBehaviourType.BBT_ROAMING);
      this.IsEnabled = true;
      this.timeElapsed = 0;
      this.currentButton = 0;

      changeState(RocketComboBehaviourState.MOVING_FROM_PLAYER);
    }

    public override void Update(GameTime gameTime)
    {
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      switch (state)
      {
        case RocketComboBehaviourState.FOLLOWING_PROJECTILE:
          if (this.timeElapsed > this.game.Options.RocketCombo_FollowingProjectileTime)
          {
            checkForHits();
            this.Transform = Matrix.Identity;
            Disable();
            return;
          }

          Vector3 projectilePointOfView = Vector3.Transform(
            new Vector3(0, 3.5f, -15),
            this.game.GameHandler.Player.Rotation);

          // Check for player input
          bool isButtonPressed =
            buttonPressed(GamePad.GetState(this.game.MainControllerIndex)) &&
            buttonsReleased(previousGamepadState);

          if (isButtonPressed && checkButton(GamePad.GetState(this.game.MainControllerIndex)))
          {
            buttons[currentButton].state = RocketComboButtonState.Hit;
            buttons[currentButton].sprite.Color = new Color(new Vector4(0, 255, 0, 255));            
            NextButton();
          }
          else if (isButtonPressed)
          {
            buttons[currentButton].state = RocketComboButtonState.Missed;
            buttons[currentButton].sprite.Color = new Color(new Vector4(255, 0, 0, 255));
            NextButton();
          }
          
          this.previousGamepadState = GamePad.GetState(this.game.MainControllerIndex);
          this.Transform = Matrix.CreateTranslation(this.newPosition);
          break;
        case RocketComboBehaviourState.MOVING_FROM_PLAYER:
          if (this.timeElapsed > this.game.Options.RocketCombo_MoveFromPlayerTime)
          {
            changeState(RocketComboBehaviourState.FOLLOWING_PROJECTILE);
          }

          this.Transform = Matrix.Lerp(
            Matrix.CreateTranslation(Vector3.Zero),
            Matrix.CreateTranslation(this.newPosition),
            this.timeElapsed / this.game.Options.RocketCombo_MoveFromPlayerTime);
          break;
      }
    }

    private void NextButton()
    {
      this.currentButton++;

      if (this.currentButton >= buttons.Count)
      {
        this.currentButton = buttons.Count - 1;
      }
    }

    private void generateButtons()
    {
      if (this.game.BossGrowthHandler.Core.DestroyableModifier.IsDestroyed)
        return;

      // Create the combo sequence
      Random r = new Random(DateTime.Now.Millisecond);

      int x = 10;
      int y = this.game.GraphicsDevice.PresentationParameters.BackBufferHeight - 50;

      for (int i = 0; i < numberOfButtons; i++)
      {
        RocketComboButton button = null;

        switch (r.Next(4))
        {
          case 0:
            button = new RocketComboButton(RocketComboButtonType.A);
            button.sprite = this.game.TextSpriteHandler.CreateSprite(
              this.game.ContentHandler.GetAsset<Texture2D>("button_a"),
              new Vector2(x, y));
            break;
          case 1:
            button = new RocketComboButton(RocketComboButtonType.B);
            button.sprite = this.game.TextSpriteHandler.CreateSprite(
              this.game.ContentHandler.GetAsset<Texture2D>("button_b"),
              new Vector2(x, y));
            break;
          case 2:
            button = new RocketComboButton(RocketComboButtonType.Y);
            button.sprite = this.game.TextSpriteHandler.CreateSprite(
              this.game.ContentHandler.GetAsset<Texture2D>("button_y"),
              new Vector2(x, y));
            break;
          case 3:
            button = new RocketComboButton(RocketComboButtonType.X);
            button.sprite = this.game.TextSpriteHandler.CreateSprite(
              this.game.ContentHandler.GetAsset<Texture2D>("button_x"),
              new Vector2(x, y));
            break;
        }

        button.sprite.Depth = 1;
        buttons.Add(button);
        x += 50;
      }
    }
  }
}