
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Entity
{
	public interface IImage : IVisible, IColor
	{
		IImgSource Source { get; set; }
	}

	public class Img : Visible, IImage
	{
		private readonly Image image;
		private IImgSource iconSource;

		public Img(GameObject gameObject) : base(gameObject)
		{
			if (gameObject == null)
				throw new ArgumentNullException("gameObject");
			var image = gameObject.GetComponent<Image>();
			if (image == null)
				throw new ArgumentNullException("image");
			this.image = image;
			SetAlpha(0);
		}

		public float Alpha
		{
			get { return this.image.color.a; }
			set { SetAlpha(value); }
		}

		public Color Color
		{
			get { return this.image.color; }
			set { this.image.color = value; }
		}

		public IImgSource Source
		{
			get { return this.iconSource; }
			set
			{
				this.iconSource = value;
				if (this.iconSource != null)
				{
					var icon = this.iconSource.Image;
					if (icon != this.image.sprite)
						this.image.sprite = icon;
					SetAlpha(1f);
				}
				else
				{
					SetAlpha(0);
				}
			}
		}

		private void SetAlpha(float alpha)
		{
			var color = this.image.color;
			if (color.a == alpha)
				return;

			color.a = alpha;
			this.image.color = color;
		}
	}

	public class AlwaysVisibleIcon : IImage
	{
		private readonly IImage icon;

		public AlwaysVisibleIcon(IImage icon)
		{
			if (icon == null)
				throw new ArgumentNullException("icon");

			this.icon = icon;
			this.icon.IsVisible = true;
		}

		public bool IsVisible
		{
			get { return true; }
			set { }
		}

		public float Alpha
		{
			get { return this.icon.Alpha; }
			set { this.icon.Alpha = value; }
		}

		public Color Color
		{
			get { return this.icon.Color; }
			set { this.icon.Color = value; }
		}

		public IImgSource Source
		{
			get { return this.icon.Source; }
			set { this.icon.Source = value; }
		}
	}

	public class NullIcon : IImage
	{
		public IImgSource Source { get; set; }
		public bool IsVisible { get; set; }
		public Color Color { get; set; }
		public float Alpha { get; set; }
	}
}