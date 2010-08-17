using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Tier.Source.Objects;
using Tier.Source.Misc;
using Tier.Source.Helpers.Cameras;
using pjEngine.Helpers;
using Tier.Source.Objects.Turrets;
using Tier.Source.Objects.Projectiles;
using Tier.Source.Objects.Powerups;
using Tier.Source.GameStates;

namespace Tier.Source.Handlers
{
  public class GameHandler
  {
    #region Properties
    private Matrix view;
    private Matrix world;
    private Matrix projection;
    private Camera camera;
    private List<Player> players;
    private OcTree octree;
    private TierGame game;
    private Player mainPlayer;
    private int playerHit;
    private bool bossDestroyed;
    private int currentLevel;
    private int piecesGrown;
    private bool isUpdateBossBoundingBox;

    public int  PiecesGrown
    {
      get { return piecesGrown; }
      set { piecesGrown = value; }
    }
	
    public int CurrentLevel
    {
      get { return currentLevel; }
      set { currentLevel = value; }
    }


    public bool IsBossDestroyed
    {
      get { return bossDestroyed; }
      set { bossDestroyed = value; }
    }

    public int PlayerHit
    {
      get { return playerHit; }
      set { playerHit = value; }
    }

    /// <summary>
    /// The main player playing the game.
    /// </summary>
    public Player Player
    {
      get { return mainPlayer; }
      set { mainPlayer = value; }
    }

    public OcTree Octree
    {
      get { return octree; }
      set { octree = value; }
    }

    public List<Player> Players
    {
      get { return players; }
      set { players = value; }
    }

    public Camera Camera
    {
      get { return camera; }
      set { camera = value; }
    }

    public Matrix Projection
    {
      get { return projection; }
      set { projection = value; }
    }

    public Matrix World
    {
      get { return world; }
      set { world = value; }
    }

    public Matrix View
    {
      get { return view; }
      set { view = value; }
    }
    #endregion

    #region Privates
    private List<GameObject> deferredObjects;
    private List<GameObject> alphaBlendObjects;
    private List<GameObject> transparentObjects;
    private List<GameObject> skyboxObjects;
    private List<GameObject> defaultObjects;
    private List<GameObject> defaultTexturedObjects;
    private List<ObjectRemovalHelper> toBeRemoved;
    private List<ObjectRemovalHelper> toBeAdded;
    private List<ObjectDamageHelper> damagedObjects;
    private List<ExplosionHandler> explosionHandlers;
    private List<ExplosionHandler> toBeDeletedExplosionHandlers;
    private List<GameObject> extraGrows;
    private struct ObjectRemovalHelper
    {
      public GameObject obj;
      public ObjectType type;
    };

    private struct ObjectDamageHelper
    {
      public GameObject obj;
      public float damage;
    };
    #endregion

    public enum ObjectType
    {
      Deferred, AlphaBlend, Transparent, Skybox, Default, DefaultTextured
    };

    public GameHandler(TierGame game)
    {
      this.game = game;

      this.defaultObjects = new List<GameObject>();
      this.defaultTexturedObjects = new List<GameObject>();
      this.deferredObjects = new List<GameObject>();
      this.alphaBlendObjects = new List<GameObject>();
      this.skyboxObjects = new List<GameObject>();
      this.transparentObjects = new List<GameObject>();
      this.players = new List<Player>();
      this.toBeRemoved = new List<ObjectRemovalHelper>();
      this.toBeAdded = new List<ObjectRemovalHelper>();
      this.octree = new OcTree(new Vector3(-20000), new Vector3(20000), 0, new GameObjectSegment());
      this.damagedObjects = new List<ObjectDamageHelper>();
      this.explosionHandlers = new List<ExplosionHandler>();
      this.extraGrows = new List<GameObject>();
      this.toBeDeletedExplosionHandlers = new List<ExplosionHandler>();

      this.world = this.projection = this.view = Matrix.Identity;
      this.currentLevel = 0;
      this.isUpdateBossBoundingBox = true;
    }

    public void AddObject(GameObject obj)
    {
      ObjectRemovalHelper helper = new ObjectRemovalHelper();
      helper.obj = obj;
      helper.type = obj.Type;

      this.toBeAdded.Add(helper);
    }

    /// <summary>
    /// Objects which are killed first will grow faster
    /// </summary>
    /// <param name="o"></param>
    public void AddExtraGrowth(GameObject o)
    {
      int currentGrowthAmount = GetBlockGrowthCount();

      if(this.extraGrows.Count > 2)
      {
        return;
      }

      if(!this.extraGrows.Contains(o))
        this.extraGrows.Add(o);
    }

    public void AddExplosionHandler(ExplosionHandler handler)
    {
      this.explosionHandlers.Add(handler);
    }

    public void AddObjectToOctree(GameObject obj)
    {
      IOcTreeSegment<GameObject> segment = Octree.AddObject(obj);

      if (segment != null)
      {
        obj.CollisionModifier.OcTreeSegment = segment;
      }
    }

    public void AddDamagedObject(GameObject obj, float damage)
    {
      if (obj.FadingModifier != null &&
        obj.FadingModifier.FadeAmount < 1)
        return;

      ObjectDamageHelper h = new ObjectDamageHelper();
      h.damage = damage; h.obj = obj;

      this.damagedObjects.Add(h);
    }

    public void AddObjectInstantly(GameObject obj)
    {
      switch (obj.Type)
      {
        case ObjectType.Deferred:
          this.deferredObjects.Add(obj);
          break;
        case ObjectType.AlphaBlend:
          this.alphaBlendObjects.Add(obj);
          break;
        case ObjectType.Transparent:
          this.transparentObjects.Add(obj);
          break;
        case ObjectType.Skybox:
          this.skyboxObjects.Add(obj);
          break;
        case ObjectType.Default:
          this.defaultObjects.Add(obj);
          break;
        case ObjectType.DefaultTextured:
          this.defaultTexturedObjects.Add(obj);
          break;
      }

      if (obj.GetType() == typeof(BossPiece))
      {
        this.game.BossCompositionHandler.AddBossPiece((BossPiece)obj);
        this.game.GameHandler.AddObjectToOctree(obj);
      }
    }

    private void AddObjects()
    {
      foreach (ObjectRemovalHelper helper in this.toBeAdded)
      {
        switch (helper.type)
        {
          case ObjectType.Deferred:
            this.deferredObjects.Add(helper.obj);
            break;
          case ObjectType.AlphaBlend:
            this.alphaBlendObjects.Add(helper.obj);
            break;
          case ObjectType.Transparent:
            this.transparentObjects.Add(helper.obj);
            break;
          case ObjectType.Skybox:
            this.skyboxObjects.Add(helper.obj);
            break;
          case ObjectType.Default:
            this.defaultObjects.Add(helper.obj);
            break;
          case ObjectType.DefaultTextured:
            this.defaultTexturedObjects.Add(helper.obj);
            break;
        }

        if (helper.obj.GetType() == typeof(BossPiece))
        {
          AddObjectToOctree(helper.obj);
          // Add to composition handler 
          this.game.BossCompositionHandler.AddBossPiece((BossPiece)helper.obj);
        }
        if (helper.obj.GetType().IsSubclassOf(typeof(Turret)))
        {
          AddObjectToOctree(helper.obj);
          // Add to composition handler 
          this.game.BossCompositionHandler.AddTurret((Turret)helper.obj);
        }
      }

      this.toBeAdded.Clear();
    }

    /// <summary>
    /// Clear level of temporary objects
    /// </summary>
    public void ClearLevel()
    {
      foreach (GameObject obj in this.defaultTexturedObjects)
      {
        RemoveObject(obj, ObjectType.Deferred);
      }

      foreach (GameObject obj in this.transparentObjects)
      {
        RemoveObject(obj, ObjectType.Transparent);
      }

      foreach (GameObject obj in this.alphaBlendObjects)
      {
        RemoveObject(obj, ObjectType.AlphaBlend);
      }

      RemoveObjects();
      // Clear explosion handlers
      this.explosionHandlers.Clear();
    }

    public void ClearObjects(ObjectType type)
    {
      switch (type)
      {
        case ObjectType.Deferred:
          break;
        case ObjectType.AlphaBlend:
          foreach (GameObject obj in this.alphaBlendObjects)
          {
            this.RemoveObject(obj, ObjectType.AlphaBlend);
          }
          this.alphaBlendObjects.Clear();
          break;
        case ObjectType.Transparent:
          foreach (GameObject obj in this.transparentObjects)
          {
            this.RemoveObject(obj, ObjectType.Transparent);
          }
          this.transparentObjects.Clear();
          break;
        case ObjectType.Skybox:
          break;
        case ObjectType.Default:
          break;
        case ObjectType.DefaultTextured:
          foreach (GameObject obj in this.defaultTexturedObjects)
          {
            this.RemoveObject(obj, ObjectType.DefaultTextured);
          }
          this.defaultTexturedObjects.Clear();
          break;
        default:
          break;
      }
    }
    
    public void Draw(GameTime gameTime, ObjectType type)
    {
      switch (type)
      {
        case ObjectType.Deferred:
          foreach (GameObject obj in this.deferredObjects)
          {
            obj.Draw(gameTime);
          }
          break;
        case ObjectType.AlphaBlend:
          foreach (GameObject obj in this.alphaBlendObjects)
          {
            obj.Draw(gameTime);
          }
          break;
        case ObjectType.Transparent:
          foreach (GameObject obj in this.transparentObjects)
          {
            obj.Draw(gameTime);
          }
          break;
        case ObjectType.Skybox:
          foreach (GameObject obj in this.skyboxObjects)
          {
            obj.Draw(gameTime);
          }
          break;
        case ObjectType.Default:
          foreach (GameObject obj in this.defaultObjects)
          {
            obj.Draw(gameTime);
          }
          break;
        case ObjectType.DefaultTextured:
          foreach (GameObject obj in this.defaultTexturedObjects)
          {
            obj.Draw(gameTime);
          }
          break;
      }
    }

    public int GetBlockGrowthCount()
    {
      return 1 + (int)(this.game.Options.BossGrowthSpeed * currentLevel);
    }

    public void GetObjects(out List<GameObject> objects, ObjectType type)
    {
      objects = null;
      switch (type)
      {
        case ObjectType.AlphaBlend:
          objects = this.alphaBlendObjects;
          break;
        case ObjectType.DefaultTextured:
          objects = this.defaultTexturedObjects;
          break;
      }
    }

    public bool HasObjects(ObjectType type)
    {
      bool result = false;

      switch (type)
      {
        case ObjectType.Deferred:
          result = this.defaultObjects.Count > 0;
          break;
        case ObjectType.AlphaBlend:
          result = this.alphaBlendObjects.Count > 0;
          break;
        case ObjectType.Transparent:
          result = this.transparentObjects.Count > 0;
          break;
        case ObjectType.Skybox:
          result = this.skyboxObjects.Count > 0;
          break;
        case ObjectType.Default:
          result = this.defaultObjects.Count > 0;
          break;
        case ObjectType.DefaultTextured:
          result = this.defaultTexturedObjects.Count > 0;
          break;
      }

      return result;
    }

    public bool IsBossGrown()
    {
      return piecesGrown >= GetBlockGrowthCount();
    }

    public void DetermineIfPlayerIsNearPowerup()
    {
      List<GameObject> objects = null;
      this.game.GameHandler.GetObjects(out objects, GameHandler.ObjectType.DefaultTextured);
      DelayCamera cam;

      if (this.camera.GetType() == typeof(DelayCamera))
      {
        cam = (DelayCamera)this.camera;
      }
      else
        return;
      
      cam.Offset = this.game.Options.DelayCamera_DefaultOffset;

      if (objects != null)
      {
        Vector3 playerDiff = this.game.GameHandler.Player.Position;
        playerDiff.Normalize();

        foreach (GameObject obj in objects)
        {
          if (obj.GetType().IsSubclassOf(typeof(Powerup)))
          {
            Vector3 powerupDiff = obj.Position;
            powerupDiff.Normalize();

            float angle = (float)Math.Acos(Vector3.Dot(powerupDiff, playerDiff));
            if (angle <= MathHelper.PiOver4)
            {
              cam.Offset = this.game.Options.DelayCamera_NearPowerupOffset;
              return;
            }
          }
        }
      }
    }

    public void NextLevel()
    {
      currentLevel++;
      this.piecesGrown = 0;

      foreach (GameObject obj in this.defaultTexturedObjects)
      {
        if (obj.GetType() == typeof(BossPiece))
        {
          obj.DestroyableModifier.Reset();
        }
        else if (obj.GetType().IsSubclassOf(typeof(Turret)))
        {
          obj.DestroyableModifier.Reset();
        }
      }

      this.game.BehaviourHandler.Reset();
      this.game.BossGrowthHandler.Grow();
      AddObjects();
    }

    public void ShowNextBoss()
    {
      this.game.ChangeState(this.game.BossIntroductionState);
    }
  
    private void RemoveDamagedObjects()
    {
      foreach (ObjectDamageHelper helper in this.damagedObjects)
      {
        if(helper.obj.DestroyableModifier != null)
          helper.obj.DestroyableModifier.Health -= helper.damage;
        
        if (helper.obj.GetType() == typeof(BossPiece))
        {
          // Gain player life
          this.Player.IsLifeGain = true;
          // Grow boss
          ((BossPiece)helper.obj).GrowChildren((int)helper.damage, helper.obj);
        }             
      }

      this.damagedObjects.Clear();
    }

    public void RemoveExplosionHandler(ExplosionHandler handler)
    {
      this.toBeDeletedExplosionHandlers.Add(handler);
    }

    public void RemoveObject(GameObject obj, ObjectType type)
    {
      ObjectRemovalHelper t = new ObjectRemovalHelper();
      t.obj = obj;
      t.type = type;

      this.toBeRemoved.Add(t);
    }


    /// <summary>
    /// Removes all powerups currently ingame.
    /// </summary>
    public void RemovePowerups()
    {
      foreach (GameObject obj in this.defaultTexturedObjects)
      {
        if (obj.GetType().IsSubclassOf(typeof(Powerup)))
        {
          RemoveObject(obj, ObjectType.DefaultTextured);
        }
      }
    }

    private void RemoveObjects()
    {
      foreach (ObjectRemovalHelper helper in this.toBeRemoved)
      {
        switch (helper.type)
        {
          case ObjectType.Deferred:
            deferredObjects.Remove(helper.obj);
            break;
          case ObjectType.AlphaBlend:
            alphaBlendObjects.Remove(helper.obj);
            break;
          case ObjectType.Transparent:
            transparentObjects.Remove(helper.obj);
            break;
          case ObjectType.Skybox:
            skyboxObjects.Remove(helper.obj);
            break;
          case ObjectType.Default:
            defaultObjects.Remove(helper.obj);
            break;
          case ObjectType.DefaultTextured:
            defaultTexturedObjects.Remove(helper.obj);
            break;
          default:
            break;
        }

        if (helper.obj != null)
        {
          if (helper.obj.CollisionModifier != null)
          {
            this.octree.RemoveObject(helper.obj);
          }

          helper.obj.Dispose();
        }
      }

      this.toBeRemoved.Clear();
    }

    public void Update(GameTime gameTime)
    {
      AddObjects();
      RemoveDamagedObjects();
      RemoveObjects();

      Update(gameTime, ObjectType.AlphaBlend);
      Update(gameTime, ObjectType.DefaultTextured);
      Update(gameTime, ObjectType.Skybox);
      Update(gameTime, ObjectType.Transparent);
      UpdateExplosionHandlers(gameTime);

      this.camera.Update(gameTime);
      this.view = this.camera.LookAtMatrix;

      this.game.BossCompositionHandler.DetermineBossPiecesBoundingBox();      
    }

    private void UpdateExplosionHandlers(GameTime gameTime)
    {
      foreach (ExplosionHandler exHandler in this.explosionHandlers)
      {
        exHandler.Update(gameTime);
      }
      if (this.toBeDeletedExplosionHandlers.Count > 0)
      {
        foreach (ExplosionHandler handler in this.toBeDeletedExplosionHandlers)
        {
          this.explosionHandlers.Remove(handler);
        }

        if (this.explosionHandlers.Count <= 0)
        {
          // Only update the boss bounding box when all explosions are finished
          this.isUpdateBossBoundingBox = true;
        }
      }
    }

    public void Update(GameTime gameTime, ObjectType type)
    {
      switch (type)
      {
        case ObjectType.Deferred:
          foreach (GameObject obj in this.deferredObjects)
          {
            obj.Update(gameTime);
          }
          break;
        case ObjectType.AlphaBlend:
          foreach (GameObject obj in this.alphaBlendObjects)
          {
            obj.Update(gameTime);
          }
          break;
        case ObjectType.Transparent:
          foreach (GameObject obj in this.transparentObjects)
          {
            obj.Update(gameTime);
          }
          break;
        case ObjectType.Skybox:
          foreach (GameObject obj in this.skyboxObjects)
          {
            obj.Update(gameTime);
          }
          break;
        case ObjectType.Default:
          foreach (GameObject obj in this.defaultObjects)
          {
            obj.Update(gameTime);
          }
          break;
        case ObjectType.DefaultTextured:
          foreach (GameObject obj in this.defaultTexturedObjects)
          {
            obj.Update(gameTime);
          }
          break;
      }
    }
  }
}