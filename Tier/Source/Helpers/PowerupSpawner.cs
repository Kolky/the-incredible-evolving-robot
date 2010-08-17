using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.Objects.Powerups;
using Tier.Source.GameStates;
using Tier.Source.GameStates.Tutorial;

namespace Tier.Source.Helpers
{
  public class PowerupSpawner
  {
    private enum PowerupTypes
    {
      HEALTH, WEAPON, WEAPONUPGRADE 
    };

    public static void Spawn(Vector3 position, TierGame game)
    {
      if (  game.GameState.GetType() != typeof(MainGameState) &&
            game.GameState.GetType() != typeof(TutorialPowerupsState))
      {
        return;
      }

      int number = game.Random.Next(1, 100);

      if (number >= 33)
      {
        SpawnHealth(game, position);
      }
      if(number >= 60)
      {
        SpawnWeaponUpgrade(game, position);
      }
      if(number >= 95)
      {
        SpawnRare(game, position);
      }
    }

    private static void SpawnHealth(TierGame game, Vector3 position)
    {
      Powerup p = new PowerupHealth(game);
      if (game.ObjectHandler.InitializeFromBlueprint<Powerup>(p, "PowerupHealth"))
      {
        p.Spawn(position);
        game.GameHandler.AddObject(p);
      }
    }

    private static void SpawnWeaponUpgrade(TierGame game, Vector3 position)
    {
      Powerup p = new PowerupWeaponUpgrade(game);
      if (game.ObjectHandler.InitializeFromBlueprint<Powerup>(p, "PowerupWeaponUpgrade"))
      {
        p.Spawn(position);
        game.GameHandler.AddObject(p);
      }
    }

    private static void SpawnRare(TierGame game, Vector3 position)
    {
      string powerupName = "";
      Powerup p = null;

      switch (game.Random.Next(2))
      {
        case 0:
          p = new PowerupLaser(game);
          powerupName = "PowerupLaser";
          break;
        case 1:
          p = new PowerupQuad(game);
          powerupName = "PowerupQuad";
          break;
        case 2:
          p = new PowerupRocket(game);
          powerupName = "PowerupRocket";
          break;
      }

      if (game.ObjectHandler.InitializeFromBlueprint<Powerup>(p, powerupName))
      {
        p.Spawn(position);
        game.GameHandler.AddObject(p);
      }
    }
  }
}
