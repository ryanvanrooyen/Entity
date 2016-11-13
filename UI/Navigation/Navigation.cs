
using System;

namespace Entity
{
	public interface INavigatable : IFocusable
	{
		void CheckNavigation();
		INavItem SelectedItem { get; }
	}

	public interface INavigation<T> : INavigatable
	{
		IButton SelectButton { get; }
		IButton CancelButton { get; }
		T PreviousInput { get; }
		T NextInput { get; }
		bool HasNext { get; }
		bool HasPrevious { get; }
	}

	public interface INavEvents
	{
		void OnChildNavGainedFocus();
		void OnChildNavGaveUpFocus();
		void OnGaveUpFocus();
	}

	public class NullNavEvents : INavEvents
	{
		public void OnChildNavGainedFocus() { }
		public void OnChildNavGaveUpFocus() { }
		public void OnGaveUpFocus() { }
	}

	public class Navigation<T> : INavigation<T>
	{
		private readonly INavItemSource itemSource;
		private readonly IButton selectButton;
		private readonly IButton cancelButton;
		private readonly ISound2d onMoveSound;
		private readonly INavEvents events;
		private readonly bool childNavsCaptureInput;
		private readonly bool autoResolveFocus;
		private readonly bool autoSelectItem;
		private bool hasFocus = false;

		public Navigation(IButton selectButton, IButton cancelButton,
			INavItemSource itemSource, ISound2d onMoveSound, INavEvents events,
			bool childNavsCaptureInput = true,
			bool autoResolveFocus = false,
			bool autoSelectItem = false)
		{
			if (selectButton == null)
				throw new ArgumentNullException("selectButton");
			if (cancelButton == null)
				throw new ArgumentNullException("cancelButton");
			if (itemSource == null)
				throw new ArgumentNullException("itemSource");
			if (onMoveSound == null)
				throw new ArgumentNullException("onMoveSound");
			if (events == null)
				throw new ArgumentNullException("events");

			this.selectButton = selectButton;
			this.cancelButton = cancelButton;
			this.itemSource = itemSource;
			this.onMoveSound = onMoveSound;
			this.events = events;
			this.childNavsCaptureInput = childNavsCaptureInput;
			this.autoResolveFocus = autoResolveFocus;
			this.autoSelectItem = autoSelectItem;
		}

		public virtual void CheckNavigation()
		{
			if (!this.HasFocus)
				return;

			var selectedItem = this.SelectedItem;
			if (selectedItem != null)
			{
				var childNavigation = selectedItem.ChildNav;
				if (childNavigation != null && this.autoSelectItem && !childNavigation.HasFocus)
				{
					childNavigation.HasFocus = true;
					this.events.OnChildNavGainedFocus();
				}

				if (childNavigation != null && childNavigation.HasFocus)
				{
					childNavigation.CheckNavigation();
					if (!childNavigation.HasFocus)
						this.events.OnChildNavGaveUpFocus();

					if (!this.childNavsCaptureInput)
						this.CheckInput();

					return;
				}

				if (this.selectButton.WasPressed)
				{
					selectedItem.OnSelect();

					if (this.autoResolveFocus)
					{
						this.HasFocus = false;
						this.events.OnGaveUpFocus();
						return;
					}

					if (childNavigation != null)
					{
						childNavigation.HasFocus = true;
						this.events.OnChildNavGainedFocus();
						return;
					}
				}
			}

			if (this.cancelButton.WasPressed)
			{
				if (selectedItem != null)
					selectedItem.OnCancel();

				this.HasFocus = false;
				this.events.OnGaveUpFocus();
				return;
			}

			this.CheckInput();
		}

		public INavItem SelectedItem
		{
			get { return this.itemSource.SelectedItem; }
		}

		public IButton SelectButton { get { return this.selectButton; } }
		public IButton CancelButton { get { return this.cancelButton; } }
		public T PreviousInput { get; protected set; }
		public T NextInput { get; protected set; }

		public bool HasNext
		{
			get { return this.itemSource.HasNext; }
		}

		public bool HasPrevious
		{
			get { return this.itemSource.HasPrevious; }
		}

		protected virtual void CheckInput()
		{
		}

		protected void MoveNext()
		{
			var previousItem = this.SelectedItem;
			this.itemSource.MoveNext();
			OnMove(previousItem);
		}

		protected void MovePrevious()
		{
			var previousItem = this.SelectedItem;
			this.itemSource.MovePrevious();
			OnMove(previousItem);
		}

		private void OnMove(INavItem previousItem)
		{
			var newItem = this.SelectedItem;
			if (previousItem == null || previousItem == newItem)
				return;

			this.onMoveSound.Play();
			previousItem.HasFocus = false;
			newItem.HasFocus = true;
		}

		public bool HasFocus
		{
			get { return this.hasFocus; }
			set
			{
				if (value == this.hasFocus)
					return;

				this.itemSource.RefreshSelectedItem();

				var selectedItem = this.SelectedItem;
				if (selectedItem != null)
					selectedItem.HasFocus = value;

				this.hasFocus = value;
			}
		}

		private void CheckChildNav()
		{

		}
	}

	public class IconNavigation<T> : INavigation<T> where T : IImgSource
	{
		protected readonly INavigation<T> navigation;
		protected readonly IImage selectIcon;
		protected readonly IImage cancelIcon;
		protected readonly IImage nextIcon;
		protected readonly IImage previousIcon;

		public IconNavigation(INavigation<T> navigation, IImage selectIcon,
			IImage cancelIcon, IImage nextIcon, IImage previousIcon)
		{
			if (cancelIcon == null)
				throw new ArgumentNullException("cancelIcon");
			if (selectIcon == null)
				throw new ArgumentNullException("selectIcon");
			if (navigation == null)
				throw new ArgumentNullException("navigation");
			if (nextIcon == null)
				throw new ArgumentNullException("nextIcon");
			if (previousIcon == null)
				throw new ArgumentNullException("previousIcon");

			this.selectIcon = selectIcon;
			this.cancelIcon = cancelIcon;
			this.navigation = navigation;
			this.nextIcon = nextIcon;
			this.previousIcon = previousIcon;
		}

		public bool HasFocus
		{
			get { return this.navigation.HasFocus; }
			set { this.navigation.HasFocus = value; }
		}

		public IButton SelectButton
		{
			get { return this.navigation.SelectButton; }
		}

		public IButton CancelButton
		{
			get { return this.navigation.CancelButton; }
		}

		public T NextInput
		{
			get { return this.navigation.NextInput; }
		}

		public T PreviousInput
		{
			get { return this.navigation.PreviousInput; }
		}

		public bool HasNext
		{
			get { return this.navigation.HasNext; }
		}

		public bool HasPrevious
		{
			get { return this.navigation.HasPrevious; }
		}

		public INavItem SelectedItem
		{
			get { return this.navigation.SelectedItem; }
		}

		public virtual void CheckNavigation()
		{
			if (!this.HasFocus)
				return;

			this.selectIcon.Source = this.SelectButton;
			this.cancelIcon.Source = this.CancelButton;

			this.nextIcon.Source = this.NextInput;
			this.nextIcon.IsVisible = this.HasNext;

			this.previousIcon.Source = this.PreviousInput;
			this.previousIcon.IsVisible = this.HasPrevious;

			this.navigation.CheckNavigation();
		}
	}
}
