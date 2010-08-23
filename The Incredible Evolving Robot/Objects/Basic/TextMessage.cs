using System;
using System.Collections.Generic;
using System.Text;
using Tier.Handlers;
using Microsoft.Xna.Framework;

namespace Tier.Objects.Basic
{
  class TextMessage : BasicObject
  {
    #region Properties
    private int ttl;
    private int elapsed;
    private string key;
    

    public int TimeToLive
    {
      get { return ttl; }
      set { ttl = value; }
    }
    #endregion

    public TextMessage(Game game)
      : base(game)
    { }

    public void SetMessage(string key, string value, Vector2 position)
    {
      try
      {
        this.key = key;
        TierGame.TextHandler.AddItem(key, value, position);
      }
      catch (Exception e)
      {               
      }
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      this.elapsed += gameTime.ElapsedGameTime.Milliseconds;

      if (this.elapsed >= this.ttl)
      {
        TierGame.TextHandler.RemoveItem(key);
        GameHandler.ObjectHandler.RemoveObject(this);
      }
    }
  }
}
