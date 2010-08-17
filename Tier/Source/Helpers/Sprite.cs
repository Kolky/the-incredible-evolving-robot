using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tier.Source.Helpers
{
  // Sprites can be drawn normally and using a destination rectangle on which it will be drawn stretched
  public enum SpriteType
  {
    NORMAL, DESTINATION_RECTANGLE
  };

  public class Sprite
  {
    #region Properties
    private Texture2D texture;
    private Vector2 position;
    private Color color;
    private bool isVisible;
    private Vector2 scale;
    private Rectangle rectangle;
    private SpriteType type;
    private float depth;
    private Vector2 origin;
    private float rotation;

    public float Rotation
    {
      get { return rotation; }
      set { rotation = value; }
    }


    public Vector2 Origin
    {
      get { return origin; }
      set { origin = value; }
    }
	
    public float Depth
    {
      get { return depth; }
      set { depth = value; }
    }
	
    public SpriteType Type
    {
      get { return type; }
      set { type = value; }
    }
	
    public Rectangle Rectangle
    {
      get { return rectangle; }
      set { rectangle = value; }
    }
	
    public Vector2 Scale
    {
      get { return scale; }
      set { scale = value; }
    }
	
    public bool IsVisible
    {
      get { return isVisible; }
      set { isVisible = value; }
    }
	
    public Color Color
    {
      get { return color; }
      set { color = value; }
    }
	
	  public Vector2 Position
	  {
		  get { return position;}
		  set { position = value;}
	  }
	
    public Texture2D Texture
    {
      get { return texture; }
      set { texture = value; }
    }
    #endregion

    public Sprite(Texture2D texture, Vector2 position)
      : this(texture, position, Color.White)
    {}

    public Sprite(Texture2D texture, Vector2 position, Color color)
    {
      this.texture = texture;
      this.position = position;
      this.color = color;
      this.isVisible = true;
      this.scale = Vector2.One;
      this.rectangle = new Rectangle();
      this.type = SpriteType.NORMAL;
      this.origin = Vector2.Zero;
      this.depth = 0;
      this.rotation = 0;
    }
  }
}
