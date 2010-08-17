using System;
using System.Collections.Generic;
using System.Text;
using Tier.Source.Objects.Turrets;
using Microsoft.Xna.Framework;
using Tier.Source.Helpers.Cameras;
using Microsoft.Xna.Framework.Graphics;
using Tier.Source.Objects;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Tier.Source.GameStates
{
  public class GameTestState : GameState
  {
    float timeElapsed;
    Camera cam;
    BossPiece piece;

    public GameTestState(TierGame game)
      : base(game)
    {
      cam = new PositionalCamera(game);
    }

    public override void Enter(GameState previousState)
    {
      cam.Position = new Vector3(0, 0, 20);
      cam.Target = Vector3.Zero;
      this.game.GameHandler.Camera = cam;

      piece = new BossPiece(this.game);
      if (this.game.ObjectHandler.InitializeFromBlueprint<BossPiece>(piece, "Bar"))
      {
        this.game.GameHandler.AddObject(piece);
      }
    }
 
    public override void Leave()
    {
      
    }

    public override void Update(GameTime gameTime)
    {
      timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

      piece.Rotation =
        Quaternion.CreateFromAxisAngle(Vector3.Left, timeElapsed / 4000.0f * (MathHelper.Pi * 2)) *
        Quaternion.CreateFromAxisAngle(Vector3.Up, timeElapsed / 2000.0f * (MathHelper.Pi * 2));

      this.game.GameHandler.Update(gameTime);
    }
  }
}
