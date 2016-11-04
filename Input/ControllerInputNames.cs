
namespace Entity
{
	public interface IControllerInputNames
	{
		IInputName HomeButton { get; }
		IInputName PrimaryButton { get; }
		IInputName SecondaryButton { get; }

		IInputName FaceButtonTop { get; }
		IInputName FaceButtonBottom { get; }
		IInputName FaceButtonLeft { get; }
		IInputName FaceButtonRight { get; }

		bool DPadUsesAxisInput { get; }
		IInputName DPadTop { get; }
		IInputName DPadBottom { get; }
		IInputName DPadLeft { get; }
		IInputName DPadRight { get; }

		IInputName LeftStickButon { get; }
		IInputName LeftStickXAxis { get; }
		IInputName LeftStickYAxis { get; }
		IInputName RightStickButon { get; }
		IInputName RightStickXAxis { get; }
		IInputName RightStickYAxis { get; }

		IInputName LeftBumper { get; }
		IInputName RightBumper { get; }

		IInputName LeftTrigger { get; }
		IInputName RightTrigger { get; }
	}

	public class InputNames : IControllerInputNames
	{
		public InputNames(
			string homeButtonName, string homeButtonLabel,
			string primaryButtonName, string primaryButtonLabel,
			string secondaryButtonName, string secondaryButtonLabel,
			string faceButtonTopName, string faceButtonTopLabel,
			string faceButtonBottomName, string faceButtonBottomLabel,
			string faceButtonLeftName, string faceButtonLeftLabel,
			string faceButtonRightName, string faceButtonRightLabel,
			string dpadTopName, string dpadTopLabel,
			string dpadBottomName, string dpadBottomLabel,
			string dpadLeftName, string dpadLeftLabel,
			string dpadRightName, string dpadRightLabel,
			string leftStickButtonName, string leftStickButtonLabel,
			string leftStickXAxisName, string leftStickXAxisLabel,
			string leftStickYAxisName, string leftStickYAxisLabel,
			string rightStickButtonName, string rightStickButtonLabel,
			string rightStickXAxisName, string rightStickXAxisLabel,
			string rightStickYAxisName, string rightStickYAxisLabel,
			string leftBumperName, string leftBumperLabel,
			string rightBumperName, string rightBumperLabel,
			string leftTriggerName, string leftTriggerLabel,
			string rightTriggerName, string rightTriggerLabel)
		{
			this.HomeButton = new InputName(homeButtonName, homeButtonLabel);
			this.PrimaryButton = new InputName(primaryButtonName, primaryButtonLabel);
			this.SecondaryButton = new InputName(secondaryButtonName, secondaryButtonLabel);

			this.FaceButtonTop = new InputName(faceButtonTopName, faceButtonTopLabel);
			this.FaceButtonBottom = new InputName(faceButtonBottomName, faceButtonBottomLabel);
			this.FaceButtonLeft = new InputName(faceButtonLeftName, faceButtonLeftLabel);
			this.FaceButtonRight = new InputName(faceButtonRightName, faceButtonRightLabel);

			this.DPadTop = new InputName(dpadTopName, dpadTopLabel);
			this.DPadBottom = new InputName(dpadBottomName, dpadBottomLabel);
			this.DPadLeft = new InputName(dpadLeftName, dpadLeftLabel);
			this.DPadRight = new InputName(dpadRightName, dpadRightLabel);

			this.LeftStickButon = new InputName(leftStickButtonName, leftStickButtonLabel);
			this.LeftStickXAxis = new InputName(leftStickXAxisName, leftStickXAxisLabel);
			this.LeftStickYAxis = new InputName(leftStickYAxisName, leftStickYAxisLabel);

			this.RightStickButon = new InputName(rightStickButtonName, rightStickButtonLabel);
			this.RightStickXAxis = new InputName(rightStickXAxisName, rightStickXAxisLabel);
			this.RightStickYAxis = new InputName(rightStickYAxisName, rightStickYAxisLabel);

			this.LeftBumper = new InputName(leftBumperName, leftBumperLabel);
			this.RightBumper = new InputName(rightBumperName, rightBumperLabel);

			this.LeftTrigger = new InputName(leftTriggerName, leftTriggerLabel);
			this.RightTrigger = new InputName(rightTriggerName, rightTriggerLabel);
		}

		public IInputName HomeButton { get; private set; }
		public IInputName PrimaryButton { get; private set; }
		public IInputName SecondaryButton { get; private set; }

		public IInputName FaceButtonTop { get; private set; }
		public IInputName FaceButtonBottom { get; private set; }
		public IInputName FaceButtonLeft { get; private set; }
		public IInputName FaceButtonRight { get; private set; }

		public virtual bool DPadUsesAxisInput { get { return false; } }
		public IInputName DPadTop { get; private set; }
		public IInputName DPadBottom { get; private set; }
		public IInputName DPadLeft { get; private set; }
		public IInputName DPadRight { get; private set; }

		public IInputName LeftStickButon { get; private set; }
		public IInputName LeftStickXAxis { get; private set; }
		public IInputName LeftStickYAxis { get; private set; }

		public IInputName RightStickButon { get; private set; }
		public IInputName RightStickXAxis { get; private set; }
		public IInputName RightStickYAxis { get; private set; }

		public IInputName LeftBumper { get; private set; }
		public IInputName RightBumper { get; private set; }

		public IInputName LeftTrigger { get; private set; }
		public IInputName RightTrigger { get; private set; }
	}

	public class ControllerInputNames : InputNames
	{
		public ControllerInputNames(
			string homeButtonName, string homeButtonLabel,
			string primaryButtonName, string primaryButtonLabel,
			string secondaryButtonName, string secondaryButtonLabel,
			string faceButtonTopName, string faceButtonTopLabel,
			string faceButtonBottomName, string faceButtonBottomLabel,
			string faceButtonLeftName, string faceButtonLeftLabel,
			string faceButtonRightName, string faceButtonRightLabel,
			string dpadTopName, string dpadBottomName, string dpadLeftName, string dpadRightName,
			string leftStickButtonName, string leftStickXAxisName, string leftStickYAxisName,
			string rightStickButtonName, string rightStickXAxisName, string rightStickYAxisName,
			string leftBumperName, string rightBumperName,
			string leftTriggerName, string rightTriggerName)
			: base(
				homeButtonName, homeButtonLabel,
				primaryButtonName, primaryButtonLabel,
				secondaryButtonName, secondaryButtonLabel,
				faceButtonTopName, faceButtonTopLabel,
				faceButtonBottomName, faceButtonBottomLabel,
				faceButtonLeftName, faceButtonLeftLabel,
				faceButtonRightName, faceButtonRightLabel,
				dpadTopName, "DPad-Up",
				dpadBottomName, "DPad-Bottom",
				dpadLeftName, "DPad-Left",
				dpadRightName, "DPad-Right",
				leftStickButtonName, "LeftStick",
				leftStickXAxisName, "LeftStick-Horizontal",
				leftStickYAxisName, "LeftStick-Vertical",
				rightStickButtonName, "RightStick",
				rightStickXAxisName, "RightStick-Horizontal",
				rightStickYAxisName, "RightStick-Vertical",
				leftBumperName, "LeftBumper",
				rightBumperName, "RightBumper",
				leftTriggerName, "LeftTrigger",
				rightTriggerName, "RightTrigger")
		{ }
	}

	public class XboxInputNames : ControllerInputNames
	{
		public XboxInputNames(
			string homeButton, string primaryButton, string secondaryButton,
			string faceButtonTop, string faceButtonBottom, string faceButtonLeft, string faceButtonRight,
			string dpadTop, string dpadBottom, string dpadLeft, string dpadRight,
			string leftStickButton, string leftStickXAxis, string leftStickYAxis,
			string rightStickButton, string rightStickXAxis, string rightStickYAxis,
			string leftBumper, string rightBumper, string leftTrigger, string rightTrigger)
			: base(
				homeButton, "Xbox",
				primaryButton, "Start",
				secondaryButton, "Select",
				faceButtonTop, "Y",
				faceButtonBottom, "A",
				faceButtonLeft, "X",
				faceButtonRight, "B",
				dpadTop, dpadBottom, dpadLeft, dpadRight,
				leftStickButton, leftStickXAxis, leftStickYAxis,
				rightStickButton, rightStickXAxis, rightStickYAxis,
				leftBumper, rightBumper, leftTrigger, rightTrigger)
		{ }
	}

	public class PlaystationInputNames : ControllerInputNames
	{
		public PlaystationInputNames(
			string homeButton, string primaryButton, string secondaryButton,
			string faceButtonTop, string faceButtonBottom, string faceButtonLeft, string faceButtonRight,
			string dpadTop, string dpadBottom, string dpadLeft, string dpadRight,
			string leftStickButton, string leftStickXAxis, string leftStickYAxis,
			string rightStickButton, string rightStickXAxis, string rightStickYAxis,
			string leftBumper, string rightBumper, string leftTrigger, string rightTrigger)
			: base(
				homeButton, "Playstation",
				primaryButton, "Options",
				secondaryButton, "Share",
				faceButtonTop, "Triangle",
				faceButtonBottom, "X",
				faceButtonLeft, "Square",
				faceButtonRight, "Circle",
				dpadTop, dpadBottom, dpadLeft, dpadRight,
				leftStickButton, leftStickXAxis, leftStickYAxis,
				rightStickButton, rightStickXAxis, rightStickYAxis,
				leftBumper, rightBumper, leftTrigger, rightTrigger)
		{ }
	}
}