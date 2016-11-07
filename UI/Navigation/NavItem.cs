
using System;
using UnityEngine;

namespace Entity
{
	public interface INavItem : IFocusable
	{
		void OnSelect();
		void OnCancel();
		INavigatable ChildNav { get; }
	}

	public class NavItem : INavItem
	{
		private readonly Action onSelect;
		private readonly Action<bool> onFocus;
		private readonly Action onCancel;
		private readonly INavigatable childNav;
		private bool hasFocus = false;

		public NavItem(Action onSelect = null,
			Action<bool> onFocus = null, Action onCancel = null,
			INavigatable childNav = null)
		{
			this.onSelect = onSelect;
			this.onFocus = onFocus;
			this.onCancel = onCancel;
			this.childNav = childNav;
		}

		public INavigatable ChildNav { get { return this.childNav; } }

		public bool HasFocus
		{
			get { return this.hasFocus; }
			set
			{
				this.hasFocus = value;
				if (this.onFocus != null)
					this.onFocus(value);
			}
		}

		public void OnSelect()
		{
			if (this.onSelect != null)
				this.onSelect();
		}

		public void OnCancel()
		{
			if (this.onCancel != null)
				this.onCancel();
		}
	}

	public class CompositeNavItem : INavItem
	{
		private readonly INavItem[] items;
		private readonly INavigatable childNav;

		public CompositeNavItem(params INavItem[] items)
		{
			this.items = items;
			var item = this.items.FirstOrDefault(i => i.ChildNav != null);
			this.childNav = item != null ? item.ChildNav : null;
		}

		public INavigatable ChildNav { get { return this.childNav; } }

		public bool HasFocus
		{
			get
			{
				var item = this.items.FirstOrDefault();
				return item != null ? item.HasFocus : false;
			}
			set
			{
				foreach (var item in this.items)
					item.HasFocus = value;
			}
		}

		public void OnSelect()
		{
			foreach (var item in this.items)
				item.OnSelect();
		}

		public void OnCancel()
		{
			foreach (var item in this.items)
				item.OnCancel();
		}
	}

	public class VisibleNavItem : NavItem
	{
		public VisibleNavItem(IVisible item)
			: base(null, focus => item.IsVisible = focus)
		{
			if (item == null)
				throw new ArgumentNullException("item");
		}
	}

	public class FadeNavItem : NavItem
	{
		public FadeNavItem(IAlpha item,
			float focusedAlpha = 0.85f, float defaultAlpha = 0.7f)
			: base(null,
				focus => item.Alpha = focus ? focusedAlpha : defaultAlpha)
		{
			if (item == null)
				throw new ArgumentNullException("item");
		}
	}

	public class ColoredNavItem : NavItem
	{
		public ColoredNavItem(IColor item, Color focusedColor, Color defaultColor)
			: base(null, focus => item.Color = focus ? focusedColor : defaultColor)
		{
			if (item == null)
				throw new ArgumentNullException("item");
		}
	}

	public class SoundNavItem : NavItem
	{
		public SoundNavItem(ISound2d selectSound, ISound2d cancelSound)
			: base(() => selectSound.Play(), null, () => cancelSound.Play())
		{
			if (selectSound == null)
				throw new ArgumentNullException("selectSound");
			if (cancelSound == null)
				throw new ArgumentNullException("cancelSound");
		}
	}
}