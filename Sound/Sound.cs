
using UnityEngine;
using System;

namespace Entity
{
	public interface ISound
	{
		void PlayAt(Vector3 point);
	}

	public abstract class Sound
	{
		protected readonly AudioSource audioSource;
		private readonly AudioClip audioClip;
		private readonly float volume;
		private readonly float minDistance;
		private readonly float maxDistance;
		private readonly float seekTime;

		public Sound(AudioSource audioSource,
			AudioClip audioClip, float volume = 1f, float minDistance = 2f,
			float maxDistance = 100f, float seekTime = 0)
		{
			if (audioSource == null)
				throw new ArgumentNullException("audioSource");
			if (audioClip == null)
				throw new ArgumentNullException("audioClip");
			if (volume < 0f || volume > 1f)
				throw new ArgumentException("volumn should be between 0-1");
			if (minDistance < 0f)
				throw new ArgumentException("minDistance should be greater than 0");
			if (maxDistance < 0f)
				throw new ArgumentException("maxDistance should be greater than 0");

			this.audioClip = audioClip;
			this.audioSource = audioSource;
			this.volume = volume;
			this.minDistance = minDistance;
			this.maxDistance = maxDistance;
			this.seekTime = seekTime;
		}

		protected abstract void SetSpatialMode();

		protected virtual void SetAudioSourceSettings()
		{
			this.audioSource.clip = this.audioClip;
			this.audioSource.time = this.seekTime;
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
		public void PlayAt(Vector3 point) { }
	}
}