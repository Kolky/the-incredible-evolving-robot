using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Helpers;
using Tier.Source.Misc;
using Tier.Source.Objects;

namespace Tier.Source.Handlers
{
  public enum InterfaceComponent
  {
    IC_CROSSHAIR, IC_FPS_COUNTER, IC_HEALTHBAR, IC_SCORE, IC_WEAPON_STATUS
  };
  
  public class InterfaceHandler
  {
    private TierGame game;
    private Crosshair crosshair;
    private Healthbar healthbar;
    private FPSCounter fpsCounter;
    private bool isInitialized;
    private PlayerWeaponStatusHelper weaponStatusHelper;

    #region Properties
    private bool isHidden;

    public bool IsHidden
    {
      get { return this.isHidden; }
    }
    #endregion

    public InterfaceHandler(TierGame game)
    {
      this.game = game;
      this.isInitialized = false;
      // Healthbar 
      this.healthbar = new Healthbar(this.game.GameHandler.Player, game);
      this.crosshair = new Crosshair(game);
      this.crosshair.Initialize();
      fpsCounter = new FPSCounter(this.game);
      this.isInitialized = true;

      if (this.weaponStatusHelper == null)
      {
        this.weaponStatusHelper = new PlayerWeaponStatusHelper(
          this.game.GameHandler.Player,
          this.game.TextSpriteHandler,
          this.game.ContentHandler);
      }
      this.weaponStatusHelper.Initialize();
            
      this.crosshair.Hide();
      this.healthbar.Hide();
      this.fpsCounter.Hide();
    }

    public void Show()
    {
      this.isHidden = false;
      Show(InterfaceComponent.IC_WEAPON_STATUS);
      Show(InterfaceComponent.IC_CROSSHAIR);
      Show(InterfaceComponent.IC_HEALTHBAR);
      Show(InterfaceComponent.IC_SCORE);
    }

    public void Show(InterfaceComponent component)
    {
      switch (component)
      {
        case InterfaceComponent.IC_CROSSHAIR:
          this.crosshair.Show();
          break;
        case InterfaceComponent.IC_FPS_COUNTER:
          this.fpsCounter.Show();
          break;
        case InterfaceComponent.IC_HEALTHBAR:
          this.healthbar.Show();
          break;
        case InterfaceComponent.IC_SCORE:
          this.game.ScoreHandler.Show();
          break;
        case InterfaceComponent.IC_WEAPON_STATUS:
          this.weaponStatusHelper.Show();
          break;
      }
    }

    public void Hide()
    {
      this.isHidden = true;

      this.crosshair.Hide();
      this.healthbar.Hide();
      this.game.ScoreHandler.Hide();
      this.weaponStatusHelper.Hide();
    }

    public Vector2 GetCrosshairPosition()
    {
      return this.crosshair.Position;
    }

    public void Update(GameTime gameTime)
    {
      if (!isInitialized)
        return;
      
      this.healthbar.Update(gameTime);
      this.crosshair.Update(gameTime);
      this.weaponStatusHelper.Update();
    }

    public void UpdateFpsCounter(GameTime gameTime)
    {
      if (!isInitialized)
        return;

      this.fpsCounter.Update(gameTime);
    }
  }
}