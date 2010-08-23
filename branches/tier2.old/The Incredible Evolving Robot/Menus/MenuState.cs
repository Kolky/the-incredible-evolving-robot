using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Tier.Misc;

namespace Tier.Menus
{
  public abstract class MenuState : IDisposable
  {
    public class MenuOptions : IDisposable
    {
      #region Properties
      private List<String> options;
      public List<String> Options
      {
        get { return options; }
      }

      private int currentOption;
      public int CurrentOption
      {
        get { return currentOption; }
        set { currentOption = value; }
      }	

      private float pos;
      #endregion

      public MenuOptions()
      {
        this.options = new List<String>();
        this.CurrentOption = 0;
        this.pos = 150f;
      }

			public void AddItem(String item)
			{
				this.AddItem(item, item);
			}

      public void AddItem(String key, String item)
      {
        float center = TierGame.Instance.Window.ClientBounds.Width * 0.5f;

				this.Options.Add(key);
				TierGame.TextHandler.AddItem(key, item, new Vector2(center, this.pos), Color.White, true, false);
        this.pos += 30f;
      }

			public void ChangeItemText(String item, String text)
			{
				TierGame.TextHandler.ChangeText(item, text);
			}

      public void Update(GameTime gameTime)
      {
        if (this.CurrentOption < this.Options.Count)
          TierGame.TextHandler.ChangeColor(this.Options[this.CurrentOption], Tier.Misc.Options.Colors.ActiveColor);

#if XBOX360
        if(TierGame.Input.checkKey(Tier.Controls.GamePadKey.DPADUP))
        {
#else
        if (TierGame.Input.checkKey(Keys.Up))
        {
#endif
          if(this.CurrentOption > 0)
          {
            TierGame.TextHandler.ChangeColor(this.Options[this.CurrentOption], Tier.Misc.Options.Colors.MenuColor);
            this.CurrentOption--;
          }
          else if(this.CurrentOption != (this.Options.Count - 1))
          {
            TierGame.TextHandler.ChangeColor(this.Options[this.CurrentOption], Tier.Misc.Options.Colors.MenuColor);
            this.CurrentOption = (this.Options.Count - 1);
          }
        }
#if XBOX360
        else if (TierGame.Input.checkKey(Tier.Controls.GamePadKey.DPADDOWN))
        {
#else
        else if (TierGame.Input.checkKey(Keys.Down))
        {
#endif
          if (this.CurrentOption < (this.Options.Count - 1))
          {
            TierGame.TextHandler.ChangeColor(this.Options[this.CurrentOption], Tier.Misc.Options.Colors.MenuColor);
            this.CurrentOption++;
          }
          else if(this.CurrentOption != 0)
          {
            TierGame.TextHandler.ChangeColor(this.Options[this.CurrentOption], Tier.Misc.Options.Colors.MenuColor);
            this.CurrentOption = 0;
          }
        }
      }
      #region IDisposable Members
      public void Dispose()
      {
        foreach (String str in this.Options)
        {
          TierGame.TextHandler.RemoveItem(str);
        }
      }
      #endregion
    }

    #region Properties
    private MenuOptions menuItems;
    public MenuOptions MenuItems
    {
      get { return menuItems; }
    }
    private float center;
    public float Center
    {
      get { return center; }
    }
    #endregion

    public MenuState()
    {
      this.center = TierGame.Instance.Window.ClientBounds.Width * 0.5f;
    }

    public virtual void Initialize()
    {
      this.menuItems = new MenuOptions();

      TierGame.TextHandler.AddItem("headerText", "", new Vector2(TierGame.Instance.Window.ClientBounds.Width * 0.5f, 100f), Color.DarkGray, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, true, false);
    }

    public virtual void Update(GameTime gameTime)
    {
      this.MenuItems.Update(gameTime);      
    }

    public virtual void Draw(GameTime gameTime)
    {
      TierGame.Device.Clear(Options.Colors.ClearColor);
    }

    #region IDisposable Members
    public virtual void Dispose()
    {
      TierGame.TextHandler.RemoveItem("headerText");
      this.MenuItems.Dispose();
    }
    #endregion
  }
}
