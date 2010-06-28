using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Tier.Menus;
using Tier.Objects;
using Tier.Objects.Attachable;
using Tier.Objects.Destroyable;
using Tier.Objects.Destroyable.Projectile;
using Tier.Misc;
#if XBOX360
using InstancedModelSample;
#else
using Instanced;
#endif

namespace Tier.Handlers
{
  public class ObjectHandler : GameComponent
  {
    #region Properties
    private LinkedList<BasicObject> objects;
    public LinkedList<BasicObject> Objects
    {
      get { return objects; }
    }

    public int Count
    {
      get { return objects.Count; }
    }

    private List<Object> toBeRemoved;
    private List<Object> toBeAdded;
    private BloomComponent bloom;
    private bool anythingChanged;

    //instancing stuff
    private Hashtable instancedCount;//count per sorteerbaar item!
    private Matrix[] instanceTransforms;
    private int instanceIndex;
    #endregion

    public ObjectHandler(Game game)
      : base(game)
    {
      this.objects = new LinkedList<BasicObject>();
      this.toBeAdded = new List<Object>();
      this.toBeRemoved = new List<Object>();

      this.bloom = new BloomComponent(game);
      this.bloom.Initialize();
      this.bloom.Settings = BloomSettings.PresetSettings[3];

      this.instancedCount = new Hashtable();
    }

    public void AddObject(Object obj)
    {
      if (!this.toBeAdded.Contains(obj))
        this.toBeAdded.Add(obj);
    }

    public void RemoveObject(Object obj)
    {
      if (!this.toBeRemoved.Contains(obj))
        this.toBeRemoved.Add(obj);
    }

    public override void Update(GameTime gameTime)
    {
      LinkedList<BasicObject>.Enumerator iter = this.Objects.GetEnumerator();
      while (iter.MoveNext())
      {
        iter.Current.Update(gameTime);

        // Skip collision when boss is respawning
        if (TierGame.GameHandler.Boss.IsSpawning)
          continue;

        //************************************
        //Do collision detection
        if (iter.Current.IsCollidable
          && iter.Current.GetType().IsSubclassOf(typeof(DestroyableObject))
          && iter.Current.GetType() != typeof(Player))
        {
          if (iter.Current.GetType() == typeof(LaserCluster))
          {	//Player shoots

            LaserCluster lcluster = (LaserCluster)iter.Current;

            LinkedList<BasicObject>.Enumerator iter2 = this.Objects.GetEnumerator();
            while (iter2.MoveNext())
            {
              if (iter2.Current.IsCollidable
                && iter2.Current.GetType().IsSubclassOf(typeof(DestroyableObject))
                && iter2.Current.GetType() != typeof(Player)
                && iter2.Current.GetType() != typeof(LaserCluster)
                && !((DestroyableObject)iter2.Current).Exploded)
              {
                if (lcluster.DetectCollision((DestroyableObject)iter2.Current))
                {
                  foreach (Laser laser in lcluster.Lasers)
                  {
                    if (laser.DetectCollision((DestroyableObject)iter2.Current))
                    {
                      DestroyableObject obj = ((DestroyableObject)iter2.Current);
                      lcluster.RemoveLaser(laser);

                      obj.ObjectHit();
                    }
                  }
                }
              }
            }
          }
          else if (GameHandler.Player.DetectCollision((DestroyableObject)iter.Current))
          {	//All other , what Player does not shoot :P
            GameHandler.HUD.HitUpdate();
            GameHandler.HUD.Die();
            this.toBeRemoved.Add(iter.Current);

            this.AddObject(new AnimatedBillboard(GameHandler.Game, "Explosion", false, iter.Current.Position.Coordinate, 1f, 150));
          }
        }
        //************************************
      }

      this.anythingChanged |= this.removeOld();
      this.anythingChanged |= this.addNew();
    }

    /// <summary>
    /// Voeg de objecten toe aan de object list die in de toBeAdded list staan
    /// </summary>
    /// <returns>true als er iets is toegevoegd</returns>
    public bool addNew()
    {
      if (this.toBeAdded.Count == 0)
        return false;

      for (int j = 0; j < this.toBeAdded.Count; j++)
      {
        if (this.Objects.Count == 0)
        {
          this.Objects.AddLast((BasicObject)this.toBeAdded[j]);
        }
        else
        {
          LinkedListNode<BasicObject> currentObj = this.Objects.First;
          bool found = false, determinedRightSpot = false;

          for (int i = 0; i < this.Objects.Count; i++)
          {
            if (found)
              break;

            if (!determinedRightSpot)
            {
              switch (currentObj.Value.CompareTo((BasicObject)this.toBeAdded[j]))
              {
                case 0:
                  this.Objects.AddBefore(currentObj, (BasicObject)this.toBeAdded[j]);
                  found = true; determinedRightSpot = true;
                  break;
                case -1:
                  determinedRightSpot = true;
                  break;
                case 1:
                  this.Objects.AddBefore(currentObj, (BasicObject)this.toBeAdded[j]);
                  found = true; determinedRightSpot = true;
                  break;
              }
            }

            if(determinedRightSpot)
            {
              if (currentObj.Value.CompareTo((BasicObject)this.toBeAdded[j]) != -1)
              {
                this.Objects.AddBefore(currentObj, (BasicObject)this.toBeAdded[j]);
                found = true;
              }
              else if ((i + 1) == this.Objects.Count)
              {
                this.Objects.AddAfter(currentObj, (BasicObject)this.toBeAdded[j]);
                found = true;
              }
            }
            currentObj = currentObj.Next;
          }
        }
      }

      this.toBeAdded.Clear();

      return true;
    }

    /// <summary>
    /// verwijder de objecten uit de object list die in de toBeRemoved list staan
    /// </summary>
    /// <returns>true als er iets is verwijderd</returns>
    public bool removeOld()
    {
      if (this.toBeRemoved.Count == 0)
        return false;

      for (int i = 0; i < this.toBeRemoved.Count; i++)
      {
        if (this.Objects.Contains((BasicObject)this.toBeRemoved[i]))
          this.Objects.Remove((BasicObject)this.toBeRemoved[i]);
      }
      this.toBeRemoved.Clear();

      return true;
    }


    public void Draw(GameTime gameTime)
    {
      if (this.anythingChanged)
      {
        this.setFirstLastMetaData();
      }

      LinkedList<BasicObject>.Enumerator iter = this.Objects.GetEnumerator();
      while (iter.MoveNext())
      {
        if (!iter.Current.IsInstanced)
        {
          if (iter.Current.Sort == SortFilter.Bloom && iter.Current.LastInList)
          {
            bloom.Draw(gameTime);
          }
          iter.Current.Draw(gameTime);
        }
        else //instanced
        {
          drawInstanced(iter.Current);
        }

#if DEBUG && BOUNDRENDER
        if (iter.Current.GetType().IsSubclassOf(typeof(DestroyableObject))
          && !((DestroyableObject)iter.Current).Exploded)
          ((DestroyableObject)iter.Current).DrawBoundingObjects();
#endif
      }
    }

    /// <summary>
    /// Per object in de objectlist word gekeken of dit de eerste of laatste van een reeks is.
    /// Bijvoorbeeld laatste bloom object of laatste instanced object van een bepaald type!
    /// </summary>
    private void setFirstLastMetaData()
    {
      if (this.Objects.Count != 0)
      {
        LinkedList<BasicObject>.Enumerator iter = this.Objects.GetEnumerator();

        iter.MoveNext();//ga naar eeste positie
        if (iter.Current.IsInstanced)
        {
          this.instancedCount[iter.Current.ModelName] = 1;
        }
        iter.Current.FirstInList = true;
        iter.Current.LastInList = false;//word op true gezet als dit het geval is

        BasicObject prev = iter.Current;
        while (iter.MoveNext())
        {
          if (iter.Current.Sort != prev.Sort
            || iter.Current.IsInstanced != prev.IsInstanced
            || (!iter.Current.ModelName.Equals(prev.ModelName) && iter.Current.IsInstanced))
          {
            iter.Current.FirstInList = true;
            iter.Current.LastInList = false;
            prev.LastInList = true;//first is net al gezet!
          }
          else
          {
            prev.LastInList = false;
            iter.Current.FirstInList = false;
          }
          
          if (iter.Current.IsInstanced)
          {
            if (this.instancedCount[iter.Current.ModelName] == null)
              this.instancedCount[iter.Current.ModelName] = 1;
            else
              this.instancedCount[iter.Current.ModelName] = (int)this.instancedCount[iter.Current.ModelName] + 1;
          }

          prev = iter.Current;
        }
        iter.Current.LastInList = true;
      }
    }

    private void drawInstanced(BasicObject bo)
    {
      if (bo.FirstInList)
      {
        int size = (int)this.instancedCount[bo.ModelName];
        Array.Resize(ref this.instanceTransforms, size);
        instanceIndex = 0;
      }

      this.instanceTransforms[instanceIndex++] = Matrix.CreateScale(bo.Scale) *
                                   bo.RotationFix *
                                   Matrix.CreateFromQuaternion(bo.Position.Front) *
                                   Matrix.CreateTranslation(bo.Position.Coordinate);

      if (bo.LastInList)
      {
        TierGame.Content.Load<InstancedModel>("Content//Models//" + bo.ModelName).DrawInstances(this.instanceTransforms, GameHandler.Camera.View, GameHandler.Camera.Projection);
        this.instancedCount[bo.ModelName] = 0;
      }
    }
  }
}