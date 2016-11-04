
using System;
using UnityEngine;

namespace Entity
{
	public interface IButtonIcon
	{
		Sprite Default { get; }
		Sprite Pressed { get; }
		Sprite Disabled { get; }
	}

	public interface IAxisIcon
	{
		Sprite DefaultIcon { get; }
		Sprite PositiveIcon { get; }
		Sprite NegativeIcon { get; }
	}

	public interface IThumbStickIcon
	{
		IAxisIcon HorizontalIcons { get; }
		IAxisIcon VerticalIcons { get; }
		IButtonIcon ButtonIcon { get; }
	}

	public interface IButtonIcons
	{
		IButtonIcon HomeButton { get; }
		IButtonIcon PrimaryButton { get; }
		IButtonIcon SecondaryButton { get; }

		IButtonIcon FaceButtonTop { get; }
		IButtonIcon FaceButtonBottom { get; }
		IButtonIcon FaceButtonLeft { get; }
		IButtonIcon FaceButtonRight { get; }

		IButtonIcon DPadTop { get; }
		IButtonIcon DPadBottom { get; }
		IButtonIcon DPadLeft { get; }
		IButtonIcon DPadRight { get; }

		IThumbStickIcon LeftStick { get; }
		IThumbStickIcon RightStick { get; }

		IButtonIcon LeftBumper { get; }
		IButtonIcon RightBumper { get; }

		IButtonIcon LeftTrigger { get; }
		IButtonIcon RightTrigger { get; }
	}

	public class ButtonIcon : IButtonIcon
	{
		public ButtonIcon(Sprite defaultIcon, Sprite pressedIcon)
		{
			//if (defaultIcon == null)
			//    throw new ArgumentNullException("defaultIcon");
			//if (pressedIcon == null)
			//    throw new ArgumentNullException("pressedIcon");

			this.Default = defaultIcon;
			this.Pressed = pressedIcon;
			this.Disabled = defaultIcon;// Copy(defaultIcon);
										//SetAlpha(this.Disabled, 0.3f);
		}

		public Sprite Default { get; private set; }
		public Sprite Pressed { get; private set; }
		public Sprite Disabled { get; private set; }

		private Sprite Copy(Sprite s)
		{
			return Sprite.Create(s.texture,
				s.textureRect, s.pivot);
		}

		private void SetAlpha(Sprite s, float alpha)
		{
			var pixels = s.texture.GetPixels();
			for (var i = 0; i < pixels.Length; i++)
			{
				var color = pixels[i];
				color.a = alpha;
			}

			s.texture.SetPixels(pixels);
		}
	}

	public class AxisIcon : IAxisIcon
	{
		public AxisIcon(Sprite defaultIcon,
			Sprite positiveIcon, Sprite negativeIcon)
		{
			//if (defaultIcon == null)
			//    throw new ArgumentNullException("defaultIcon");
			//if (positiveIcon == null)
			//    throw new ArgumentNullException("positiveIcon");
			//if (negativeIcon == null)
			//    throw new ArgumentNullException("negativeIcon");

			this.DefaultIcon = defaultIcon;
			this.PositiveIcon = positiveIcon;
			this.NegativeIcon = negativeIcon;
		}

		public Sprite DefaultIcon { get; private set; }
		public Sprite PositiveIcon { get; private set; }
		public Sprite NegativeIcon { get; private set; }
	}

	public class ThumbStickIcon : IThumbStickIcon
	{
		public ThumbStickIcon(IAxisIcon horizontal,
			IAxisIcon vertical, IButtonIcon button)
		{
			if (horizontal == null)
				throw new ArgumentNullException("horizontal");
			if (vertical == null)
				throw new ArgumentNullException("vertical");
			if (button == null)
				throw new ArgumentNullException("button");

			this.HorizontalIcons = horizontal;
			this.VerticalIcons = vertical;
			this.ButtonIcon = button;
		}

		public IAxisIcon HorizontalIcons { get; private set; }
		public IAxisIcon VerticalIcons { get; private set; }
		public IButtonIcon ButtonIcon { get; private set; }
	}

	public class ButtonIcons : IButtonIcons
	{
		private readonly IButtonIconSource icons;

		public ButtonIcons(IButtonIconSource icons)
		{
			if (icons == null)
				throw new ArgumentNullException("icons");

			this.icons = icons;
		}

		public IButtonIcon HomeButton
		{
			get
			{
				return new ButtonIcon(
			  this.icons.HomeButtonDefault,
			  this.icons.HomeButtonPressed);
			}
		}

		public IButtonIcon PrimaryButton
		{
			get
			{
				return new ButtonIcon(
			  this.icons.PrimaryButtonDefault,
			  this.icons.PrimaryButtonPressed);
			}
		}

		public IButtonIcon SecondaryButton
		{
			get
			{
				return new ButtonIcon(
			  this.icons.SecondaryButtonDefault,
			  this.icons.SecondaryButtonPressed);
			}
		}

		public IButtonIcon FaceButtonTop
		{
			get
			{
				return new ButtonIcon(
			  this.icons.FaceButtonTopDefault,
			  this.icons.FaceButtonTopPressed);
			}
		}

		public IButtonIcon FaceButtonBottom
		{
			get
			{
				return new ButtonIcon(
			  this.icons.FaceButtonBottomDefault,
			  this.icons.FaceButtonBottomPressed);
			}
		}

		public IButtonIcon FaceButtonLeft
		{
			get
			{
				return new ButtonIcon(
					this.icons.FaceButtonLeftDefault,
					this.icons.FaceButtonLeftPressed);
			}
		}

		public IButtonIcon FaceButtonRight
		{
			get
			{
				return new ButtonIcon(
					this.icons.FaceButtonRightDefault,
					this.icons.FaceButtonRightPressed);
			}
		}

		public IButtonIcon DPadTop
		{
			get
			{
				return new ButtonIcon(
					this.icons.DPadTopDefault,
					this.icons.DPadTopPressed);
			}
		}

		public IButtonIcon DPadBottom
		{
			get
			{
				return new ButtonIcon(
					this.icons.DPadBottomDefault,
					this.icons.DPadBottomPressed);
			}
		}

		public IButtonIcon DPadLeft
		{
			get
			{
				return new ButtonIcon(
			  this.icons.DPadLeftDefault,
			  this.icons.DPadLeftPressed);
			}
		}

		public IButtonIcon DPadRight
		{
			get
			{
				return new ButtonIcon(
			  this.icons.DPadRightDefault,
			  this.icons.DPadRightPressed);
			}
		}

		public IThumbStickIcon LeftStick
		{
			get
			{
				return new ThumbStickIcon(
				  new AxisIcon(
					  this.icons.LeftStickHorizontalDefault,
					  this.icons.LeftStickHorizontalPositive,
					  this.icons.LeftStickHorizontalNegative),
				  new AxisIcon(
					  this.icons.LeftStickVerticalDefault,
					  this.icons.LeftStickVerticalPositive,
					  this.icons.LeftStickVerticalNegative),
				  new ButtonIcon(
					  this.icons.LeftStickButtonDefault,
					  this.icons.LeftStickButtonPressed));
			}
		}

		public IThumbStickIcon RightStick
		{
			get
			{
				return new ThumbStickIcon(
				  new AxisIcon(
					  this.icons.RightStickHorizontalDefault,
					  this.icons.RightStickHorizontalPositive,
					  this.icons.RightStickHorizontalNegative),
				  new AxisIcon(
					  this.icons.RightStickVerticalDefault,
					  this.icons.RightStickVerticalPositive,
					  this.icons.RightStickVerticalNegative),
				  new ButtonIcon(
					  this.icons.RightStickButtonDefault,
					  this.icons.RightStickButtonPressed));
			}
		}

		public IButtonIcon LeftBumper
		{
			get
			{
				return new ButtonIcon(
			  this.icons.LeftBumperDefault,
			  this.icons.LeftBumperPressed);
			}
		}

		public IButtonIcon RightBumper
		{
			get
			{
				return new ButtonIcon(
			  this.icons.RightBumperDefault,
			  this.icons.RightBumperPressed);
			}
		}

		public IButtonIcon LeftTrigger
		{
			get
			{
				return new ButtonIcon(
			  this.icons.LeftTriggerDefault,
			  this.icons.LeftTriggerPressed);
			}
		}

		public IButtonIcon RightTrigger
		{
			get
			{
				return new ButtonIcon(
			  this.icons.RightTriggerDefault,
			  this.icons.RightTriggerPressed);
			}
		}
	}
}