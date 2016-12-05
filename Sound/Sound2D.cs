
using UnityEngine;

namespace Entity
{
	public interface ISound2d
	{
		void Play();
	}

	public class Sound2D : SoundBase, ISound2d
	{
		public Sound2D(AudioSource audioSource,
			AudioClip audioClip, float volume = 1f, float seekTime = 0)
			: base(audioSource, audioClip, volume, seekTime)
		{
		}

		protected override void SetSpatialMode()
		{
			this.audioSource.spatialBlend = 0;
		}
	}
}
