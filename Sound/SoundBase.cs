
using UnityEngine;
using System;

namespace Entity
{
	public abstract class SoundBase
	{
		protected readonly AudioSource audioSource;
		protected readonly AudioClip audioClip;
		private readonly float volume;
		private readonly float minDistance;
		private readonly float maxDistance;
        private readonly float seekTime;
        private readonly float pitch;

        public SoundBase(AudioSource audioSource,
            AudioClip audioClip, float volume = 1f, float minDistance = 2f,
            float maxDistance = 100f, float seekTime = 0, float pitch = 1f)
		{
			if (audioSource == null)
				throw new ArgumentNullException("audioSource");
			if (audioClip == null)
				throw new ArgumentNullException("audioClip");
			if (minDistance < 0f)
				throw new ArgumentException("minDistance should be greater than 0");
			if (maxDistance < 0f)
				throw new ArgumentException("maxDistance should be greater than 0");
            if (pitch < 0f)
                throw new ArgumentException("pitch should be greater than 0");
  
			this.audioClip = audioClip;
			this.audioSource = audioSource;
			this.volume = volume;
			this.minDistance = minDistance;
			this.maxDistance = maxDistance;
			this.seekTime = seekTime;
            this.pitch = pitch;
            
            if (this.volume > 1f)
                this.volume = 1f;
            else if (this.volume < 0)
                this.volume = 0;
		}

		protected abstract void SetSpatialMode();

		protected virtual void SetAudioSourceSettings()
		{
			this.audioSource.clip = this.audioClip;
			this.audioSource.time = this.seekTime;
            this.audioSource.pitch = this.pitch;
			this.audioSource.volume = volume;
			this.audioSource.minDistance = this.minDistance;
			this.audioSource.maxDistance = this.maxDistance;
		}

		public void Play()
		{
			SetSpatialMode();
			SetAudioSourceSettings();
			this.audioSource.Play();
		}
	}

	public class NoSound : ISound, ISound2d
	{
		public void Play() { }
		public void Play(GameObject source) { }
	}
}