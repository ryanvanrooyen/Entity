
using System;
using UnityEngine;

namespace Entity
{
	public class LoggedSound : ISound
	{
		private readonly ISound sound;
		private readonly ILogger logger;
		private readonly string text;

		public LoggedSound(ISound sound, ILogger logger, string text)
		{
			if (sound == null) throw new ArgumentNullException("sound");
			if (logger == null) throw new ArgumentNullException("logger");
			if (text == null) throw new ArgumentNullException("text");

			this.sound = sound;
			this.logger = logger;
			this.text = text;
		}

		public void PlayAt(Vector3 point)
		{
			this.sound.PlayAt(point);
			this.logger.Log(this.text);
		}
	}

	public class LoggedSound2d : ISound2d
	{
		private readonly ISound2d sound;
		private readonly ILogger logger;
		private readonly string text;

		public LoggedSound2d(ISound2d sound, ILogger logger, string text)
		{
			if (sound == null) throw new ArgumentNullException("sound");
			if (logger == null) throw new ArgumentNullException("logger");
			if (text == null) throw new ArgumentNullException("text");

			this.sound = sound;
			this.logger = logger;
			this.text = text;
		}

		public void Play()
		{
			this.sound.Play();
			this.logger.Log(this.text);
		}
	}
}
