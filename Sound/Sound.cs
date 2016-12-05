﻿
using UnityEngine;

namespace Entity
{
	public interface ISound
	{
		void Play(GameObject source);
	}

	public class Sound : SoundBase, ISound
	{
		private readonly float audibleRadius;

		public Sound(AudioSource audioSource, AudioClip audioClip,
			float audibleRadius, float volume = 1f, float minDistance = 2f,
			float maxDistance = 100f, float seekTime = 0)
			: base(audioSource, audioClip, volume,
				minDistance, maxDistance, seekTime)
		{
			this.audibleRadius = audibleRadius > 0 ? audibleRadius : 0;
		}

		protected override void SetSpatialMode()
		{
			this.audioSource.spatialBlend = 1f;
			this.audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
		}

		public void Play(GameObject source)
		{
			if (source == null)
				return;

			var point = source.transform.position;
			this.audioSource.transform.position = point;
			this.Play();

			if (this.audibleRadius > 0)
				TriggerSoundListeners(source);
		}

		private void TriggerSoundListeners(GameObject source)
		{
			var colliders = Physics.OverlapSphere(source.transform.position, this.audibleRadius);
			foreach (Collider hit in colliders)
			{
				var gameObj = hit.gameObject;
				if (gameObj == null)
					continue;
				
				var listener = gameObj.GetComponent<ISoundListener>();
				if (listener == null)
					continue;

				listener.OnHeard(this, source);
			}
		}

		public override string ToString()
		{
			return "Sound(" + this.audioClip.name + ")";
		}
	}
}
