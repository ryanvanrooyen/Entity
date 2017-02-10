
using UnityEngine;
using System;

namespace Entity
{
	public interface IUIButton : IButton, IVisible
	{
	}

	public class UIButton : ButtonDecorator, IUIButton
	{
		private readonly IImage icon;
		private readonly Action onPressed;
		private readonly Action<bool> isPressed;

		public UIButton(IButton button, GameObject obj,
			Action onPressed = null, Action<bool> isPressed = null) : base(button)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			this.onPressed = onPressed;
			this.isPressed = isPressed;
			this.icon = new Img(obj);
			var behavior = obj.AddBehavior();
			behavior.OnUpdate = () => RefreshImage();
		}

		public bool IsVisible
		{
			get { return this.icon.IsVisible; }
			set { this.icon.IsVisible = value; }
		}

		public override IButton New()
		{
			return this;
		}

		private void RefreshImage()
		{
			this.icon.Source = this.button;

			if (this.isPressed != null)
				this.isPressed(this.button.IsPressed);
			if (this.onPressed != null && this.button.WasPressed)
				this.onPressed();
		}
	}
}