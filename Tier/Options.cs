using System;
using Microsoft.Xna.Framework;

namespace Tier
{
  /// <summary>
  /// Summary description for Class1
  /// </summary>
  public class Options
  {   
    public int BaseHealth = 2000;
    public int HealthPerLevel = 500;
    public float ComboTimeOnScreen = 500.0f;
    public bool IsDrawBoundingVolumes = false;
    public bool IsSoundEnabled = false;
    public float BossGrowthSpeed = 1.2f;
    public float PlayerHealthLossPerSecond = 0.9f;
    public float PlayerHealthGainPerSecond = 20f;

    /*=================================
      | BEHAVIOUR SETTINGS            |
     *================================= 
     */ 
    // Charge behaviour settings
    public float Charge_TimeToRotate = 750.0f;
    public float Charge_JustInTime = 600.0f;  // Dodged just in time extra points
    public int Charge_TimeToCharge = 750;
    public int Charge_TimeToReturn = 750;
    public int Charge_TimeTotal = 2500;
    public float Charge_TimeDirectionRotation = 100.0f;
    public int Charge_DodgePoints = 500;
    public int Charge_PerfectDodgePoints = 1000;
    public int Charge_BaseNumberOfCharges = 1;
    public float Charge_ChargesPerLevel = 0.1f;
    public float BossChargeDamage = 10.0f;
    // Rotating behaviour settings
    public int Rotating_Duration = 1000;
    // Energy shield behaviour settings
    public int EnergyShield_Duration = 10000;
    // Rocket combo behaviour settings        
    public int RocketCombo_MoveFromPlayerTime = 1000;
    public int RocketCombo_FollowingProjectileTime = 5000;
    public int RocketCombo_StartButtons = 3;
    public float RocketCombo_ButtonsPerLevel = 0.2f;
    public float RocketCombo_Damage = 5f;
    // Frenzy behaviour settings
    public float Frenzy_TimeToTurnRed = 1000.0f;
    public float Frenzy_TimeToShoot = 4000.0f;
    public float Frenzy_FireMultiplier = 0.25f;
    public float Frenzy_FireStartMultiplier = 2.0f;
    // Roam behaviour settings
    public float RoamSpeed = 0.025f;
    public float RoamDistance = 5.0f;
    // Laserbeam battle behaviour settings
    public int LBB_TimeToRotateCam = 500;
    public int LBB_TimeToStart = 500;
    public float LBB_PlayerDamage = 10.0f;
    public float LLB_BossBaseProgressPerSecond = 2.0f;
    public float LLB_BossProgressPerSecond = 0.5f; // Per level
    public float LLB_PlayerProgressPerSecond = 8.0f;

    /*=================================
      | PLAYER BEHAVIOUR SETTINGS        |
     *================================= 
     */
    public int QuadDamage_TTL = 5000;

    /*=================================
      | PLAYER WEAPON SETTINGS        |
     *================================= 
     */
    // LASERBEAM
    public int    PlayerWeapon_Laserbeam_BaseDamage = 5;
    public int    PlayerWeapon_Laserbeam_DamageUpgrade = 1;    
    // PLASMA
    public int    PlayerWeapon_Plasma_BaseDamage = 20;
    public int    PlayerWeapon_Plasma_DamageUpgrade = 2;
    public int    PlayerWeapon_Plasma_Cooldown_Level1 = 100;
    public int    PlayerWeapon_Plasma_Cooldown_Level2 = 75;
    public int    PlayerWeapon_Plasma_Cooldown_Level3 = 50;
    public int    PlayerWeapon_Plasma_Cooldown_Level4 = 25;    
    public float  PlayerWeapon_Plasma_Spread_Level1 = 0.05f;
    public float  PlayerWeapon_Plasma_Spread_Level2 = 0.075f;
    public float  PlayerWeapon_Plasma_Spread_Level3 = 0.1f;
    public float  PlayerWeapon_Plasma_Spread_Level4 = 0.2f;
    // ROCKETS
    public int    PlayerWeapon_Rocket_BaseDamage          = 100;
    public int    PlayerWeapon_Rocket_DamageUpgrade       = 25;
    public int    PlayerWeapon_Rocket_AoE_Distance        = 10;
    public int    PlayerWeapon_Rocket_AoE_BaseDamage      = 50;
    public int    PlayerWeapon_Rocket_AoE_DamageUpgrade   = 10;
    public int    PlayerWeapon_Rocket_AoE_DistanceUpgrade = 2;
    public int    PlayerWeapon_Rocket_Cooldown            = 350;

    // Delay Camera Options
    public int      DelayCamera_DelayTimerLimit = 4;
    public float    DelayCamera_RetainPercentage = 0.8f;
    public float    DelayCamera_AddPercentage = 0.2f;
    public Vector3  DelayCamera_DefaultOffset = new Vector3(0, 2.5f, 12.5f);
    public Vector3  DelayCamera_NearPowerupOffset = new Vector3(0, 5, 30.0f);

    // Score settings
    public int      ScoreSingleBlockDestroyed = 1000;
    public int      ScoreComboBlockDestroyed = 1250;
    public Vector2  ScorePosition = new Vector2(10, 60);

    // Powerup settings
    public float Powerup_SpawnSpeed = 2000.0f;
    // Health powerup
    public int PowerupHealth_Lifegain = 10;
  }
}