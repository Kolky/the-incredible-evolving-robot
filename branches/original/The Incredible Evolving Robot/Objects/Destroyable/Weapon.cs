using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Tier.Handlers;

namespace Tier.Objects.Destroyable
{
  public abstract class Weapon : DestroyableObject
  {
    #region Properties
    private BasicObject source;
    public BasicObject Source
    {
      get { return source; }
      set { source = value; }
    }

    private int damage;
    public int Damage
    {
      get { return damage; }
      set { damage = value; }
    }	
    #endregion

    public Weapon(Game game, BasicObject source)
      : base(game)
    {
      this.source = source;
      GameHandler.ObjectHandler.AddObject(this);
    }

    abstract public void Fire();

    //sommige guns laten iets zien als ze aimen, denk aan dikke laser van enemy of piramide van speler
    virtual public void Aim(GameTime gameTime) { }
  }
}
