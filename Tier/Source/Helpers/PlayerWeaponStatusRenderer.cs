using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.Objects;
using Tier.Source.Handlers;
using pjEngine.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tier.Source.Helpers
{
  public class PlayerWeaponStatusHelper
  {
    private TextSpriteHandler textSpriteHandler;
    private ContentHandler contentHandler;
    private Player player;
    private Sprite[] weaponLevels;
    private Sprite[] numberOfUpgrades;
    private Sprite background;
    private TierGame game;
    private bool isVisible;

    public PlayerWeaponStatusHelper(Player player, 
      TextSpriteHandler textSpritehandler,
      ContentHandler contentHandler)
    {
      this.player = player;
      this.textSpriteHandler = textSpritehandler;
      this.contentHandler = contentHandler;
      this.game = player.Game;

      this.numberOfUpgrades = new Sprite[15];
      this.weaponLevels = new Sprite[4];
    }

    private void CreateSprites()
    {
      // Create sprites
      background = this.textSpriteHandler.CreateSprite(
        contentHandler.GetAsset<Texture2D>("WeaponUpgradeFrame"),
        new Vector2(140, 585));
      background.Origin = new Vector2(128, 128);

      // Create upgrade icons
      for (int i = 0; i < 15; i++)
      {
        numberOfUpgrades[i] = this.textSpriteHandler.CreateSprite(
          this.contentHandler.GetAsset<Texture2D>("WeaponUpgradeSlot"),
          new Vector2(140, 585));

        numberOfUpgrades[i].Origin = new Vector2(128, 128);
        numberOfUpgrades[i].Rotation = (float)(Math.PI * 2.0f) * i / 15.0f;
      }

      // Create levels
      for (int i = 0; i < 4; i++)
      {
        weaponLevels[i] = this.textSpriteHandler.CreateSprite(
          this.contentHandler.GetAsset<Texture2D>(string.Format("WeaponUpgradeLevel{0}", i+1)),
          new Vector2(140, 585));
        weaponLevels[i].Origin = new Vector2(128, 128);
      }
    }

    public void Hide()
    {
      this.isVisible = false;
    }

    public void Show()
    {
      this.isVisible = true;
    }

    public void Initialize()
    {
      if (background == null)
      {
        CreateSprites();
      }

      Reset();
    }

    public void Reset()
    {
      for (int i = 0; i < 15; i++)
      {
        numberOfUpgrades[i].IsVisible = false;
      }
    }

    public void Update()
    {
      if(!isVisible)
      {
        for (int i = 0; i < numberOfUpgrades.Length; i++)
        {
          numberOfUpgrades[i].IsVisible = false;
        }

        background.IsVisible = false;
        weaponLevels[0].IsVisible = false;
        weaponLevels[1].IsVisible = false;
        weaponLevels[2].IsVisible = false;
        weaponLevels[3].IsVisible = false;

        return;
      }

      for (int i = 0; i < this.player.PlayerWeapon.TimesUpgraded; i++)
      {
        numberOfUpgrades[i].IsVisible = true;
      }

      weaponLevels[0].IsVisible = false;
      weaponLevels[1].IsVisible = false;
      weaponLevels[2].IsVisible = false;
      weaponLevels[3].IsVisible = false;
      background.IsVisible      = true;

      int level = this.player.PlayerWeapon.TimesUpgraded / 5;
      weaponLevels[level].IsVisible = true;
    }
  }
}
