using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Tier.Source.Handlers
{
  public class SoundHandler
  {
    private AudioEngine engine;
    private WaveBank wavebank;
    private SoundBank soundbank;
    private Hashtable musicPlaying;
    private Queue<Cue> cues;
    private TierGame game;

    public SoundHandler(TierGame game)
    {
#if WINDOWS
      engine = new AudioEngine("Content\\Audio\\Win\\Tier.xgs");
      wavebank = new WaveBank(engine, "Content\\Audio\\Win\\Wave Bank.xwb");
      soundbank = new SoundBank(engine, "Content\\Audio\\Win\\Sound Bank.xsb");
#else
      engine = new AudioEngine("Content\\Audio\\Xbox360\\Tier.xgs");
      wavebank = new WaveBank(engine, "Content\\Audio\\Xbox360\\Wave Bank.xwb");
      soundbank = new SoundBank(engine, "Content\\Audio\\Xbox360\\Sound Bank.xsb");
#endif

      this.game = game;
      musicPlaying = new Hashtable();
      cues = new Queue<Cue>();
    }

    public void Play(string name, bool isMusic)
    {
      if(!this.game.Options.IsSoundEnabled)
        return;
      
      Cue cue = null;

      if (isMusic && musicPlaying[name] == null)
      {
        cue = soundbank.GetCue(name);
        musicPlaying.Add(name, cue);
      }
      else if (!isMusic)
      {
        cue = soundbank.GetCue(name);
        cues.Enqueue(cue);
      }

      if (cue != null)
        cue.Play();
    }

    public void Play(string name)
    {
      Play(name, false);
    }

    public void Stop(string name)
    {
      if (musicPlaying[name] != null)
      {
        Cue c = (Cue)musicPlaying[name];
        c.Stop(AudioStopOptions.Immediate);
        c.Dispose();
        musicPlaying.Remove(name);
      }
    }

    public void Update(GameTime gameTime)
    {
      if (!this.game.Options.IsSoundEnabled)
        return;

      engine.Update();

      if (cues.Count > 0 && cues.Peek().IsStopped)
      {
        Cue c = cues.Dequeue();
        c.Dispose();
      }
    }
  }
}
