
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
	public interface IAlpha
	{
		float Alpha { get; set; }
	}

	public interface IColor : IAlpha
	{
		Color Color { get; set; }
	}

	public class CompositeColor : AlphaComposite, IColor
	{
		private readonly IColor[] colors;

		public CompositeColor(params IColor[] colors) : base(colors)
		{
			this.colors = colors;
		}

		public Color Color
		{
			get { return this.colors.FirstOrDefault().Color; }

			set
			{
				foreach (var color in this.colors)
					color.Color = value;
			}
		}
	}

	public class AlphaChannel : IAlpha
	{
		private readonly Renderer renderer;

		public AlphaChannel(GameObject gameObject, float alpha = 1f)
		{
			if (gameObject == null)
				throw new ArgumentNullException("gameObject");

			this.renderer = gameObject.GetComponent<Renderer>();

			if (this.renderer == null)
				throw new ArgumentException("gameObject does not contain Renderer component");

			SetAlpha(alpha);
		}

		public float Alpha
		{
			get { return this.renderer.material.color.a; }
			set
			{
				SetAlpha(value);
			}
		}

		private void SetAlpha(float alpha)
		{
			if (alpha < 0)
				alpha = 0;
			else if (alpha > 1f)
				alpha = 1f;

			Color c = this.renderer.material.color;
			c.a = alpha;
			renderer.material.color = c;
		}
	}

	public class AlphaComposite : IAlpha
	{
		private readonly IAlpha[] channels;

		public AlphaComposite(params IAlpha[] channels)
		{
			if (channels == null)
				throw new ArgumentNullException("channels");
			this.channels = channels;
		}

		public float Alpha
		{
			get { return this.channels.FirstOrDefault().Alpha; }
			set
			{
				foreach (var v in this.channels)
					v.Alpha = value;
			}
		}
	}
}