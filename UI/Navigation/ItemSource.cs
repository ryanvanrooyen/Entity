
using System;

namespace Entity
{
	public interface IItemSource<T>
	{
		T SelectedItem { get; }
		void RefreshSelectedItem();
		bool HasNext { get; }
		bool HasPrevious { get; }
		void MoveNext();
		void MovePrevious();
	}

	public interface INavItemSource : IItemSource<INavItem>
	{
	}

	public class NavItemSource : INavItemSource
	{
		public INavItem SelectedItem { get; set; }
		public void RefreshSelectedItem() { }
		public bool HasNext { get; private set; }
		public bool HasPrevious { get; private set; }
		public void MoveNext() { }
		public void MovePrevious() { }
	}

	public class DynamicNavItemSource : INavItemSource
	{
		private static INavItemSource Null = new NavItemSource();
		private INavItemSource src = Null;

		public INavItemSource Source
		{
			get { return this.src; }
			set { this.src = value ?? Null; }
		}

		public INavItem SelectedItem { get { return this.src.SelectedItem; } }
		public void RefreshSelectedItem() { this.src.RefreshSelectedItem(); }
		public bool HasNext { get { return this.src.HasNext; } }
		public bool HasPrevious { get { return this.src.HasPrevious; } }
		public void MoveNext() { this.src.MoveNext(); }
		public void MovePrevious() { this.src.MovePrevious(); }
	}

	public class StaticItemSource<T> : IItemSource<T> where T : IFocusable
	{
		private T[] items;
		private int selectedIndex;

		public StaticItemSource(params T[] items)
		{
			if (items == null)
				throw new ArgumentNullException("items");
			this.Items = items;
		}

		public T[] Items
		{
			get { return this.items; }
			set
			{
				this.selectedIndex = 0;
				this.items = value;
				Refresh();
			}
		}

		public void SetSelectedItem(int index)
		{
			if (index >= 0 && index < this.items.Length)
			{
				this.selectedIndex = index;
				Refresh();
			}
		}

		public virtual T SelectedItem
		{
			get
			{
				var length = this.items.Length;
				if (length == 0 || this.selectedIndex < 0 || this.selectedIndex >= length)
					return default(T);

				return this.items[this.selectedIndex];
			}
		}

		public virtual void RefreshSelectedItem()
		{
		}

		public bool HasNext
		{
			get
			{
				var length = this.items.Length;
				return length > 0 && this.selectedIndex < (length - 1);
			}
		}

		public void MoveNext()
		{
			if (this.HasNext)
			{
				this.selectedIndex++;
				this.OnMoved();
			}
		}

		public bool HasPrevious
		{
			get
			{
				var length = this.items.Length;
				return length > 0 && this.selectedIndex > 0;
			}
		}

		public void MovePrevious()
		{
			if (this.HasPrevious)
			{
				this.selectedIndex--;
				this.OnMoved();
			}
		}

		public void Refresh()
		{
			if (this.items.Length > 0)
			{
				for (var i = 0; i < this.items.Length; i++)
					this.items[i].HasFocus = i == this.selectedIndex;
			}
		}

		protected virtual void OnMoved() { }
	}

	public class StaticNavItemSource : StaticItemSource<INavItem>, INavItemSource
	{
		public StaticNavItemSource(params INavItem[] items) : base(items)
		{
		}
	}
}
