using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Helpers;
using Tier.Source.Handlers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tier.Source.GameStates
{
  public class CreditsState : GameState
  {
    private class TextBlock
    {
      private List<Text> texts;
      private Vector2 startPosition;

      public TextBlock(Text header, Vector2 startPosition)
      {
        texts = new List<Text>();
        this.startPosition = startPosition;

        AddText(header);
      }

      public void AddText(Text t)
      {
        t.Position = this.startPosition + new Vector2(0, 40) * this.texts.Count;
        texts.Add(t);
      }

      public void Remove(TextSpriteHandler handler)
      {
        foreach (Text t in texts)
        {
          handler.RemoveText(t);
        }

        texts.Clear();
      }

      public void UpdatePosition(Vector2 delta)
      {
        foreach (Text t in texts)
        {
          t.Position += delta;
        }
      }
    }

    private List<TextBlock> textBlocks;

    public CreditsState(TierGame game)
      : base(game)
    {
      this.textBlocks = new List<TextBlock>();
    }

    public override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(this.game.MainControllerIndex).Buttons.Back == ButtonState.Pressed)
      {
        this.game.ChangeState(this.game.TitleScreenState);
        return;
      }

      foreach (TextBlock tb in textBlocks)
      {
        tb.UpdatePosition(new Vector2(0, -40) * (gameTime.ElapsedGameTime.Milliseconds / 1000.0f));
      }
    }

    public override void Enter(GameState previousState)
    {
      Vector2 startPosition = new Vector2(100, this.game.GraphicsDevice.PresentationParameters.BackBufferHeight);

      // Create textblocks
      TextBlock leadDesign = new TextBlock(
        this.game.TextSpriteHandler.CreateText("Design", Vector2.Zero, Color.Red),
        startPosition);
      leadDesign.AddText(this.game.TextSpriteHandler.CreateText(" Pieter Ted de Vries - Lead designer", Vector2.Zero, Color.Lime));

      TextBlock leadPro = new TextBlock(
        this.game.TextSpriteHandler.CreateText("Programming", Vector2.Zero, Color.Red),
        startPosition + new Vector2(0, 200));
      leadPro.AddText(this.game.TextSpriteHandler.CreateText(" Pieter Ted de Vries - Lead programmer", Vector2.Zero, Color.Lime));

      TextBlock leadArt = new TextBlock(
        this.game.TextSpriteHandler.CreateText("Art", Vector2.Zero, Color.Red),
        startPosition + new Vector2(0, 400));
      leadArt.AddText(this.game.TextSpriteHandler.CreateText(" Pieter Ted de Vries - Lead artist", Vector2.Zero, Color.Lime));
      leadArt.AddText(this.game.TextSpriteHandler.CreateText(" Henk van Alphen      - Artist", Vector2.Zero, Color.LimeGreen));

      TextBlock leadAudio = new TextBlock(
        this.game.TextSpriteHandler.CreateText("Audio", Vector2.Zero, Color.Red),
        startPosition + new Vector2(0, 620));
      leadAudio.AddText(this.game.TextSpriteHandler.CreateText(" Bram Stege - Engineer", Vector2.Zero, Color.Lime));

      TextBlock leadThanks = new TextBlock(
        this.game.TextSpriteHandler.CreateText("Special thanks to: ", Vector2.Zero, Color.Red),
        startPosition + new Vector2(0, 900));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText(" Alexander van der Kolk", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText(" Jonathan (Rambo) Rambelje", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText(" Maarten van den Heijkant", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText(" De Zwijndrecht Gang", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText(" Me moeder", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText(" De Goede", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText(" ", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText(" Ronimo Games: ", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText("  Fabian Akker", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText("  Gijs Hermans", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText("  Jasper Koning", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText("  Joost van Dongen", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText("  Martijn Thieme", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText("  Olivier Thijssen", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText("  Ralph Rademakers", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText(" ", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText(" The XNA team", Vector2.Zero, Color.Lime));
      leadThanks.AddText(this.game.TextSpriteHandler.CreateText(" World of Warcraft", Vector2.Zero, Color.Lime));  

      textBlocks.Add(leadDesign);
      textBlocks.Add(leadPro);
      textBlocks.Add(leadArt);
      textBlocks.Add(leadAudio);
      textBlocks.Add(leadThanks);
    }

    public override void Leave()
    {
      foreach (TextBlock tb in textBlocks)
      {
        tb.Remove(this.game.TextSpriteHandler);
      }

      textBlocks.Clear();
    }
  }
}
