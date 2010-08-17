using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections;
using Tier.Source.Objects.Projectiles;
using Tier.Source.Objects;

namespace Tier.Source.Handlers
{
  public enum ProjectileHandlerType
  {
    PHT_PLAYER_PLASMA = 0,
    PHT_BOSS_PLASMA = 1,
    PHT_BOSS_LASER = 2,
    PHY_BOSS_SHOTGUN = 3  
  };

  public class ProjectileHandlerElement : IComparable
  {
    #region Properties
    public Vector3 direction;
    public Vector3 position;
    public bool isDone;
    public Projectile projectileBlueprint;
    public TierGame game;
    public int priority;
    public ProjectileHandlerType type;
    private int ttl;
    private int timeElapsed;
    #endregion
 
    public ProjectileHandlerElement(TierGame game)
    {
      this.game = game;
      this.isDone = true;
      this.direction = this.position = Vector3.Zero;
      this.projectileBlueprint = null;
      this.ttl = 0;
      this.timeElapsed = 0;
      this.priority = 0;
      this.type = ProjectileHandlerType.PHT_BOSS_LASER;
    }

    public void Start(Projectile projectile)
    {
      this.projectileBlueprint = projectile;
      this.isDone = false;
      this.ttl = projectileBlueprint.TemporaryModifier.TTL;
      this.timeElapsed = 0;
    }

    public void Stop()
    {
      this.isDone = true;      
    }

    public void Update(GameTime gameTime)
    {
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      if (this.timeElapsed >= this.ttl)
      {
        Stop();
        return;
      }

      // Setup values for projectiles ObjectModifiers
      this.projectileBlueprint.MovableModifier.Velocity =
        this.direction * this.projectileBlueprint.Speed;
      this.projectileBlueprint.Position = this.position;
      // Notify projectile blueprint of this ProjectileHandlerElement
      this.projectileBlueprint.HandlerElement = this;
      // Run update of blueprint projectile
      this.projectileBlueprint.Update(gameTime);
      // Retrieve modified values from blueprint object
      this.position = this.projectileBlueprint.Position;
    }

    public void Draw(GameTime gameTime)
    {
      this.projectileBlueprint.Position = this.position;
      this.projectileBlueprint.Draw(gameTime);
    }

    #region IComparable Members

    public int CompareTo(object obj)
    {
      ProjectileHandlerElement element = (ProjectileHandlerElement)obj;

      if (element.priority > this.priority)
        return 1;
      if (element.priority < this.priority)
        return -1;

      return 0;
    }

    #endregion
  }
  
  public class ProjectileHandler
  {
    private TierGame game;
    private ProjectileHandlerElement[] projectiles;
    private int NUMBER_OF_PROJECTILES = 200;
    // Used to store all unique projectiles and assign values stored in projectiles array
    private Hashtable projectileBlueprints;
    private bool isInitialized;
    private List<ProjectileHandlerElement> toBeAdded;

    public ProjectileHandler(TierGame game)
    {
      this.game = game;
      this.projectiles = new ProjectileHandlerElement[NUMBER_OF_PROJECTILES];
      this.toBeAdded = new List<ProjectileHandlerElement>();
      this.isInitialized = false;
    }

    public void Clear()
    {
      for (int i = 0; i < NUMBER_OF_PROJECTILES; i++)
      {
        projectiles[i].isDone = true;
      }
    }

    public void Draw(GameTime gameTime)
    {
      if (!this.isInitialized)
        return;

      for (int i = 0; i < NUMBER_OF_PROJECTILES; i++)
      {
        if (projectiles[i].isDone)
          continue;

        projectiles[i].Draw(gameTime);
      }
    }

    public void SetQuadDamageColor()
    {
      GameObject obj = (GameObject)this.projectileBlueprints[ProjectileHandlerType.PHT_PLAYER_PLASMA];
      obj.Color = Color.Blue;
    }

    /// <summary>
    /// Resets color values of projectile blueprints
    /// </summary>
    public void ResetColor()
    {
      for (int i = 0; i < sizeof(ProjectileHandlerType); i++)
      {
        GameObject obj = null;

        switch (i)
        {
          case (int)ProjectileHandlerType.PHT_PLAYER_PLASMA:
            obj = (GameObject)this.projectileBlueprints[ProjectileHandlerType.PHT_PLAYER_PLASMA];
            obj.Color = Color.Gold;
            break;
          case (int)ProjectileHandlerType.PHT_BOSS_PLASMA:
            obj = (GameObject)this.projectileBlueprints[ProjectileHandlerType.PHT_BOSS_PLASMA];
            obj.Color = Color.LimeGreen;
            break;
          case (int)ProjectileHandlerType.PHT_BOSS_LASER:
            obj = (GameObject)this.projectileBlueprints[ProjectileHandlerType.PHT_BOSS_LASER];
            obj.Color = Color.Red;
            break;
          case (int)ProjectileHandlerType.PHY_BOSS_SHOTGUN:
            obj = (GameObject)this.projectileBlueprints[ProjectileHandlerType.PHY_BOSS_SHOTGUN];
            obj.Color = Color.Gold;
            break;
        }
      }
    }

    public bool CreateParticle(Vector3 position, Vector3 direction, ProjectileHandlerType type)
    {
      /*for (int i = 0; i < NUMBER_OF_PROJECTILES; i++)
      {
        if (projectiles[i].IsDone)
        {
          projectiles[i].Position = position;
          projectiles[i].Direction = direction;
          projectiles[i].Start((Projectile)this.projectileBlueprints[type]);

          return true;
        }
      }

      return false;*/
      ProjectileHandlerElement element = new ProjectileHandlerElement(this.game);
      element.direction = direction;
      element.position = position;
      element.type = type;

      switch (type)
      {
        case ProjectileHandlerType.PHT_PLAYER_PLASMA:
          element.priority = 4;
          break;
        case ProjectileHandlerType.PHT_BOSS_PLASMA:
          element.priority = 3;
          break;
        case ProjectileHandlerType.PHT_BOSS_LASER:
          element.priority = 2;
          break;
        case ProjectileHandlerType.PHY_BOSS_SHOTGUN:
          element.priority = 1;
          break;
      }

      this.toBeAdded.Add(element);

      return true;
    }

    public Projectile GetProjectileBlueprint(ProjectileHandlerType type)
    {
      return (Projectile)this.projectileBlueprints[type];
    }

    public void Initialize()
    {
      // Create blueprints
      projectileBlueprints = new Hashtable();

      // Player plasma
      {
        Projectile p = new PlayerLaserProjectile(this.game);
        if (this.game.ObjectHandler.InitializeFromBlueprint<Projectile>(
          p, "PlayerLaserProjectile"))
        {
          p.Color = Color.Gold;
          this.projectileBlueprints[ProjectileHandlerType.PHT_PLAYER_PLASMA] = p;
          p.SetEffectParameters();
        }
      }
      // Boss plasma
      {
        Projectile p = new PlasmaProjectile(this.game);
        if (this.game.ObjectHandler.InitializeFromBlueprint<Projectile>(
          p, "Plasma"))
        {
          p.Color = Color.LimeGreen;
          this.projectileBlueprints[ProjectileHandlerType.PHT_BOSS_PLASMA] = p;
          p.SetEffectParameters();
        }
      }
      // Boss laser
      {
        Projectile p = new LaserProjectile(this.game);
        if (this.game.ObjectHandler.InitializeFromBlueprint<Projectile>(
          p, "LaserProjectile"))
        {
          p.Color = Color.Red;
          this.projectileBlueprints[ProjectileHandlerType.PHT_BOSS_LASER] = p;
          p.SetEffectParameters();
        }
      }
      // Boss shotgun shell
      {
        Projectile p = new ShotgunProjectile(this.game);

        if (this.game.ObjectHandler.InitializeFromBlueprint<Projectile>(p, "ShotgunProjectile"))
        {
          this.projectileBlueprints[ProjectileHandlerType.PHY_BOSS_SHOTGUN] = p;          
          p.Color = Color.Gold;
          p.TextureName = "ShotgunProjectile";
          p.SetEffectParameters();
        }
      }

      for (int i = 0; i < NUMBER_OF_PROJECTILES; i++)
      {
        projectiles[i] = new ProjectileHandlerElement(game);
      }

      this.isInitialized = true;
    }

    private void AddObjects()
    {
      this.toBeAdded.Sort();

      for (int j = 0; j < this.toBeAdded.Count; j++)
			{
        for (int i = 0; i < NUMBER_OF_PROJECTILES; i++)
        {
          if (!projectiles[i].isDone)
            continue;

          projectiles[i] = this.toBeAdded[j];
          projectiles[i].isDone = false;
          projectiles[i].Start((Projectile)this.projectileBlueprints[projectiles[i].type]);
          break;
        }			 
			}
      this.toBeAdded.Clear();
    }

    public void Update(GameTime gameTime)
    {
      if (!this.isInitialized)
        return;

      for (int i = 0; i < NUMBER_OF_PROJECTILES; i++)
      {
        if (projectiles[i].isDone)
          continue;

        projectiles[i].Update(gameTime);
      }

      if (this.toBeAdded.Count > 0)
      {
        AddObjects();
      }
    }
  }
}
