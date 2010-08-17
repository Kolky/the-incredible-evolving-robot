using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Tier.Source.ObjectModifiers;
using Tier.Source.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Objects;

namespace Tier.Source.Handlers
{
  public enum ExplosionHandlerState
  {
    IDLE, COMBO_ON_SCREEN, MOVING_TO_SCORE, CHECKING_IF_DONE, DONE
  };

  public class ExplosionHandler : IDisposable
  {
    #region Properties
    private DestroyableModifier parent;
    private bool isRunning;
    private bool isDone;

    public bool IsDone
    {
      get { return isDone; }
      set { isDone = value; }
    }
	
    public bool IsRunning
    {
      get { return isRunning; }
      set { isRunning = value; }
    }
	
    public DestroyableModifier Parent
    {
      get { return parent; }
      set { parent = value; }
    }
    #endregion

    private float timeElapsed = 0, timeToShow = 0;
    private int count = 0;
    private Text text;
    private ExplosionHandlerState state;
    private Vector2 movement;

    public ExplosionHandler(DestroyableModifier destroyableModifier)
    {
      this.IsRunning = this.isDone = false;
      this.parent = destroyableModifier;
      this.state = ExplosionHandlerState.IDLE;
      this.movement = Vector2.Zero;

      this.text =
        destroyableModifier.Parent.Game.TextSpriteHandler.CreateText(
          "",
          Unproject(destroyableModifier.Parent.Game, destroyableModifier.Parent),
          Color.White);
    }

    private Vector2 Unproject(TierGame game, GameObject obj)
    {
      Vector3 v = game.GraphicsDevice.Viewport.Project(
        obj.Position,
        game.GameHandler.Projection,
        game.GameHandler.View,
        game.GameHandler.World);

      return new Vector2(v.X, v.Y);
    }

    public void Start()
    {
      if (!isRunning)
      {        
        this.isRunning = true;
        this.timeElapsed = 0;
      }
    }

    public void Update(GameTime gameTime)
    {
      timeElapsed += gameTime.ElapsedGameTime.Milliseconds;      

      switch (this.state)
      {
        case ExplosionHandlerState.COMBO_ON_SCREEN:
          if (this.timeElapsed >= this.timeToShow)
          {
            changeState(ExplosionHandlerState.MOVING_TO_SCORE); 
          }
          break;
        case ExplosionHandlerState.MOVING_TO_SCORE:
          if(this.timeElapsed >= 1500.0f)
          {
            changeState(ExplosionHandlerState.DONE);            
          }
          // Move to position of score
          this.text.Position += movement * (gameTime.ElapsedGameTime.Milliseconds / 1500.0f);
          break;
        case ExplosionHandlerState.CHECKING_IF_DONE:
          break;
        case ExplosionHandlerState.DONE:
          if (this.timeElapsed >= 500.0f)
          {
            this.IsRunning = false;
            this.isDone = true;
            this.Parent.Parent.Game.GameHandler.RemoveExplosionHandler(this);
          }
          break;
      }
    }

    private void changeState(ExplosionHandlerState newstate)
    {
      switch (newstate)
      {
        case ExplosionHandlerState.IDLE:
          break;
        case ExplosionHandlerState.COMBO_ON_SCREEN:
          break;
        case ExplosionHandlerState.MOVING_TO_SCORE:
          // Determine the speed and direction of the score
          VisualScore score = new VisualScore(this.text.Value, 
            this.text.Position, this.text.Color, this.Parent.Parent.Game);
          this.Parent.Parent.Game.ScoreHandler.AddVisualScore(score);
          this.Parent.Parent.Game.TextSpriteHandler.RemoveText(this.text);
          break;
        case ExplosionHandlerState.CHECKING_IF_DONE:
          break;
        case ExplosionHandlerState.DONE:
          this.text.IsVisible = false;
          this.timeElapsed = 0;

          TierGame g = this.parent.Parent.Game;
          // Update score
          if (count > 1)
            g.ScoreHandler.AddScore(this.count * g.Options.ScoreComboBlockDestroyed);
          else
            g.ScoreHandler.AddScore(g.Options.ScoreSingleBlockDestroyed);
          break;
      }

      this.state = newstate;
    }

    /// <summary>
    /// Checks all attached objects is they are destroyed.
    /// </summary>
    /// <returns></returns>
    private bool CheckIsIfDestroyed(GameObject parent)
    {
      bool value = true;

      foreach (Connector conn in parent.AttachableModifier.Connectors)
      {
        if (conn.ConnectedTo != null &&
          conn.ConnectedTo != parent)
        {
          if (!conn.ConnectedTo.DestroyableModifier.IsDestroyed)
          {
            return false;
          }

          value = CheckIsIfDestroyed(conn.ConnectedTo);
        }
      }

      return value;
    }

    public void ShowCombo(float timeToShow)
    {
      this.state = ExplosionHandlerState.COMBO_ON_SCREEN;
      this.timeElapsed = 0;
      this.timeToShow = timeToShow;
      this.text.IsVisible = false;
      count++;
      this.text.IsVisible = true;

      if (count >= 2)
      {
        this.text.Value = string.Format("{0}x! {1}", count, 
          count * Parent.Parent.Game.Options.ScoreComboBlockDestroyed);        
      }
      else
        this.text.Value = string.Format("{0}",
            count * Parent.Parent.Game.Options.ScoreSingleBlockDestroyed);
    }

    #region IDisposable Members

    public void Dispose()
    {
      this.text.Dispose();
    }

    #endregion
  }
}
