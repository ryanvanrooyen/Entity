
using System;
using UnityEngine;

namespace Entity
{
	public class LoggedSound : ISound
	{
		private readonly ISound sound;
		private readonly ILog logger;
		private readonly string text;

		public LoggedSound(ISound sound, ILog logger, string text)
		{
			if (sound == null) throw new ArgumentNullException("sound");
			if (logger == null) throw new ArgumentNullException("logger");
			if (text == null) throw new ArgumentNullException("text");

			this.sound = sound;
			this.logger = logger;
			this.text = text;
		}

		public void Play(GameObject source)
		{
			this.sound.Play(source);
			this.logger.Info(this.text);
		}
	}

	public class LoggedSound2d : ISound2d
	{
		private readonly ISound2d sound;
		private readonly ILog logger;
		private readonly string text;

		public LoggedSound2d(ISound2d sound, ILog logger, string text)
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
			this.logger.Info(this.text);
		}
	}
}
