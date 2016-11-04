
namespace Entity
{
	public class XboxOneWindowsInput : XboxInputNames
	{
		public XboxOneWindowsInput()
			: base(
				homeButton: "Button10",
				primaryButton: "Button7",
				secondaryButton: "Button6",
				faceButtonTop: "Button3",
				faceButtonBottom: "Button0",
				faceButtonLeft: "Button2",
				faceButtonRight: "Button1",
				dpadTop: "Axis7", dpadBottom: "Axis7", dpadLeft: "Axis6", dpadRight: "Axis6",
				leftStickButton: "Button8", leftStickXAxis: "Horizontal", leftStickYAxis: "Vertical",
				rightStickButton: "Button9", rightStickXAxis: "Axis4", rightStickYAxis: "Axis5",
				leftBumper: "Button4", rightBumper: "Button5", leftTrigger: "Axis9", rightTrigger: "Axis10")
		{ }

		public override bool DPadUsesAxisInput { get { return true; } }
	}

	public class XboxOneMacInput : XboxInputNames
	{
		public XboxOneMacInput()
			: base(
				homeButton: "Button15",
				primaryButton: "Button9",
				secondaryButton: "Button10",
				faceButtonTop: "Button19",
				faceButtonBottom: "Button16",
				faceButtonLeft: "Button18",
				faceButtonRight: "Button17",
				dpadTop: "Button5", dpadBottom: "Button6", dpadLeft: "Button7", dpadRight: "Button8",
				leftStickButton: "Button11", leftStickXAxis: "Horizontal", leftStickYAxis: "Vertical",
				rightStickButton: "Button12", rightStickXAxis: "Axis3", rightStickYAxis: "Axis4",
				leftBumper: "Button13", rightBumper: "Button14", leftTrigger: "Axis5", rightTrigger: "Axis6")
		{ }
	}
}