using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Misc
{
  public enum SortFilter
  {
    BloomInstanced, Bloom, Sphere, Other, OtherInstanced, AlphaBlendedBillboard
  };

  public class Options
  {
    #region Basic & Game
    public static Random Random = new Random();
    public static int IdCounter = 1;

    public class Game
    {
      public const int StartTime = 60;
    }
    #endregion

    #region Weapons
    public class HomingRocket
    {
      public const float MaxDistance = 10000f;
      public const float MaxAngle = MathHelper.PiOver2;
      public const int UpdateCheck = 5;
    }

    public class DoubleBullet
    {
      public const float MaxAngle = MathHelper.PiOver2;
      public const int UpdateCheck = 5;
    }
    #endregion

    /// <summary>Audio Options</summary>
		public class Audio
		{
			public const String AudioEngineFile = "Content\\Audio\\Audio.xgs";
			public const String WaveBankFile = "Content\\Audio\\Wave Bank.xwb";
			public const String SoundBankFile = "Content\\Audio\\Sound Bank.xsb";
		}

    /// <summary>HighScores Options</summary>
    public class HighScore
    {
      public const int MaxEntries = 10;
    }

		/// <summary>Game settings such as music on</summary>
		public class Settings
		{
			public static float BGMVolume = 1f;
			public static float SFXVolume = 1f;
		}

		/// <summary>Camera Options</summary>
    public class Camera
    {
      public const float FieldOfView = MathHelper.PiOver4;
      public const float AspectRatio = 4f / 3f;
      public const float NearPlaneDistance = 0.1f;
      public const float FarPlaneDistance = 100000.0f;

			// Delay Camera Options
			public const float DelayRotationLength = 0.95f; /// smaller than 1
			public const float DelayCoordinateLength = 0.9f; /// smaller than 1
      public const double DelayTimerLimit = 15;
    }
    /// <summary>Controls Options</summary>
    public class Controls
    {
#if XBOX360
      public const float TriggerDepth = 0.3f;
#endif
    }

		/// <summary>Player Options</summary>
		public class Player
		{
			public const float Speed = 25f; //Bigger than 0f
			public static int DiePenalty = 10;
		}

		/// <summary>SphereGrid Color</summary>
    public class Colors
    {
      public static Color ClearColor = Color.Black;

      public static Color GridColor = Color.DarkBlue;
      public static Color HeadColor = Color.Black;
      public static Color MenuColor = Color.White;
      public static Color ActiveColor = Color.Gray;

      public class Boss
      {
        public static Vector3 CoreColor = Color.Red.ToVector3();
        public static Vector3 BlockColor = Color.Lime.ToVector3();
        public static Vector3 LineColor = Color.Yellow.ToVector3();
      }

      public class ControlsMenu
      {
        public static Color ClearColor = Color.Orange;
      }
      public class GameMenu
      {
        public static Color ClearColor = Color.Black;
      }
      public class GameOverMenu
      {
        public static Color ClearColor = Color.Red;
      }
      public class HighScoreMenu
      {
        public static Color ClearColor = Color.Cyan;
        public static Color ScoreColor = Color.Red;
        public static Color HeaderColor = Color.Gold;
      }
      public class NextLevelMenu
      {
        public static Color ClearColor = Color.Green;
      }
      public class OptionsMenu
      {
        public static Color ClearColor = Color.Blue;
      }
      public class PauseMenu
      {
        public static Color ClearColor = Color.Purple;
      }
      public class StartMenu
      {
        public static Color ClearColor = Color.White;
      }

#if DEBUG
      /// <summary>Axis Colors</summary>
      public class Axis
      {
        public static Color XColor = Color.Red;
        public static Color YColor = Color.Green;
        public static Color ZColor = Color.Blue;
      }
#endif
    }
  }
}
