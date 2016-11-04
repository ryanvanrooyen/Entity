
using UnityEngine;

namespace Entity
{
	public class Sound3D : Sound, ISound
	{
		public Sound3D(AudioSource audioSource, AudioClip audioClip,
			float volume = 1f, float minDistance = 2f,
			float maxDistance = 100f, float seekTime = 0)
			: base(audioSource, audioClip, volume,
				minDistance, maxDistance, seekTime)
		{
		}

		protected override void SetSpatialMode()
		{
			this.audioSource.spatialBlend = 1f;
			this.audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
		}

		public void PlayAt(Vector3 point)
		{
			this.audioSource.transform.position = point;
			this.Play();
		}
	}
}
