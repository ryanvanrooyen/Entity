
using UnityEngine;

namespace Entity
{
	public interface IButtonIconSource
	{
		Sprite HomeButtonDefault { get; }
		Sprite HomeButtonPressed { get; }
		Sprite PrimaryButtonDefault { get; }
		Sprite PrimaryButtonPressed { get; }
		Sprite SecondaryButtonDefault { get; }
		Sprite SecondaryButtonPressed { get; }

		Sprite FaceButtonTopDefault { get; }
		Sprite FaceButtonTopPressed { get; }
		Sprite FaceButtonBottomDefault { get; }
		Sprite FaceButtonBottomPressed { get; }
		Sprite FaceButtonLeftDefault { get; }
		Sprite FaceButtonLeftPressed { get; }
		Sprite FaceButtonRightDefault { get; }
		Sprite FaceButtonRightPressed { get; }

		Sprite DPadTopDefault { get; }
		Sprite DPadTopPressed { get; }
		Sprite DPadBottomDefault { get; }
		Sprite DPadBottomPressed { get; }
		Sprite DPadLeftDefault { get; }
		Sprite DPadLeftPressed { get; }
		Sprite DPadRightDefault { get; }
		Sprite DPadRightPressed { get; }

		Sprite LeftStickHorizontalDefault { get; }
		Sprite LeftStickHorizontalPositive { get; }
		Sprite LeftStickHorizontalNegative { get; }
		Sprite LeftStickVerticalDefault { get; }
		Sprite LeftStickVerticalPositive { get; }
		Sprite LeftStickVerticalNegative { get; }
		Sprite LeftStickButtonDefault { get; }
		Sprite LeftStickButtonPressed { get; }

		Sprite RightStickHorizontalDefault { get; }
		Sprite RightStickHorizontalPositive { get; }
		Sprite RightStickHorizontalNegative { get; }
		Sprite RightStickVerticalDefault { get; }
		Sprite RightStickVerticalPositive { get; }
		Sprite RightStickVerticalNegative { get; }
		Sprite RightStickButtonDefault { get; }
		Sprite RightStickButtonPressed { get; }

		Sprite LeftBumperDefault { get; }
		Sprite LeftBumperPressed { get; }
		Sprite RightBumperDefault { get; }
		Sprite RightBumperPressed { get; }

		Sprite LeftTriggerDefault { get; }
		Sprite LeftTriggerPressed { get; }
		Sprite RightTriggerDefault { get; }
		Sprite RightTriggerPressed { get; }
	}

	public class XboxOneIconSource : ButtonIconSource
	{
		public XboxOneIconSource()
		{
			this.Folder = "XboxOne/";

			this.FaceButtonTopDefault = Icon("Y_Button");
			this.FaceButtonTopPressed = Icon("Y_Button_Pressed");
			this.FaceButtonBottomDefault = Icon("A_Button");
			this.FaceButtonBottomPressed = Icon("A_Button_Pressed");
			this.FaceButtonLeftDefault = Icon("X_Button");
			this.FaceButtonLeftPressed = Icon("X_Button_Pressed");
			this.FaceButtonRightDefault = Icon("B_Button");
			this.FaceButtonRightPressed = Icon("B_Button_Pressed");

			this.LeftBumperDefault = Icon("LeftBumper");
			this.LeftBumperPressed = Icon("LeftBumper_Pressed");
			this.RightBumperDefault = Icon("RightBumper");
			this.RightBumperPressed = Icon("RightBumper_Pressed");

			this.Folder = "General/";

			this.LeftStickHorizontalPositive = Icon("RightArrow");
			this.LeftStickHorizontalNegative = Icon("LeftArrow");
			this.LeftStickVerticalPositive = Icon("UpArrow");
			this.LeftStickVerticalNegative = Icon("DownArrow");
		}
	}

	public class ButtonIconSource : IButtonIconSource
	{
		protected string Folder = "";

		public Sprite HomeButtonDefault { get; protected set; }
		public Sprite HomeButtonPressed { get; protected set; }
		public Sprite PrimaryButtonDefault { get; protected set; }
		public Sprite PrimaryButtonPressed { get; protected set; }
		public Sprite SecondaryButtonDefault { get; protected set; }
		public Sprite SecondaryButtonPressed { get; protected set; }

		public Sprite FaceButtonTopDefault { get; protected set; }
		public Sprite FaceButtonTopPressed { get; protected set; }
		public Sprite FaceButtonBottomDefault { get; protected set; }
		public Sprite FaceButtonBottomPressed { get; protected set; }
		public Sprite FaceButtonLeftDefault { get; protected set; }
		public Sprite FaceButtonLeftPressed { get; protected set; }
		public Sprite FaceButtonRightDefault { get; protected set; }
		public Sprite FaceButtonRightPressed { get; protected set; }

		public Sprite DPadTopDefault { get; protected set; }
		public Sprite DPadTopPressed { get; protected set; }
		public Sprite DPadBottomDefault { get; protected set; }
		public Sprite DPadBottomPressed { get; protected set; }
		public Sprite DPadLeftDefault { get; protected set; }
		public Sprite DPadLeftPressed { get; protected set; }
		public Sprite DPadRightDefault { get; protected set; }
		public Sprite DPadRightPressed { get; protected set; }

		public Sprite LeftStickHorizontalDefault { get; protected set; }
		public Sprite LeftStickHorizontalPositive { get; protected set; }
		public Sprite LeftStickHorizontalNegative { get; protected set; }
		public Sprite LeftStickVerticalDefault { get; protected set; }
		public Sprite LeftStickVerticalPositive { get; protected set; }
		public Sprite LeftStickVerticalNegative { get; protected set; }
		public Sprite LeftStickButtonDefault { get; protected set; }
		public Sprite LeftStickButtonPressed { get; protected set; }

		public Sprite RightStickHorizontalDefault { get; protected set; }
		public Sprite RightStickHorizontalPositive { get; protected set; }
		public Sprite RightStickHorizontalNegative { get; protected set; }
		public Sprite RightStickVerticalDefault { get; protected set; }
		public Sprite RightStickVerticalPositive { get; protected set; }
		public Sprite RightStickVerticalNegative { get; protected set; }
		public Sprite RightStickButtonDefault { get; protected set; }
		public Sprite RightStickButtonPressed { get; protected set; }

		public Sprite LeftBumperDefault { get; protected set; }
		public Sprite LeftBumperPressed { get; protected set; }
		public Sprite RightBumperDefault { get; protected set; }
		public Sprite RightBumperPressed { get; protected set; }

		public Sprite LeftTriggerDefault { get; protected set; }
		public Sprite LeftTriggerPressed { get; protected set; }
		public Sprite RightTriggerDefault { get; protected set; }
		public Sprite RightTriggerPressed { get; protected set; }

		protected Sprite Icon(string fileName)
		{
			return Resources.Load<Sprite>(
				"Images/Icons/" + this.Folder + fileName);
		}
	}
}