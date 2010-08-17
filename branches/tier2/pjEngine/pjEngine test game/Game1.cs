using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using pjEngine.Helpers;

namespace pjEngine_test_game
{
  public class GameObjectSegment : OcTreeSegment<GameObject>
  {
    public GameObjectSegment()
      : this(Vector3.Zero, Vector3.Zero)
    { }

    public GameObjectSegment(Vector3 min, Vector3 max)
      : base(min, max)
    { }

    public override ContainmentType IsInSegment(GameObject obj)
    {
      return this.BoundingBox.Contains(obj.BoundingBox);
    }

    public override IOcTreeSegment<GameObject> CreateInstance(Vector3 min, Vector3 max)
    {
      GameObjectSegment segment = new GameObjectSegment(min, max);

      return segment;
    }

    public override void Update(GameTime gameTime)
    {
      foreach (OcTreeSegment<GameObject> segment in this.segments)
      {
        segment.Update(gameTime);  
      }

      foreach (GameObject obj in this.Objects)
      {
        
      }
    }

    public override void Draw(GameTime gameTime)
    {
      foreach (OcTreeSegment<GameObject> segment in this.segments)
      {
        segment.Draw(gameTime);
      }

      foreach (GameObject obj in this.Objects)
      {

      }      
    }
  }

  public class GameObject : DrawableGameComponent
  {
    private BoundingBox bb;
    private VertexPositionColor[] vertices;
    private VertexDeclaration decl;
    private BasicEffect effect;

    public BoundingBox BoundingBox
    {
      get { return bb; }
      set { bb = value; }
    }

    public GameObject(Game game) : base(game)
    {
      vertices = new VertexPositionColor[3];
      decl = new VertexDeclaration(this.GraphicsDevice, VertexPositionColor.VertexElements);
      effect = new BasicEffect(this.GraphicsDevice, null);

      vertices[0] = new VertexPositionColor(new Vector3(-1, -1, 0), Color.Red);
      vertices[1] = new VertexPositionColor(new Vector3(1, -1, 0), Color.Green);
      vertices[2] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Blue);
    }

    public override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.VertexDeclaration = decl;

      effect.Begin();

      foreach (EffectPass pass in effect.CurrentTechnique.Passes)
      {
        pass.Begin();
       
        this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 1);

        pass.End();
      }

      effect.End();
    }
  }

  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Microsoft.Xna.Framework.Game
  {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    OcTree<GameObject, GameObjectSegment> objects;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      
      this.objects = new OcTree<GameObject, GameObjectSegment>(
        new Vector3(-50, -50, -50), new Vector3(50, 50, 50), 1, new GameObjectSegment());     
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();

      GameObject obj = new GameObject(this);
      obj.BoundingBox = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));

      this.objects.AddObject(obj);
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      // TODO: use this.Content to load your game content here
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      // TODO: Add your update logic here

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

      // TODO: Add your drawing code here

      base.Draw(gameTime);
    }
  }
}
