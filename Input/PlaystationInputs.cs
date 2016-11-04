
namespace Entity
{
	public class Ds4WindowsInput : PlaystationInputNames
	{
		public Ds4WindowsInput()
			: base(
				homeButton: "joystick button 12",
				primaryButton: "joystick button 9",
				secondaryButton: "joystick button 8",
				faceButtonTop: "joystick button 3",
				faceButtonBottom: "joystick button 1",
				faceButtonLeft: "joystick button 0",
				faceButtonRight: "joystick button 2",
				dpadTop: "Axis8", dpadBottom: "Axis8", dpadLeft: "Axis7", dpadRight: "Axis7",
				leftStickButton: "joystick button 10", leftStickXAxis: "Axis1", leftStickYAxis: "Axis2",
				rightStickButton: "joystick button 11", rightStickXAxis: "Axis3", rightStickYAxis: "Axis4",
				leftBumper: "joystick button 4", rightBumper: "joystick button 5", leftTrigger: "Axis5", rightTrigger: "Axis6")
		{
		}

		public override bool DPadUsesAxisInput { get { return true; } }
	}

	public class Ds4MacInput : PlaystationInputNames
	{
		public Ds4MacInput()
			: base(
				homeButton: "joystick button 12",
				primaryButton: "joystick button 9",
				secondaryButton: "joystick button 8",
				faceButtonTop: "joystick button 3",
				faceButtonBottom: "joystick button 1",
				faceButtonLeft: "joystick button 0",
				faceButtonRight: "joystick button 2",
				dpadTop: "Axis8", dpadBottom: "Axis8", dpadLeft: "Axis7", dpadRight: "Axis7",
				leftStickButton: "joystick button 10", leftStickXAxis: "Axis1", leftStickYAxis: "Axis2",
				rightStickButton: "joystick button 11", rightStickXAxis: "Axis3", rightStickYAxis: "Axis4",
				leftBumper: "joystick button 4", rightBumper: "joystick button 5", leftTrigger: "Axis5", rightTrigger: "Axis6")
		{
		}

		public override bool DPadUsesAxisInput { get { return true; } }
	}
}