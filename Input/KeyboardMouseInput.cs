
namespace Entity
{
	public class KeyboardMouseInput : IControllerInputNames
	{
		public IInputName HomeButton { get { return new InputName("Button10", "Home"); } }
		public IInputName PrimaryButton { get { return new InputName("Esc", "Esc"); } }
		public IInputName SecondaryButton { get { return new InputName("Button6", "Select"); } }

		public IInputName FaceButtonTop { get { return new InputName("Button3", "Y"); } }
		public IInputName FaceButtonBottom { get { return new InputName("Button0", "A"); } }
		public IInputName FaceButtonLeft { get { return new InputName("Button2", "X"); } }
		public IInputName FaceButtonRight { get { return new InputName("Button1", "B"); } }

		public bool DPadUsesAxisInput { get { return false; } }
		public IInputName DPadTop { get { return new InputName("Axis8", "DPad-Up"); } }
		public IInputName DPadBottom { get { return new InputName("Axis8", "DPad-Down"); } }
		public IInputName DPadLeft { get { return new InputName("Axis7", "DPad-Left"); } }
		public IInputName DPadRight { get { return new InputName("Axis7", "DPad-Right"); } }

		public IInputName LeftStickButon { get { return new InputName("Button8", "LeftStick"); } }
		public IInputName LeftStickXAxis { get { return new InputName("Horizontal", "LeftStick-Horizontal"); } }
		public IInputName LeftStickYAxis { get { return new InputName("Vertical", "LeftStick-Vertical"); } }

		public IInputName RightStickButon { get { return new InputName("Button9", "RightStick"); } }
		public IInputName RightStickXAxis { get { return new InputName("Mouse X", "Mouse-X"); } }
		public IInputName RightStickYAxis { get { return new InputName("Mouse Y", "Mouse-Y"); } }

		public IInputName LeftBumper { get { return new InputName("Button4", "LeftBumper"); } }
		public IInputName RightBumper { get { return new InputName("Button5", "RightBumper"); } }

		public IInputName LeftTrigger { get { return new InputName("Axis9", "LeftTrigger"); } }
		public IInputName RightTrigger { get { return new InputName("Axis10", "RightTrigger"); } }
	}
}