using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tier.Source.Helpers
{
  public class Text : IDisposable
  {
    #region Properties
    private string textvalue;
    private Vector2 position;
    private Color color;
    private float scale;
    private bool isVisible;

    public bool  IsVisible
    {
      get { return isVisible; }
      set { isVisible = value; }
    }
	
    public float Scale
    {
      get { return scale; }
      set { scale = value; }
    }
	
    public Color Color
    {
      get { return color; }
      set { color = value; }
    }
	
    public Vector2 Position
    {
      get { return position; }
      set { position = value; }
    }
	
    public string Value
    {
      get { return textvalue; }
      set { textvalue = value; }
    }
    #endregion

    public Text(string value, Vector2 position)
      : this(value, position, Color.White)
    { }

    public Text(string value, Vector2 position, Color color)
    {
      this.textvalue = value;
      this.position = position;
      this.color = color;
      this.scale = 1;
      this.isVisible = true;
    }

    #region IDisposable Members

    public void Dispose()
    {      
    }

    #endregion
  }
}
