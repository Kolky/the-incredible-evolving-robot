using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Tier.Camera;
using Tier.Menus;
using Tier.Misc;
using Tier.Objects;
using Tier.Objects.Attachable;
using Tier.Objects.Attachable.Weapons;
using Tier.Objects.Basic;
using Tier.Objects.Destroyable;
using Tier.Objects.Destroyable.Projectile;
#if XBOX360
using InstancedModelSample;
#else
using Instanced;
#endif

using System.Collections;

namespace Tier.Handlers
{
    public class GameHandler
    {
        #region Properties

        private int currentLevel;

        public int CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }

        private Enemy boss;
        public Enemy Boss
        {
            get { return boss; }
            set { boss = value; }
        }

        private static MenuState menuState;
        public static MenuState MenuState
        {
            get { return menuState; }
            set
            {
                if (menuState != null)
                    menuState.Dispose();
                menuState = value;
                menuState.Initialize();
            }
        }

        private static BossGrowthHandler growthHandler;
        public static BossGrowthHandler BossGrowthHandler
        {
            get { return growthHandler; }
            set { growthHandler = value; }
        }

        private static BossPieceHandler bossPieceHandler;
        public static BossPieceHandler BossPieceHandler
        {
            get { return bossPieceHandler; }
        }

        private static ObjectHandler objectHandler;
        public static ObjectHandler ObjectHandler
        {
            get { return objectHandler; }
        }

        private static Tier.Camera.Camera camera;
        public static Tier.Camera.Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        private static HUD hud;
        public static HUD HUD
        {
            get { return hud; }
            set { hud = value; }
        }

        private static Game game;
        public static Game Game
        {
            get { return game; }
        }

        private static Player player;
        public static Player Player
        {
            get { return player; }
        }
        #endregion

        public GameHandler(Game game)
        {
            GameHandler.game = game;
            GameHandler.MenuState = new StartMenu();
            GameHandler.objectHandler = new ObjectHandler(game);
            GameHandler.HUD = new HUD();
            GameHandler.bossPieceHandler = new BossPieceHandler(game);
            GameHandler.BossGrowthHandler = new BossGrowthHandler();
        }

        private void InitializeComponents()
        {
            // Boss pieces
            GameHandler.BossPieceHandler.AddPiece("Bar", TierGame.ContentHandler.GetModel("Bar"));
            GameHandler.BossPieceHandler.AddPiece("L", TierGame.ContentHandler.GetModel("L"));
            GameHandler.BossPieceHandler.AddPiece("T", TierGame.ContentHandler.GetModel("T"));
            GameHandler.BossPieceHandler.AddPiece("Boss", TierGame.ContentHandler.GetModel("Boss"));
            GameHandler.BossPieceHandler.AddPiece("Sphere", TierGame.ContentHandler.GetModel("Sphere"));
            GameHandler.BossPieceHandler.AddPiece("Cube", TierGame.ContentHandler.GetModel("Cube"));
            GameHandler.BossPieceHandler.AddPiece("Cross", TierGame.ContentHandler.GetModel("Cross"));
            GameHandler.BossPieceHandler.AddPiece("Nico", TierGame.ContentHandler.GetModel("Nico"));

            Sphere sphere = new Sphere(GameHandler.Game, 34.0f);
            GameHandler.player = new Player(GameHandler.Game, sphere);
            GameHandler.Camera = new DelayCamera(new Vector3(0f, 0f, 23f), Vector3.Zero, GameHandler.Player, new Vector3(0f, 0.1f, 0f));

            GameHandler.ObjectHandler.AddObject(sphere);
            GameHandler.ObjectHandler.AddObject(GameHandler.Player);

            this.NewGame();
        }

        public void NewGame()
        {
            this.Boss = GameHandler.BossPieceHandler.GetEnemy("Cube");
            this.Boss.GrowthPattern = GameHandler.BossGrowthHandler.Load("Default");
            this.Boss.Initialize();
            this.Boss.Spawn();
            this.currentLevel = 1;

            GameHandler.ObjectHandler.AddObject(this.Boss);
        }

        public void LoadGraphicsContent(Boolean loadAllContent)
        {
            if (loadAllContent)
            {
                // Blocks
                TierGame.ContentHandler.setModel("Bar", TierGame.Instance.Content.Load<Model>("Content//Models//Bar"));
                TierGame.ContentHandler.setModel("L", TierGame.Instance.Content.Load<Model>("Content//Models//L"));
                TierGame.ContentHandler.setModel("Globe", TierGame.Instance.Content.Load<Model>("Content//Models//Globe"));
                TierGame.ContentHandler.setModel("Boss", TierGame.Instance.Content.Load<Model>("Content//Models//TierCore"));
                TierGame.ContentHandler.setModel("Cross", TierGame.Instance.Content.Load<Model>("Content//Models//Cross"));
                TierGame.ContentHandler.setModel("Nico", TierGame.Instance.Content.Load<Model>("Content//Models//Nico"));
                TierGame.ContentHandler.setModel("T", TierGame.Instance.Content.Load<Model>("Content//Models//T"));
                TierGame.ContentHandler.setModel("Cube", TierGame.Instance.Content.Load<Model>("Content//Models//Cube"));
                // Sphere
                TierGame.ContentHandler.setModel("Sphere", TierGame.Instance.Content.Load<Model>("Content//Models//SphereConnector"));
                // Player
                TierGame.ContentHandler.setModel("Ship", TierGame.Instance.Content.Load<Model>("Content//Models//Ship"));
                // Weapons
                TierGame.ContentHandler.setModel("Laser", TierGame.Instance.Content.Load<Model>("Content//Models//Laser"));
                TierGame.ContentHandler.setModel("LaserGun", TierGame.Instance.Content.Load<Model>("Content//Models//LaserGun"));
                //TierGame.ContentHandler.setModel("DoubleBullet", TierGame.Instance.Content.Load<Model>("Content//Models//DoubleBullet"));
                TierGame.ContentHandler.setModel("DoubleBulletTurret", TierGame.Instance.Content.Load<Model>("Content//Models//DoubleBulletTurret"));
                TierGame.ContentHandler.setModel("RocketTurret", TierGame.Instance.Content.Load<Model>("Content//Models//RocketTurret"));
                TierGame.ContentHandler.setModel("Laserbeam", TierGame.Instance.Content.Load<Model>("Content//Models//Laserbeam"));
                TierGame.ContentHandler.setModel("LaserbeamTurret", TierGame.Instance.Content.Load<Model>("Content//Models//LaserbeamTurret"));


                TierGame.ContentHandler.setAnimatedTexture("Explosion", "Content//Textures//Explosion//BigExplosion");

                //instanced models:
                TierGame.ContentHandler.setModel("DoubleBullet", TierGame.Instance.Content.Load<Model>("Content//Models//DoubleBullet"));
                TierGame.ContentHandler.setModel("Rocket", TierGame.Instance.Content.Load<Model>("Content//Models//Rocket"));

                BasicEffect effect = new BasicEffect(TierGame.Device, null);
                effect.TextureEnabled = true;
                effect.CommitChanges();

                TierGame.ContentHandler.setEffect("AnimatedBillboard", effect);
                TierGame.ContentHandler.setEffect("BlockEffect", TierGame.Instance.Content.Load<Effect>("Content//Effects//Blocks"));

                this.InitializeComponents();
            }
        }

        public void Update(GameTime gameTime)
        {
            Effect ef = TierGame.ContentHandler.getEffect("BlockEffect");
            ef.Parameters["lightDir"].SetValue(GameHandler.Camera.Direction);
            GameHandler.Camera.Update(gameTime);

            GameHandler.MenuState.Update(gameTime);

            if (GameHandler.MenuState.GetType() == typeof(GameMenu))
                GameHandler.ObjectHandler.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            GameHandler.MenuState.Draw(gameTime);

            if (GameHandler.MenuState.GetType() == typeof(GameMenu))
                GameHandler.ObjectHandler.Draw(gameTime);
        }

        public void NextLevel()
        {
            this.currentLevel++;
            this.Boss.GrowBoss();
        }
    }
}
