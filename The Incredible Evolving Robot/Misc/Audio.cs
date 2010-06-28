#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Tier.Objects;
#endregion

namespace Tier.Misc
{
	/// <summary>An Audio manager for BGM, SFX and playlists</summary>
	public class Audio
	{
		#region Properties
		private AudioEngine audioEngine;
		private WaveBank waveBank;
		private SoundBank soundBank;

		private Cue bgm;
		private bool isbgmloop = false;
		private String bgmCueName = "";

		private String[] playList;
		private Boolean playListIsPlaying = false;
		private int playListPoint = 0;
		private Cue playListCurrent;

		private Hashtable sfx3DList = new Hashtable();
		private Hashtable sfx3DListEmitterObj = new Hashtable();
		private AudioListener listener = new AudioListener();
		private AudioEmitter emitter = new AudioEmitter();
		private List<String> sfx3DListRemove = new List<String>();

		public String[] PlayList
		{
			get { return playList; }
			set
			{
				if (playListIsPlaying)
					throw new Exception("Do not change playlist while playing");

				playListPoint = 0; //Reset the play pointer to start
				playList = value;
			}
		}
		#endregion

		/// <summary>
		/// Creates an Audio class using the data of XACT
		/// Standard location C:\Program Files\Microsoft XNA\XNA Game Studio Express\v1.0\Tools\Xact.exe
		/// so search for ?\Microsoft XNA\XNA Game Studio Express\v1.0\Tools\Xact.exe
		/// Using version 2.0 NOT v1.0 as the folder says
		/// Call Dispose as deconstructor!
		/// </summary>
		/// <param name="audioEngineFile">file to xap</param>
		/// <param name="waveBankFile">file to xwb</param>
		/// <param name="soundBankFile">file to xsb</param>
		public Audio(String audioEngineFile, String waveBankFile, String soundBankFile)
		{
			this.audioEngine = new AudioEngine(audioEngineFile);
			this.waveBank = new WaveBank(audioEngine, waveBankFile);
			this.soundBank = new SoundBank(audioEngine, soundBankFile);
		}

		public void Dispose()
		{
			this.audioEngine.Dispose();
			this.waveBank.Dispose();
			this.soundBank.Dispose();

			this.sfx3DList.Clear();
			this.sfx3DListEmitterObj.Clear();
			this.sfx3DListRemove.Clear();
			this.sfx3DList = null;
			this.sfx3DListEmitterObj = null;
			this.listener = null;
			this.emitter = null;
			this.sfx3DListRemove = null;
		}

		/// <summary>Sets the volume of all audio associated within category parameter category</summary>
		/// <param name="category">Category name set in Xact.exe tool</param>
		/// <param name="volume">Volume 0 as mute, 1.0 as max</param>
		public void setVolume(String category, float volume)
		{
			this.audioEngine.GetCategory(category).SetVolume(volume);
		}

		/// <summary>Sets the volume of all audio associated with this category bgm</summary>
		/// <param name="volume">Volume 0 as mute, 1.0 as max</param>
		public void setVolumeBGM(float volume)
		{
			this.setVolume("BGM", volume);
		}

		/// <summary>Sets the volume of all audio associated with this category sfx</summary>
		/// <param name="volume">Volume 0 as mute, 1.0 as max</param>
		public void setVolumeSFX(float volume)
		{
			this.setVolume("SFX", volume);
		}

		#region playlist

		/// <summary>Set playlist (while not playing) and this methode will play the playlist repeatley</summary>
		public void playlistPlay()
		{
			if (playList.Length > 0)
			{
				playListPoint = 0;
				playListCurrent = soundBank.GetCue(playList[playListPoint]);
				playListCurrent.Play();
				playListIsPlaying = true;
			}
		}

		public void playlistPausePlay()
		{
			if (playListIsPlaying)
				if (playListCurrent.IsPlaying)
					playListCurrent.Pause();
				else if (playListCurrent.IsPaused)
					playListCurrent.Play();
		}

		public void playlistPause()
		{
			if (playListIsPlaying)
				playListCurrent.Pause();
		}

		public void playlistResume()
		{
			if (playListIsPlaying)
				playListCurrent.Resume();
		}


		public void playlistStop(AudioStopOptions audioStopOptions)
		{
			if (playList.Length > 0 && playListIsPlaying)
			{
				playListCurrent.Stop(audioStopOptions);
				playListIsPlaying = false;
				playListCurrent = null;
			}
		}

		public void playlistPlayNext()
		{
			if (playListIsPlaying)
			{
				playListPoint++;
				if (playListPoint == playList.Length)
					playListPoint = 0;

				playListCurrent = soundBank.GetCue(playList[playListPoint]);
				playListCurrent.Play();
			}
		}

		public void playlistPlayPrevious()
		{
			if (playListIsPlaying)
			{
				playListPoint--;
				if (playListPoint == 0)
					playListPoint = playList.Length - 1;

				playListCurrent = soundBank.GetCue(playList[playListPoint]);
				playListCurrent.Play();
			}
		}
		#endregion

		/// <summary>SFX can't be stopped, paused or looped</summary>
		/// <param name="cueName"></param>
		public void playSFX(String cueName)
		{
			this.soundBank.PlayCue(cueName);
		}

		#region Audio 3d
		public void set3DListener(BasicObject listener_)
		{
			listener.Position = listener_.Position.Coordinate;
		}

		/// <summary>Plays an SFX sound applying the 3D effect</summary>
		/// <param name="cueName">cuename of the sound you want to play</param>
		/// <param name="emitter_">the object who emits the sound</param>
		/// <param name="allowXSounds">allow only a number of X sounds simulatious play</param>
		public void playSFX3D(String cueName, BasicObject emitter_, int allowXSounds)
		{
			String cueKeyName = cueName;
			int number = 1;
			if (allowXSounds < 1)
				allowXSounds = 1;
			while (sfx3DListEmitterObj.ContainsKey(cueKeyName))
			{
				cueKeyName = cueName + number;
				number++;
				if (number > allowXSounds)
					return;
			}
			sfx3DListEmitterObj.Add(cueKeyName, emitter_);
			sfx3DList.Add(cueKeyName, soundBank.GetCue(cueName));
			emitter.Position = emitter_.Position.Coordinate;
			((Cue)sfx3DList[cueKeyName]).Apply3D(listener, emitter);
			((Cue)sfx3DList[cueKeyName]).Play();
		}

		/// <summary>Plays an SFX sound applying the 3D effect</summary>
		/// <param name="cueName">cuename of the sound you want to play</param>
		/// <param name="emitter_">the object who emits the sound</param>
		public void playSFX3D(String cueName, BasicObject emitter_)
		{
			playSFX3D(cueName, emitter_, 999);
			/*
			String cueKeyName = cueName;
			int number = 1;
			while (sfx3DListEmitterObj.ContainsKey(cueKeyName))
			{
				number++;
				cueKeyName = cueName + number;
			}
			sfx3DListEmitterObj.Add(cueKeyName, emitter_);
			sfx3DList.Add(cueKeyName, soundBank.GetCue(cueName));
			emitter.Position = emitter_.Position.Coordinate;
			((Cue)sfx3DList[cueKeyName]).Apply3D(listener, emitter);
			((Cue)sfx3DList[cueKeyName]).Play();
			 * */
		}
		#endregion

		#region BGM
		public void playBGM(String cueName, bool loop)
		{
			this.isbgmloop = loop;
			this.bgmCueName = cueName;
			if (bgm != null)
				if (bgm.IsPlaying || bgm.IsPaused)
					bgm.Stop(AudioStopOptions.Immediate);
			bgm = soundBank.GetCue(bgmCueName);
			bgm.Play();
		}

		public void pauseBGM()
		{
			bgm.Pause();
		}

		public void pausePlayBGM()
		{
			if (bgm.IsPlaying)
				bgm.Pause();
			else if (bgm.IsPaused)
				bgm.Play();
		}

		public void resumeBGM()
		{
			bgm.Resume();
		}

		public void stopBGM(AudioStopOptions audioStopOptions)
		{
			bgm.Stop(audioStopOptions);
		}
		#endregion

		/// <summary>Performs periodic work required by the audio engine.</summary>
		public void Update()
		{
			audioEngine.Update();

			//Do 3D audio update
			sfx3DListRemove.Clear();
			foreach (DictionaryEntry e in sfx3DList)
			{
				if (!((Cue)e.Value).IsPlaying)
				{
					sfx3DListEmitterObj.Remove(e.Key);
					sfx3DListRemove.Add((String)e.Key);
					continue;
				}
				emitter.Position = ((BasicObject)sfx3DListEmitterObj[e.Key]).Position.Coordinate;
				((Cue)e.Value).Apply3D(listener, emitter);
			}

			foreach (String key in sfx3DListRemove)
				sfx3DList.Remove(key);

			//Do playlist control
			if (playListIsPlaying)
				if (!playListCurrent.IsPlaying)
					playlistPlayNext();
			//

			//Do bgm loop control
			if (bgm != null)
				if (isbgmloop && !bgm.IsPlaying)
					playBGM(bgmCueName, isbgmloop);
		}
	}
}
