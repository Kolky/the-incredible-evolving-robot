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
        public int CurrentLevel { get; private set; }
        public Enemy Boss { get; private set; }
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
        public static BossGrowthHandler BossGrowthHandler { get; private set; }
        public static BossPieceHandler BossPieceHandler { get; private set; }
        public static ObjectHandler ObjectHandler { get; private set; }
        public static Tier.Camera.Camera Camera { get; private set; }
        public static HUD HUD { get; private set; }
        public static Game Game { get; private set; }
        public static Player Player { get; private set; }
        #endregion

        public GameHandler(Game game)
        {
            Game = game;
            MenuState = new StartMenu();
            ObjectHandler = new ObjectHandler(game);
            HUD = new HUD();
            BossPieceHandler = new BossPieceHandler(game);
            BossGrowthHandler = new BossGrowthHandler();
        }

        private void InitializeComponents()
        {
            // Boss pieces
            BossPieceHandler.AddPiece("Bar", TierGame.ContentHandler.GetModel("Bar"));
            BossPieceHandler.AddPiece("L", TierGame.ContentHandler.GetModel("L"));
            BossPieceHandler.AddPiece("T", TierGame.ContentHandler.GetModel("T"));
            BossPieceHandler.AddPiece("Boss", TierGame.ContentHandler.GetModel("Boss"));
            BossPieceHandler.AddPiece("Sphere", TierGame.ContentHandler.GetModel("Sphere"));
            BossPieceHandler.AddPiece("Cube", TierGame.ContentHandler.GetModel("Cube"));
            BossPieceHandler.AddPiece("Cross", TierGame.ContentHandler.GetModel("Cross"));
            BossPieceHandler.AddPiece("Nico", TierGame.ContentHandler.GetModel("Nico"));

            Sphere sphere = new Sphere(Game, 34.0f);
            Player = new Player(Game, sphere);
            Camera = new DelayCamera(new Vector3(0f, 0f, 23f), Vector3.Zero, Player, new Vector3(0f, 0.1f, 0f));

            ObjectHandler.AddObject(sphere);
            ObjectHandler.AddObject(Player);

            NewGame();
        }

        public void NewGame()
        {
            Boss = BossPieceHandler.GetEnemy("Cube");
            Boss.GrowthPattern = BossGrowthHandler.Load("Default");
            Boss.Initialize();
            Boss.Spawn();
            CurrentLevel = 1;

            ObjectHandler.AddObject(Boss);
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

                InitializeComponents();
            }
        }

        public void Update(GameTime gameTime)
        {
            Effect ef = TierGame.ContentHandler.getEffect("BlockEffect");
            ef.Parameters["lightDir"].SetValue(Camera.Direction);
            Camera.Update(gameTime);

            MenuState.Update(gameTime);

            if (MenuState.GetType() == typeof(GameMenu))
                ObjectHandler.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            MenuState.Draw(gameTime);

            if (MenuState.GetType() == typeof(GameMenu))
                ObjectHandler.Draw(gameTime);
        }

        public void NextLevel()
        {
            CurrentLevel++;
            Boss.GrowBoss();
        }
    }
}
