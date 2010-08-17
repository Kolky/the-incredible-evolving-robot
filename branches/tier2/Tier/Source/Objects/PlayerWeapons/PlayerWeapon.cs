using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Tier.Source.Objects.PlayerWeapons
{
  public abstract class PlayerWeapon
  {
    #region Protected members
    protected int cooldown;
    protected int timeElapsed;
    protected TierGame game;
    protected int timesUpgraded;
    protected enum PlayerWeaponLevel
    {
      PWL_ONE, PWL_TWO, PWL_THREE, PWL_FOUR
    };
    protected PlayerWeaponLevel weaponLevel;
    #endregion

    #region Properties
    public int TimesUpgraded
    {
      get { return this.timesUpgraded; }
    }
    #endregion

    public PlayerWeapon(TierGame game, int cooldown)
    {
      this.cooldown = cooldown;
      this.game = game;
      this.timesUpgraded = 0;
      this.weaponLevel = PlayerWeaponLevel.PWL_ONE;
    }

    public abstract void AttachWeapon();

    protected void ChangeLevel(PlayerWeaponLevel newlevel)
    {
      if (this.weaponLevel == newlevel)
        return;

      switch (newlevel)
      {
        case PlayerWeaponLevel.PWL_TWO:
          this.game.SoundHandler.Play("VoiceLevel2");
          break;
        case PlayerWeaponLevel.PWL_THREE:
          this.game.SoundHandler.Play("VoiceLevel3");
          break;
        case PlayerWeaponLevel.PWL_FOUR:
          this.game.SoundHandler.Play("VoiceLlevel4");
          break;
      }

      this.weaponLevel = newlevel;
    }

    protected abstract void DoShoot(Vector3 direction);
    
    public abstract void RemoveWeapon();

    public virtual void Reset()
    {
      this.weaponLevel = PlayerWeaponLevel.PWL_ONE;
      this.timesUpgraded = 0;
    }

    public void Shoot(Vector3 direction)
    {
      if (timeElapsed >= cooldown)
      {
        timeElapsed = 0;
        DoShoot(direction);
      }
    }
    
    public virtual void Update(GameTime gameTime)
    {
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
    }

    public virtual void Upgrade()
    {
      if (timesUpgraded < 15)
      {
        timesUpgraded++;
      }

      if (timesUpgraded >= 15)        
      {
        ChangeLevel(PlayerWeaponLevel.PWL_FOUR);
      }
      else if (timesUpgraded >= 10)
      {
        ChangeLevel(PlayerWeaponLevel.PWL_THREE);
      }
      else if (timesUpgraded >= 5)
      {
        ChangeLevel(PlayerWeaponLevel.PWL_TWO);
      }
    }
  }
}
