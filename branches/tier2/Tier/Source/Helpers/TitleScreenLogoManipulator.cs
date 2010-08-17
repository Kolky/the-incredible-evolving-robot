using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Tier.Source.Helpers
{
  public class TitleScreenLogoManipulator
  {
    #region Properties
    private float timeElapsed;
    private bool isRunning;
    private LogoManipulator manipulator;
    private Hashtable logoManipulationTimes;
    #endregion

    private class LogoManipulator
    {
      private Sprite logo;
      private float timeElapsed;

      public LogoManipulator(Sprite logo)
      {
        this.logo = logo;
        this.timeElapsed = float.MaxValue;
      }

      public void Start()
      {
        this.timeElapsed = 0;
        
      }

      public void Update(GameTime gameTime)
      {
        if (timeElapsed >= 500)
        {
          return;
        }

        Rectangle r = this.logo.Rectangle;
        this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

        if (timeElapsed < 250)
        {
          r.Inflate(4, 4);
        }
        else if (timeElapsed < 500)
        {
          r.Inflate(-4, -4);
        }

        this.logo.Rectangle = r;
      }
    };

    public TitleScreenLogoManipulator(Sprite logo)
    {
      this.timeElapsed = 0;      
      this.logoManipulationTimes = new Hashtable();
      this.manipulator = new LogoManipulator(logo);
    }

    public void Start()
    {
      this.timeElapsed = 0;
      this.isRunning = true;
    }

    public void Stop()
    {
      this.isRunning = false;
    }

    public void AddTime(int milliseconds)
    {
      logoManipulationTimes.Add(milliseconds, false);
    }

    public void Clear()
    {
      // Resets all timers
      this.logoManipulationTimes.Clear();  			      
    }

    public void Update(GameTime gameTime)
    {
      if (!isRunning)
        return;

      this.manipulator.Update(gameTime);

      int totalTime =
        gameTime.TotalGameTime.Minutes * 60000 +
        gameTime.TotalGameTime.Seconds * 1000 +
        gameTime.TotalGameTime.Milliseconds;

      foreach (int millis in logoManipulationTimes.Keys)
      {
        if (totalTime >= millis &&
          (bool)logoManipulationTimes[millis] == false)
        {
          logoManipulationTimes[millis] = true;
          this.manipulator.Start();
          break;
        }
      }
      
      this.timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
    }
  }
}
