
using UnityEngine;

namespace Entity
{
	public class ControllerFactory : IFactory<IController>
	{
		public IController Create()
		{
			IControllerInputNames inputNames = null;
			IButtonIconSource iconSource = null;
			var platform = new PlatformSource();

			//var controller = GetControllerType();

			if (platform.IsWindows)
			{
				inputNames = new XboxOneWindowsInput();
				iconSource = new XboxOneIconSource();
			}
			else if (platform.IsMac)
			{
				inputNames = new Ds4MacInput();
				iconSource = new XboxOneIconSource();
			}

			var icons = new ButtonIcons(iconSource);
			return new Controller(inputNames, icons);
		}

		private string GetControllerType()
		{
			var controllers = Input.GetJoystickNames()
				.Where(c => !string.IsNullOrEmpty(c));

			if (controllers.Length == 0)
				return null;
			if (controllers.Length == 1)
				return controllers[0];

			return controllers[0];
		}
	}
}