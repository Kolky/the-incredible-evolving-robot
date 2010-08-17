using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Objects;
using Tier.Source.Helpers.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Helpers;
using Tier.Helpers;
using Tier.Source.Objects.Projectiles;
using Tier.Source.ObjectModifiers;

namespace Tier.Source.GameStates
{
  public enum ModifierTest
  {
    Collision, Attachable
  };

  /// <summary>
  /// Test state designed to test the modifiers assigned to all objects.
  /// Things like visualization of bounding volumes (CollisionModifier) and connection points (AttachableModifier)
  /// </summary>
  class ModifierTestState : GameState
  {
    #region Properties
    private List<GameObject> objects;
    private RotatingCamera cam;
    private GameObject currentObject;
    private KeyboardState previousKb;
    private int currentIndex;
    private ModifierTest currentTest;
    private VertexPositionColor[] vertices;
    private BasicEffect effect;
    private Axis axis;
    private bool axisVisible;
    private Model cubeModel;
    #endregion

    public ModifierTestState(TierGame game)
      : base(game)
    {
      this.objects = new List<GameObject>();
      this.cam = new RotatingCamera(this.game);
      this.axis = new Axis(this.game);      
    }

    public override void Enter(GameState previousState)
    {
      base.Enter(previousState);

      this.axis.Init();
      this.axisVisible = true;
      this.cubeModel = this.game.ContentHandler.GetAsset<Model>("cube");

      this.effect = new BasicEffect(this.Game.GraphicsDevice, null);
      this.currentTest = ModifierTest.Collision;

      this.game.ObjectHandler.ClearObjects();
      this.game.ObjectHandler.LoadObjects("Content//Xml//Objects//BossPieces//Tedris");

      foreach (string key in this.game.ObjectHandler.Objects.Keys)
      {
        BossPiece piece = new BossPiece(this.game);
        this.game.ObjectHandler.InitializeFromBlueprint<BossPiece>(piece, key);
        piece.Initialize();
        this.objects.Add(piece);
      }

      if (this.objects.Count > 0)
      {
        this.currentIndex = 0;
        this.currentObject = this.objects[0];

        this.cam.StartRotation(this.currentObject, 10000, MathHelper.Pi * 2);
        this.game.GameHandler.Camera = this.cam;
      }
    }

    private void LoadObjectTypes<T>() where T : GameObject
    {
      foreach (String name in this.game.ObjectHandler.Objects.Keys)
      {
        if (this.game.ObjectHandler.Objects[name].GetType() == typeof(T))
        {
          Projectile piece = new Projectile(this.game);

          this.game.ObjectHandler.InitializeFromBlueprint<Projectile>(piece, name);
          
          this.objects.Add(piece);
        }
      }
    }

    public override void Leave()
    {
      foreach (GameObject obj in this.objects)
      {
        obj.Dispose();
      }

      this.objects.Clear();
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      if (this.objects.Count == 0)
        return;

      this.game.ContentHandler.GetAsset<Effect>("Default").Parameters["matView"].SetValue(
        this.game.GameHandler.View);
      this.game.ContentHandler.GetAsset<Effect>("Default").Parameters["matProj"].SetValue(
        this.game.GameHandler.Projection);

      cam.Update(gameTime);

      KeyboardState kbstate = Keyboard.GetState();

      foreach (Keys key in kbstate.GetPressedKeys())
	    {
        switch (key)
        {
          case Keys.PageDown:
            this.cam.Distance -= 1.0f;
            break;
          case Keys.PageUp:
            this.cam.Distance += 1.0f;
            break;
        }    		 
	    }

      // AttachableModifier
      if (kbstate.IsKeyDown(Keys.D1) && previousKb.IsKeyUp(Keys.D1))
      {
        this.currentTest = ModifierTest.Attachable;
      }

      // CollisionModifier
      if (kbstate.IsKeyDown(Keys.D2) && previousKb.IsKeyUp(Keys.D2))
      {
        this.currentTest = ModifierTest.Collision;
      }

      if (kbstate.IsKeyDown(Keys.A) && previousKb.IsKeyUp(Keys.A))
        this.axisVisible = !this.axisVisible;

      if (kbstate.IsKeyDown(Keys.Up) && previousKb.IsKeyUp(Keys.Up))
      {
        if(++this.currentIndex < this.objects.Count)
          this.currentObject = this.objects[this.currentIndex];
        else
          this.currentIndex = this.objects.Count -1;
      }

      if (kbstate.IsKeyDown(Keys.Down) && previousKb.IsKeyUp(Keys.Down))
      {
        if (--this.currentIndex >= 0)
          this.currentObject = this.objects[this.currentIndex];
        else
          this.currentIndex = 0;
      }

      previousKb = kbstate;
    }
  }
}