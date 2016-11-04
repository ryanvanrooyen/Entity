
using System;

namespace Entity
{
	public class ButtonNavigation : Navigation<IButton>
	{
		public ButtonNavigation(IButton selectButton, IButton cancelButton,
			IButton nextButton, IButton previousButton, ISound2d onMoveSound,
			INavEvents events, INavItemSource itemSource,
			bool childNavsCaptureInput = true,
			bool autoResolveFocus = false,
			bool autoSelectItem = false) :
			base(selectButton, cancelButton, itemSource, onMoveSound, events,
					childNavsCaptureInput, autoResolveFocus, autoSelectItem)
		{
			if (nextButton == null)
				throw new ArgumentNullException("nextButton");
			if (previousButton == null)
				throw new ArgumentNullException("previousButton");

			this.PreviousInput = nextButton;
			this.NextInput = previousButton;
		}

		protected override void CheckInput()
		{
			if (this.NextInput.WasPressed)
				MoveNext();
			else if (this.PreviousInput.WasPressed)
				MovePrevious();
		}
	}

	public class MultiOptionNavigation : Navigation<IButton>
	{
		private readonly NavItemSource itemSource;
		private readonly ProxyButton selectButton;
		private readonly Option[] options;
		private bool buttonWasPressed = false;

		public MultiOptionNavigation(IButton cancelButton,
			INavEvents events, params Option[] options)
			: this(cancelButton, events, new ProxyButton(), new NavItemSource())
		{
			if (options == null)
				throw new ArgumentNullException("options");

			this.options = options;
		}

		private MultiOptionNavigation(IButton cancelButton,
			INavEvents events, ProxyButton selectButton, NavItemSource itemSource)
			: base(selectButton, cancelButton, itemSource, new NoSound(), events)
		{
			if (cancelButton == null)
				throw new ArgumentNullException("cancelButton");

			this.selectButton = selectButton;
			this.itemSource = itemSource;

			this.selectButton.WasPressedProxy = () =>
			{
				var wasPressed = this.buttonWasPressed;
				if (this.buttonWasPressed)
					this.buttonWasPressed = false;

				return wasPressed;
			};
		}

		protected override void CheckInput()
		{
			foreach (var option in this.options)
			{
				if (option.Button.WasPressed)
				{
					this.itemSource.SelectedItem = option.Item;
					this.buttonWasPressed = true;
					return;
				}
			}
		}

		public override void CheckNavigation()
		{
			if (!this.HasFocus)
				return;

			base.CheckNavigation();
		}

		public class Option
		{
			public readonly IButton Button;
			public readonly INavItem Item;

			public Option(IButton button, INavItem item)
			{
				if (button == null)
					throw new ArgumentNullException("button");
				if (item == null)
					throw new ArgumentNullException("item");

				this.Button = button;
				this.Item = item;
			}
		}
	}

	public class ButtonIconNavigation : IconNavigation<IButton>
	{
		public ButtonIconNavigation(INavigation<IButton> navigation,
			IImage selectIcon, IImage cancelIcon, IImage nextIcon, IImage previousIcon)
			: base(navigation, cancelIcon, selectIcon, nextIcon, previousIcon)
		{

		}
	}
}