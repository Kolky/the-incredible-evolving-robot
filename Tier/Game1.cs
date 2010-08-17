using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.GameStates;
using Tier.Source.GameStates.Tutorial;
using pjEngine.Content;
using Tier.Source.Handlers;
using Tier.Source.Objects;
using Tier.Source.Helpers.Renderers;
using Tier.Source.Misc;
using BloomPostprocess;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace Tier
{
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class TierGame : Game
  {
    #region Properties
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private GameState gamestate;
    private ContentHandler content;
    private BossGrowthHandler growthHandler;
    private BossGrowthPatternHandler growthPatternHandler;    
    private ObjectHandler objectHandler;
    private BossBehaviourHandler behaviourHandler;
    private VertexDeclaration billboardVertexDeclaration;
    private VertexPositionTexture[] billBoardVertices;
    private MainGameState mainGameState;
    private BossIntroductionState bossIntroductionState; 
    private BossPieceTemplateHandler bossPieceTemplateHandler;
    private Options options;
    private TextSpriteHandler textSpriteHandler;
    private ScoreHandler scoreHandler;
    private float speed;
    private ResolveTargetHandler resolveTargetHandler;
    private SoundHandler soundHandler;
    private TitleScreenState titleScreenState;
    private ControlsState controlsState;
    private CreditsState creditsState;
    private Renderer renderer;    
    private BloomComponent bloom;
    private Random random;
    private BossCompositionHandler bossCompositionHandler;
    private ProjectileHandler projectileHandler;
    private InterfaceHandler interfaceHandler;
    private StatisticsHandler statisticsHandler;
    private PlayerIndex mainControllerIndex;

    public PlayerIndex MainControllerIndex
    {
      get { return mainControllerIndex; }
      set { mainControllerIndex = value; }
    }

    public StatisticsHandler StatisticsHandler
    {
      get { return statisticsHandler; }
    }

    public InterfaceHandler InterfaceHandler
    {
      get { return interfaceHandler; }
    }

    public ProjectileHandler ProjectileHandler
    {
      get { return projectileHandler; }
      set { projectileHandler = value; }
    }

    public Renderer Renderer 
    { 
      get { return this.renderer; } 
    }
    
    public BossCompositionHandler BossCompositionHandler 
    { 
      get { return bossCompositionHandler;}
      set { bossCompositionHandler = value;} 
    }

    public Random Random 
    {
      get { return random; }
    }
    
    public BloomComponent BloomComponent
    {
      get { return bloom; }
      set { bloom = value; }
    }
	
    public GraphicsDeviceManager GraphicsDeviceManager
    {
      get { return graphics; }
    }

    public CreditsState CreditsState
    {
      get { return creditsState; }
      set { creditsState = value; }
    }
    public GameState GameState
    {
      get { return gamestate; }
    }
    
    public ControlsState ControlsState
    {
      get { return controlsState; }
      set { controlsState = value; }
    }
	
    public TitleScreenState TitleScreenState
    {
      get { return titleScreenState; }
      set { titleScreenState = value; }
    }
	
    public SoundHandler SoundHandler
    {
      get { return soundHandler; }
      set { soundHandler = value; }
    }
	
    public ResolveTargetHandler ResolveTargetHandler
    {
      get { return resolveTargetHandler; }
      set { resolveTargetHandler = value; }
    }
	
    /// <summary>
    /// Gamespeed should be zero at any time, change to create slowmotion/speed up effects.
    /// </summary>
    public float Speed
    {
      get { return speed; }
      set { speed = value; }
    }
	
    public ScoreHandler ScoreHandler
    {
      get { return scoreHandler; }
      set { scoreHandler = value; }
    }
	
    public TextSpriteHandler TextSpriteHandler
    {
      get { return textSpriteHandler; }
      set { textSpriteHandler = value; }
    }
	
    public Options Options
    {
      get { return options; }
      set { options = value; }
    }
	
    public BossPieceTemplateHandler BossPieceTemplateHandler
    {
      get { return bossPieceTemplateHandler; }
      set { bossPieceTemplateHandler = value; }
    }

    public BossIntroductionState BossIntroductionState
    {
      get { return bossIntroductionState; }
      set { bossIntroductionState = value; }
    }
	
    public MainGameState MainGameState
    {
      get { return mainGameState; }
      set { mainGameState = value; }
    }
	
    public VertexPositionTexture[] BillboardVertices
    {
      get { return billBoardVertices; }
      set { billBoardVertices = value; }
    }
	

    public VertexDeclaration BillboardVertexDeclaration
    {
      get { return billboardVertexDeclaration; }
      set { billboardVertexDeclaration = value; }
    }
	
    public BossBehaviourHandler BehaviourHandler
    {
      get { return behaviourHandler; }
      set { behaviourHandler = value; }
    }
	
    public ObjectHandler ObjectHandler
    {
      get { return objectHandler; }
      set { objectHandler = value; }
    }
	
    public BossGrowthHandler BossGrowthHandler
    {
      get { return growthHandler; }
      set { growthHandler = value; }
    }
	
    public BossGrowthPatternHandler BossGrowthPatternHandler
    {
      get { return growthPatternHandler; }
      set { growthPatternHandler = value; }
    }
	
    private GameHandler gameHandler;

    public GameHandler GameHandler
    {
      get { return gameHandler; }
      set { gameHandler = value; }
    }
	
    public ContentHandler ContentHandler
    {
      get { return content; }
      set { content = value; }
    }
    #endregion

    private bool isPauzed;
    private GamePadState previousGamepadState;
    private KeyboardState previousKeyboardState;
    private ResolveTexture2D texScreen;
    public float timeElapsedSinceUpdate;

    public TierGame()
    {
      Content.RootDirectory = "Content";
      
      int test = (int)DateTime.Now.Ticks;
      this.random = new Random(test);

      this.options = new Options();
      this.content = new ContentHandler(this);
      this.objectHandler = new ObjectHandler(this);      
      this.graphics = new GraphicsDeviceManager(this);
      this.gameHandler = new GameHandler(this);
      this.gameHandler.Player = new Player(this);
      this.renderer = new DefaultRenderer(this);
      this.textSpriteHandler = new TextSpriteHandler(this);   
      this.behaviourHandler = new BossBehaviourHandler(this);
      this.bossPieceTemplateHandler = new BossPieceTemplateHandler(this);
      this.scoreHandler = new ScoreHandler(this);
      this.projectileHandler = new ProjectileHandler(this);
      this.statisticsHandler = new StatisticsHandler(this);
      this.speed = 1.0f;

      this.bossCompositionHandler = new BossCompositionHandler(this);
      this.soundHandler = new SoundHandler(this);
      this.renderer.Initialize(1280, 720);
    }

    public void ChangeState(GameState newstate)
    {
      if(this.gamestate != null)
        this.gamestate.Leave();
      
      newstate.Enter(this.gamestate);
      this.gamestate = newstate;
    }

    private void CreateBillboardVertices()
    {
      this.BillboardVertices = new VertexPositionTexture[6];
      this.BillboardVertices[0] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 0));
      this.BillboardVertices[1] = new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 0));
      this.BillboardVertices[2] = new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 1));
      this.BillboardVertices[3] = new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 1));
      this.BillboardVertices[4] = new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 1));
      this.BillboardVertices[5] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 0));

      this.BillboardVertexDeclaration = new VertexDeclaration(this.GraphicsDevice, VertexPositionTexture.VertexElements);      
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
      base.Initialize();

      if (this.bloom != null)
      {
        this.bloom.Initialize();
        this.BloomComponent.Settings = BloomSettings.PresetSettings[3];
      }

      ChangeState(new LoadContentGameState(this));
      //ChangeState(this.titleScreenState);
      //ChangeState(new TutorialStartState(this));
    }

    public void InitializeAfterContentLoaded()
    {
      this.projectileHandler.Initialize();      
      this.interfaceHandler = new InterfaceHandler(this);
      CreateBillboardVertices();

      // Create game states
      this.mainGameState = new MainGameState(this);
      this.bossIntroductionState = new BossIntroductionState(this);
      this.titleScreenState = new TitleScreenState(this);
      this.controlsState = new ControlsState(this);
      this.creditsState = new CreditsState(this);

      this.gameHandler.Projection = Matrix.CreatePerspectiveFieldOfView(
        MathHelper.PiOver4, 1280.0f / 720.0f, 0.1f, 1000.0f);

      // Initialize player
      this.objectHandler.InitializeFromBlueprint<Player>(this.gameHandler.Player, "Player");
    }

    public void TogglePauzed()
    {
      this.isPauzed = !this.isPauzed;
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
      this.content.UnLoad("Default");
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      this.timeElapsedSinceUpdate = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

      /*
      if (this.gamestate.GetType() == typeof(MainGameState))
      {
        GamePadState currentGamePadState = GamePad.GetState(this.game.MainControllerIndex);

        if (previousGamepadState.Buttons.Start == ButtonState.Released &&
          currentGamePadState.Buttons.Start == ButtonState.Pressed)
        {
          this.TogglePauzed();
        }

        previousGamepadState = currentGamePadState;
      }
      */
#if WINDOWS
      if (previousKeyboardState.IsKeyUp(Keys.PrintScreen) &&
        Keyboard.GetState().IsKeyDown(Keys.PrintScreen))
      {
        string name = String.Format("Tier{0}{1}{2}.jpg",
          DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        if (texScreen == null)
        {
          texScreen = new ResolveTexture2D(
            this.GraphicsDevice,
            this.GraphicsDevice.PresentationParameters.BackBufferWidth,
            this.GraphicsDevice.PresentationParameters.BackBufferHeight,
            1,
            this.GraphicsDevice.PresentationParameters.BackBufferFormat);
        }
        this.graphics.GraphicsDevice.ResolveBackBuffer(texScreen);
        texScreen.Save(name, ImageFileFormat.Jpg);
      }
      previousKeyboardState = Keyboard.GetState();
#endif

      if (!this.isPauzed)
      {
        this.soundHandler.Update(gameTime);

        TimeSpan tElapsedGameTime = new TimeSpan(gameTime.ElapsedGameTime.Days, gameTime.ElapsedGameTime.Hours,
          gameTime.ElapsedGameTime.Minutes, gameTime.ElapsedGameTime.Seconds, (int)(gameTime.ElapsedGameTime.Milliseconds * this.speed));
        TimeSpan tElapsedRealTime = new TimeSpan(gameTime.ElapsedRealTime.Days, gameTime.ElapsedRealTime.Hours,
          gameTime.ElapsedRealTime.Minutes, gameTime.ElapsedRealTime.Seconds, (int)(gameTime.ElapsedRealTime.Milliseconds * this.speed));
        GameTime t = new GameTime(gameTime.TotalRealTime, tElapsedRealTime, gameTime.TotalGameTime, tElapsedGameTime);

        this.gamestate.Update(t);
        if(this.interfaceHandler != null)
          this.InterfaceHandler.Update(gameTime);
        base.Update(t);
      }
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

      this.renderer.Draw(gameTime);      
    }
  }
}