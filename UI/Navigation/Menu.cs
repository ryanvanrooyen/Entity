
using System;
using UnityEngine;

namespace Entity
{
	public interface IMenu : INavigatable, INavItem, IVisible, IGameObjSource
	{
	}

	public abstract class Menu : IMenu, INavEvents
	{
		private IGameObjSource source;
		private IVisible visible;
		protected INavigatable nav;
		protected bool hideOnSubMenus = true;

		public bool IsVisible
		{
			get { return this.visible.IsVisible; }
			set { this.visible.IsVisible = value; }
		}

		public virtual bool HasFocus
		{
			get { return this.nav.HasFocus; }
			set
			{
				if (value != this.nav.HasFocus)
					this.nav.HasFocus = value;
				this.IsVisible = value;
			}
		}

		public INavItem SelectedItem
		{
			get { return this.nav.SelectedItem; }
		}

		public GameObject GameObj
		{
			get { return this.source.GameObj; }
		}

		protected void SetSource(GameObject gameObj, bool? initVisible = null)
		{
			this.source = new GameObjSource(gameObj);
			this.visible = new Visible(gameObj, initVisible);
		}

		protected void SetSource(IGameObjSource source, bool? initVisible = null)
		{
			this.source = source;
			this.visible = new Visible(this.source.GameObj, initVisible);
		}
        
        public void Refresh()
        {
            this.nav.Refresh();
        }

		public virtual void CheckNavigation()
		{
			if (!this.HasFocus)
				return;

			this.nav.CheckNavigation();
		}

		public virtual void OnChildNavGainedFocus()
		{
			if (this.hideOnSubMenus)
				this.IsVisible = false;
		}

		public virtual void OnChildNavGaveUpFocus()
		{
			this.IsVisible = true;
		}

		public virtual void OnGaveUpFocus()
		{
			this.IsVisible = false;
		}

		public INavigatable ChildNav { get { return this; } }
		public virtual void OnCancel() { }
		public virtual void OnSelect() { }
	}

	public class MenuContainer : ObjectGroup, INavItem
	{
		private readonly IMenu menu;

		public MenuContainer(GameObject container,
			IMenu menu, params IGameObjSource[] items)
			: base(container)
		{
			if (menu == null)
				throw new ArgumentNullException("menu");

			this.menu = menu;
			foreach (var item in items)
				this.Add(item);
		}

		public bool HasFocus
		{
			get { return this.IsVisible; }
			set { this.IsVisible = value; }
		}

		public INavigatable ChildNav { get { return this.menu; } }
		public virtual void OnCancel() { }
		public virtual void OnSelect() { }
	}
}