using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tier.Source.ObjectModifiers;

namespace Tier.Source.Objects.ObjectModifiers
{
  public class FadingModifier : ObjectModifier
  {
    private float timeToFadeIn, timeElapsed;
    private float fadeAmount;

    public float FadeAmount
    {
      get { return fadeAmount; }
    }

    public FadingModifier(GameObject obj)
      : this(obj, 1000)
    { }

    public FadingModifier(GameObject obj, int timeToFadeIn)
      : base(obj)
    {
      this.timeToFadeIn = timeToFadeIn;
      fadeAmount = 0;
    }

    public void IncreaseTimeElapsed(int value)
    {
      timeElapsed += value;
    }

    public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      if(timeElapsed > timeToFadeIn)
      {
        timeElapsed = timeToFadeIn;
      }

      fadeAmount = timeElapsed / timeToFadeIn;
    }

    public override void Clone(out ObjectModifier objmod, GameObject parent)
    {
      objmod = null;
    }
  }
}
