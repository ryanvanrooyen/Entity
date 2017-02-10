
namespace Entity
{
	public interface IController
	{
		IButton HomeButton { get; }
		IButton PrimaryButton { get; }
		IButton SecondaryButton { get; }

		IFaceButtons FaceButtons { get; }
		IFaceButtons DPad { get; }

		IThumbStick LeftStick { get; }
		IThumbStick RightStick { get; }

		IButton LeftBumper { get; }
		IButton RightBumper { get; }

		ITrigger LeftTrigger { get; }
		ITrigger RightTrigger { get; }
	}

	public class Controller : IController
	{
		public Controller(IControllerInputNames names, IButtonIcons icons)
		{
			this.HomeButton = CreateButton(names.HomeButton, icons.HomeButton);
			this.PrimaryButton = CreateButton(names.PrimaryButton, icons.PrimaryButton);
			this.SecondaryButton = CreateButton(names.SecondaryButton, icons.SecondaryButton);

			this.FaceButtons = new FaceButtons(
				CreateButton(names.FaceButtonTop, icons.FaceButtonTop),
				CreateButton(names.FaceButtonBottom, icons.FaceButtonBottom),
				CreateButton(names.FaceButtonLeft, icons.FaceButtonLeft),
				CreateButton(names.FaceButtonRight, icons.FaceButtonRight));

			if (names.DPadUsesAxisInput)
			{
				this.DPad = new FaceButtons(
					CreateAxisButton(names.DPadTop, icons.DPadTop,
						 names.DPadVerticalAxisInverted ? -0.5f : 0.5f),
					CreateAxisButton(names.DPadBottom, icons.DPadBottom,
						 names.DPadVerticalAxisInverted ? 0.5f : -0.5f),
					CreateAxisButton(names.DPadLeft, icons.DPadLeft, -0.5f),
					CreateAxisButton(names.DPadRight, icons.DPadRight, 0.5f));
			}
			else
			{
				this.DPad = new FaceButtons(
					CreateButton(names.DPadTop, icons.DPadTop),
					CreateButton(names.DPadBottom, icons.DPadBottom),
					CreateButton(names.DPadLeft, icons.DPadLeft),
					CreateButton(names.DPadRight, icons.DPadRight));
			}

			this.LeftStick = CreateThumbStick(names.LeftStickXAxis,
				names.LeftStickYAxis, names.LeftStickButon, icons.LeftStick);
			this.RightStick = CreateThumbStick(names.RightStickXAxis,
				names.RightStickYAxis, names.RightStickButon, icons.RightStick);
			this.RightStick = this.RightStick.InvertVertical();

			this.LeftBumper = CreateButton(names.LeftBumper, icons.LeftBumper);
			this.RightBumper = CreateButton(names.RightBumper, icons.RightBumper);

			this.LeftTrigger = CreateTrigger(names.LeftTrigger, icons.LeftTrigger);
			this.RightTrigger = CreateTrigger(names.RightTrigger, icons.RightTrigger);
		}

		public IButton HomeButton { get; private set; }
		public IButton PrimaryButton { get; private set; }
		public IButton SecondaryButton { get; private set; }

		public IFaceButtons FaceButtons { get; private set; }
		public IFaceButtons DPad { get; private set; }

		public IThumbStick LeftStick { get; private set; }
		public IThumbStick RightStick { get; private set; }

		public IButton LeftBumper { get; private set; }
		public IButton RightBumper { get; private set; }

		public ITrigger LeftTrigger { get; private set; }
		public ITrigger RightTrigger { get; private set; }

		private IButton CreateButton(IInputName inputName, IButtonIcon icon)
		{
			return new Button(new ButtonInput(inputName), icon);
		}

		private IButton CreateAxisButton(IInputName axisName, IButtonIcon icon, float threshold)
		{
			return new Button(new AxisButtonInput(new AxisInput(axisName), threshold), icon);
		}

		private IThumbStick CreateThumbStick(IInputName xAxisName, IInputName yAxisName,
			IInputName buttonName, IThumbStickIcon icon)
		{
			var button = new ButtonInput(buttonName);

			var input = new ThumbStickInput(
				new AxisInput(xAxisName),
				new AxisInput(yAxisName), button);

			return new ThumbStick(input, icon);
		}

		private ITrigger CreateTrigger(IInputName inputName, IButtonIcon icon)
		{
			return new Trigger(new TriggerInput(inputName), icon);
		}
	}
}