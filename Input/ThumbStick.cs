
using UnityEngine;
using System;

namespace Entity
{
	public interface IDualAxisInput
	{
		IAxisInput Horizontal { get; }
		IAxisInput Vertical { get; }
		Vector2 Value { get; }
	}

	public interface IDualAxis
	{
		IAxis Horizontal { get; }
		IAxis Vertical { get; }
		Vector2 Value { get; }
	}

	public interface IThumbStickInput : IDualAxisInput, IButtonInput
	{
	}

	public interface IThumbStick : IDualAxis, IButton
	{
		IThumbStick InvertHorizontal();
		IThumbStick InvertVertical();
	}

	public class ThumbStickInput : IThumbStickInput
	{
		private readonly IAxisInput horizontalAxis;
		private readonly IAxisInput verticalAxis;
		private readonly IButtonInput buttonInput;

		public ThumbStickInput(IAxisInput horizontalAxis,
			IAxisInput verticalAxis, IButtonInput buttonInput)
		{
			if (horizontalAxis == null)
				throw new ArgumentNullException("horizontalAxis");
			if (verticalAxis == null)
				throw new ArgumentNullException("verticalAxis");
			if (buttonInput == null)
				throw new ArgumentNullException("buttonInput");

			this.horizontalAxis = horizontalAxis;
			this.verticalAxis = verticalAxis;
			this.buttonInput = buttonInput;
		}

		public IAxisInput Horizontal
		{
			get { return this.horizontalAxis; }
		}

		public IAxisInput Vertical
		{
			get { return this.verticalAxis; }
		}

		public string Name
		{
			get { return this.buttonInput.Name; }
		}

		public Vector2 Value
		{
			get { return new Vector2(this.Horizontal.Value, this.Vertical.Value); }
		}

		public bool IsPressed
		{
			get { return this.buttonInput.IsPressed; }
		}

		public bool WasPressed
		{
			get { return this.buttonInput.WasPressed; }
		}
	}

	public class ThumbStick : Button, IThumbStick
	{
		private readonly IThumbStickInput input;
		private readonly IAxis horizontal;
		private readonly IAxis vertical;
		private readonly IButtonIcon icon;

		public ThumbStick(IThumbStickInput input, IThumbStickIcon icon)
			: this(input,
				  new Axis(input.Horizontal, icon.HorizontalIcons),
				  new Axis(input.Vertical, icon.VerticalIcons), icon.ButtonIcon)
		{
		}

		private ThumbStick(IThumbStickInput input,
			IAxis horizontal, IAxis vertical, IButtonIcon icon) : base(input, icon)
		{
			if (input == null)
				throw new ArgumentNullException("input");
			if (horizontal == null)
				throw new ArgumentNullException("horizontal");
			if (vertical == null)
				throw new ArgumentNullException("vertical");
			if (icon == null)
				throw new ArgumentNullException("icons");

			this.input = input;
			this.icon = icon;
			this.horizontal = horizontal;
			this.vertical = vertical;
		}

		public IAxis Horizontal
		{
			get { return this.horizontal; }
		}

		public IAxis Vertical
		{
			get { return this.vertical; }
		}

		public Vector2 Value
		{
			get { return this.input.Value; }
		}

		public IThumbStick InvertHorizontal()
		{
			return new ThumbStick(this.input,
				this.horizontal.Invert(),
				this.vertical,
				this.icon);
		}

		public IThumbStick InvertVertical()
		{
			return new ThumbStick(this.input,
				this.horizontal,
				this.vertical.Invert(),
				this.icon);
		}
	}
}