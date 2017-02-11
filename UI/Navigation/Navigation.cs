
using System;

namespace Entity
{
	public interface INavigatable : IFocusable
	{
		void CheckNavigation();
		INavItem SelectedItem { get; }
        void Refresh();
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
    
    public class NullNav : INavigatable
    {
        public void CheckNavigation() { }
        public INavItem SelectedItem { get { return null; } }
        public void Refresh() { }
        public bool HasFocus { get; set; } 
    }

    public class Navigation<T> : INavigation<T>
	{
		protected readonly INavItem[] items;
		private readonly IButton selectButton;
		private readonly IButton cancelButton;
		private readonly ISound2d onMoveSound;
		private readonly INavEvents events;
		private readonly bool childNavsCaptureInput;
		private readonly bool autoResolveFocus;
		private readonly bool autoSelectItem;
        private int selectedIndex;
		private bool hasFocus = false;

		public Navigation(IButton selectButton, IButton cancelButton,
			INavItem[] items, ISound2d onMoveSound, INavEvents events,
			bool childNavsCaptureInput = true,
			bool autoResolveFocus = false,
			bool autoSelectItem = false)
		{
			if (selectButton == null)
				throw new ArgumentNullException("selectButton");
			if (cancelButton == null)
				throw new ArgumentNullException("cancelButton");
			if (items == null)
				throw new ArgumentNullException("items");
            if (items.Length == 0)
                throw new ArgumentException("navigation items cannot be empty");
            if (onMoveSound == null)
                throw new ArgumentNullException("onMoveSound");
            if (events == null)
                throw new ArgumentNullException("events");
                
			this.selectButton = selectButton;
			this.cancelButton = cancelButton;
			this.items = items;
			this.onMoveSound = onMoveSound;
			this.events = events;
			this.childNavsCaptureInput = childNavsCaptureInput;
			this.autoResolveFocus = autoResolveFocus;
			this.autoSelectItem = autoSelectItem;
            this.Refresh();
		}
        
        protected int CurrentIndex { get { return this.selectedIndex; } }

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
			get { return this.items[this.selectedIndex]; }
		}

		public IButton SelectButton { get { return this.selectButton; } }
		public IButton CancelButton { get { return this.cancelButton; } }
		public T PreviousInput { get; protected set; }
		public T NextInput { get; protected set; }

		public bool HasNext
		{
			get { return this.selectedIndex < (this.items.Length - 1); }
		}

		public bool HasPrevious
		{
            get { return this.selectedIndex > 0; }
		}

		protected virtual void CheckInput()
		{
		}

		protected void MoveNext(int skip = 0)
		{
            if (skip < 0)
                skip = 0;
                
			var previousItem = this.SelectedItem;
            var newIndex = this.selectedIndex + skip + 1;
            if (newIndex >= this.items.Length)
                newIndex = this.items.Length - 1;

            this.selectedIndex = newIndex;
			OnMove(previousItem);
		}

		protected void MovePrevious(int skip = 0)
		{
            if (skip < 0)
                skip = 0;
                
            var previousItem = this.SelectedItem;
            var newIndex = this.selectedIndex - skip - 1;
            if (newIndex < 0)
                newIndex = 0;

            this.selectedIndex = newIndex;
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

				var selectedItem = this.SelectedItem;
				if (selectedItem != null)
					selectedItem.HasFocus = value;

				this.hasFocus = value;
			}
		}
        
        public void Refresh()
        {
            for (var i = 0; i < this.items.Length; i++)
                this.items[i].HasFocus = i == this.selectedIndex;
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

        public void Refresh()
        {
            this.navigation.Refresh();
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
