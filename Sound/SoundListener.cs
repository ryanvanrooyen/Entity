
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
	public interface ISoundListener
	{
		void OnHeard(ISound sound, GameObject source);
	}

	public interface ISoundListenerSource
	{
		void AddListener(ISoundListener listener);
		void RemoveListener(ISoundListener listener);
	}

	public class SoundListener : MonoBehaviour, ISoundListenerSource, ISoundListener
	{
		private List<ISoundListener> listeners;

		public void Start()
		{
			this.gameObject.AddMetadata<ISoundListener>(this);
		}

		public void AddListener(ISoundListener listener)
		{
			if (listener == null)
				return;
			if (this.listeners == null)
				this.listeners = new List<ISoundListener>();
			if (this.listeners.Contains(listener))
				return;

			this.listeners.Add(listener);
		}

		public void RemoveListener(ISoundListener listener)
		{
			if (listener == null)
				return;
			if (this.listeners == null)
				return;
			if (!this.listeners.Contains(listener))
				return;

			this.listeners.Remove(listener);
		}

		public void OnHeard(ISound sound, GameObject source)
		{
			if (sound == null)
				return;

			for (var i = 0; i < this.listeners.Count; i++)
				this.listeners[i].OnHeard(sound, source);
		}
	}
}
