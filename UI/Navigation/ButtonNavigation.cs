
using System;

namespace Entity
{
	public class ButtonNavigation : Navigation<IButton>
	{
		public ButtonNavigation(IButton selectButton, IButton cancelButton,
			IButton nextButton, IButton previousButton, ISound2d onMoveSound,
			INavEvents events, INavItem[] items,
			bool childNavsCaptureInput = true,
			bool autoResolveFocus = false,
			bool autoSelectItem = false) :
			base(selectButton, cancelButton, items, onMoveSound, events,
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
    
	public class ButtonIconNavigation : IconNavigation<IButton>
	{
		public ButtonIconNavigation(INavigation<IButton> navigation,
			IImage selectIcon, IImage cancelIcon, IImage nextIcon, IImage previousIcon)
			: base(navigation, cancelIcon, selectIcon, nextIcon, previousIcon)
		{

		}
	}
}