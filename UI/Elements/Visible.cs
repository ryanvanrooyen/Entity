
using UnityEngine;
using System;
using System.Collections;

namespace Entity
{
	public interface IVisible
	{
		bool IsVisible { get; set; }
	}

	public class NullVisible : IVisible
	{
		public virtual bool IsVisible { get; set; }
	}

	public class Visible : IVisible
	{
		protected readonly GameObject gameObject;

		public Visible(GameObject gameObject, bool? initVisible = null)
		{
			if (gameObject == null)
				throw new ArgumentNullException("gameObject");

			this.gameObject = gameObject;
			if (initVisible.HasValue)
			{
				if (this.gameObject != null && initVisible.Value != this.gameObject.activeSelf)
					this.gameObject.SetActive(initVisible.Value);
			}
		}

		public virtual bool IsVisible
		{
			get
			{
				return this.gameObject != null && this.gameObject.activeSelf;
			}
			set
			{
				if (this.gameObject != null && value != this.gameObject.activeSelf)
					this.gameObject.SetActive(value);
			}
		}
	}

	public class VisibleDec : IVisible
	{
		protected readonly Action<bool> onVisible;
		private bool isVisible;

		public VisibleDec(Action<bool> onVisible = null, bool isVisible = true)
		{
			if (onVisible == null)
				throw new ArgumentNullException("onVisible");

			this.onVisible = onVisible;
			this.isVisible = isVisible;
		}

		public bool IsVisible
		{
			get { return this.isVisible; }
			set
			{
				if (this.isVisible == value)
					return;

				this.isVisible = value;
				if (this.onVisible != null)
					this.onVisible(value);
			}
		}
	}

	public class FadeVisible : IVisible
	{
		private readonly GameObject gameObject;
		private readonly Action<float> setAlpha;
		private bool isVisible;

		public FadeVisible(GameObject gameObject,
			Action<float> setAlpha, bool isVisible = true)
		{
			if (gameObject == null)
				throw new ArgumentNullException("gameObject");
			if (setAlpha == null)
				throw new ArgumentNullException("setAlpha");

			this.gameObject = gameObject;
			this.setAlpha = setAlpha;
			this.isVisible = isVisible;

			if (!this.isVisible)
				this.setAlpha(0f);
		}

		public bool IsVisible
		{
			get
			{
				return this.isVisible;
			}
			set
			{
				if (value == this.isVisible)
					return;

				this.isVisible = value;
				Fade();
			}
		}

		private void Fade()
		{
			if (this.gameObject == null)
				return;
			
			if (this.isVisible)
				this.gameObject.Run("FadeOut", FadeOut());
			else
				this.gameObject.Run("FadeIn", FadeIn());
		}

		private IEnumerator FadeIn()
		{
			for (var alpha = 0f; alpha <= 1f; alpha += 0.1f)
			{
				this.setAlpha(alpha);
				yield return null;
			}
		}

		private IEnumerator FadeOut()
		{
			for (var alpha = 1f; alpha >= 0; alpha -= 0.1f)
			{
				this.setAlpha(alpha);
				yield return null;
			}
		}
	}
	
	public class ScaleVisible : IVisible
	{
		private readonly GameObject gameObject;
		private readonly float visibleScale;
		private readonly float invisibleScale;
		private readonly float duration;
		private bool isVisible;

		public ScaleVisible(GameObject gameObject,
			float visibleScale, float invisibleScale, float duration, bool isVisible = true)
		{
			if (gameObject == null)
				throw new ArgumentNullException("gameObject");

			this.gameObject = gameObject;
			this.visibleScale = visibleScale;
			this.invisibleScale = invisibleScale;
			this.duration = duration;
			this.isVisible = isVisible;

			this.gameObject.SetActive(isVisible);
			if (isVisible)
				this.gameObject.Scale(visibleScale);
		}

		public bool IsVisible
		{
			get
			{
				return this.isVisible;
			}
			set
			{
				if (value == this.isVisible)
					return;

				this.isVisible = value;
				if (this.gameObject == null)
					return;


				var popInDuration = 0.08f;
				var popInAmount = 0.08f;
				
				if (value)
				{
					this.gameObject.Scale(this.invisibleScale);
					this.gameObject.SetActive(true);

					this.gameObject.Scale(this.visibleScale + popInAmount, this.duration - popInDuration,
						animateIndependentOfTime: true, callback: () => {

						this.gameObject.Scale(this.visibleScale, popInDuration, animateIndependentOfTime: true);
					});

					return;
				}

				this.gameObject.Scale(this.visibleScale + popInAmount, popInDuration,
						animateIndependentOfTime: true, callback: () => {

						this.gameObject.Scale(this.invisibleScale, this.duration - popInDuration,
							animateIndependentOfTime: true, callback: () => this.gameObject.SetActive(false));
				});
				
			}
		}
	}

	public class VisibleComposite : IVisible
	{
		private readonly IVisible[] visibleObjs;

		public VisibleComposite(params IVisible[] objects)
		{
			if (objects == null)
				throw new ArgumentNullException("objects");
			this.visibleObjs = objects;
		}

		public bool IsVisible
		{
			get { return this.visibleObjs.All(v => v.IsVisible); }
			set
			{
				foreach (var v in this.visibleObjs)
					v.IsVisible = value;
			}
		}
	}
}