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
        public LinkedList<BasicObject> Objects { get; private set; }
        public int Count
        {
            get { return Objects.Count; }
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
            Objects = new LinkedList<BasicObject>();
            toBeAdded = new List<Object>();
            toBeRemoved = new List<Object>();

            bloom = new BloomComponent(game);
            bloom.Initialize();
            bloom.Settings = BloomSettings.PresetSettings[3];

            instancedCount = new Hashtable();
        }

        public void AddObject(Object obj)
        {
            if (!toBeAdded.Contains(obj))
                toBeAdded.Add(obj);
        }

        public void RemoveObject(Object obj)
        {
            if (!toBeRemoved.Contains(obj))
                toBeRemoved.Add(obj);
        }

        public override void Update(GameTime gameTime)
        {
            LinkedList<BasicObject>.Enumerator iter = Objects.GetEnumerator();
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

                        LinkedList<BasicObject>.Enumerator iter2 = Objects.GetEnumerator();
                        while (iter2.MoveNext())
                        {
                            if (iter2.Current.GetType().IsSubclassOf(typeof(DestroyableObject)))
                            {
                                DestroyableObject destroyable = (DestroyableObject)iter2.Current;
                                if (iter2.Current.IsCollidable && destroyable != null && !destroyable.Exploded
                                  && iter2.Current.GetType() != typeof(Player) && iter2.Current.GetType() != typeof(LaserCluster))
                                {
                                    if (lcluster.DetectCollision(destroyable))
                                    {
                                        foreach (Laser laser in lcluster.Lasers)
                                        {
                                            if (laser.DetectCollision(destroyable))
                                            {
                                                lcluster.RemoveLaser(laser);
                                                destroyable.ObjectHit();
                                            }
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
                        toBeRemoved.Add(iter.Current);

                        AddObject(new AnimatedBillboard(GameHandler.Game, "Explosion", false, iter.Current.Position.Coordinate, 1f, 150));
                    }
                }
                //************************************
            }

            anythingChanged |= removeOld();
            anythingChanged |= addNew();
        }

        /// <summary>
        /// Voeg de objecten toe aan de object list die in de toBeAdded list staan
        /// </summary>
        /// <returns>true als er iets is toegevoegd</returns>
        public bool addNew()
        {
            if (toBeAdded.Count == 0)
                return false;

            for (int j = 0; j < toBeAdded.Count; j++)
            {
                if (Objects.Count == 0)
                {
                    Objects.AddLast((BasicObject)toBeAdded[j]);
                }
                else
                {
                    LinkedListNode<BasicObject> currentObj = Objects.First;
                    bool found = false, determinedRightSpot = false;

                    for (int i = 0; i < Objects.Count; i++)
                    {
                        if (found)
                            break;

                        if (!determinedRightSpot)
                        {
                            switch (currentObj.Value.CompareTo((BasicObject)toBeAdded[j]))
                            {
                                case 0:
                                    Objects.AddBefore(currentObj, (BasicObject)toBeAdded[j]);
                                    found = true; determinedRightSpot = true;
                                    break;
                                case -1:
                                    determinedRightSpot = true;
                                    break;
                                case 1:
                                    Objects.AddBefore(currentObj, (BasicObject)toBeAdded[j]);
                                    found = true; determinedRightSpot = true;
                                    break;
                            }
                        }

                        if (determinedRightSpot)
                        {
                            if (currentObj.Value.CompareTo((BasicObject)toBeAdded[j]) != -1)
                            {
                                Objects.AddBefore(currentObj, (BasicObject)toBeAdded[j]);
                                found = true;
                            }
                            else if ((i + 1) == Objects.Count)
                            {
                                Objects.AddAfter(currentObj, (BasicObject)toBeAdded[j]);
                                found = true;
                            }
                        }
                        currentObj = currentObj.Next;
                    }
                }
            }

            toBeAdded.Clear();

            return true;
        }

        /// <summary>
        /// verwijder de objecten uit de object list die in de toBeRemoved list staan
        /// </summary>
        /// <returns>true als er iets is verwijderd</returns>
        public bool removeOld()
        {
            if (toBeRemoved.Count == 0)
                return false;

            for (int i = 0; i < toBeRemoved.Count; i++)
            {
                if (Objects.Contains((BasicObject)toBeRemoved[i]))
                    Objects.Remove((BasicObject)toBeRemoved[i]);
            }
            toBeRemoved.Clear();

            return true;
        }


        public void Draw(GameTime gameTime)
        {
            if (anythingChanged)
            {
                setFirstLastMetaData();
            }

            LinkedList<BasicObject>.Enumerator iter = Objects.GetEnumerator();
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
                {
                    ((DestroyableObject)iter.Current).DrawBoundingObjects();
                }
#endif
            }
        }

        /// <summary>
        /// Per object in de objectlist word gekeken of dit de eerste of laatste van een reeks is.
        /// Bijvoorbeeld laatste bloom object of laatste instanced object van een bepaald type!
        /// </summary>
        private void setFirstLastMetaData()
        {
            if (Objects.Count != 0)
            {
                LinkedList<BasicObject>.Enumerator iter = Objects.GetEnumerator();

                iter.MoveNext();//ga naar eeste positie
                if (iter.Current.IsInstanced)
                {
                    instancedCount[iter.Current.ModelName] = 1;
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
                        if (instancedCount[iter.Current.ModelName] == null)
                            instancedCount[iter.Current.ModelName] = 1;
                        else
                            instancedCount[iter.Current.ModelName] = (int)instancedCount[iter.Current.ModelName] + 1;
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
                int size = (int)instancedCount[bo.ModelName];
                Array.Resize(ref instanceTransforms, size);
                instanceIndex = 0;
            }

            instanceTransforms[instanceIndex++] = Matrix.CreateScale(bo.Scale) *
                                         bo.RotationFix *
                                         Matrix.CreateFromQuaternion(bo.Position.Front) *
                                         Matrix.CreateTranslation(bo.Position.Coordinate);

            if (bo.LastInList)
            {
                TierGame.Instance.Content.Load<InstancedModel>("Content//Models//" + bo.ModelName).DrawInstances(instanceTransforms, GameHandler.Camera.View, GameHandler.Camera.Projection);
                instancedCount[bo.ModelName] = 0;
            }
        }
    }
}