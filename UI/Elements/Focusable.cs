
using System;

namespace Entity
{
	public interface IFocusable
	{
		bool HasFocus { get; set; }
	}

	public class NullFocusable : IFocusable
	{
		public virtual bool HasFocus { get; set; }
	}

	public class VisibleFocus : IFocusable
	{
		private readonly IVisible visible;

		public VisibleFocus(IVisible visible)
		{
			if (visible == null)
				throw new ArgumentNullException("visible");

			this.visible = visible;
		}

		public virtual bool HasFocus
		{
			get { return this.visible.IsVisible; }
			set { this.visible.IsVisible = value; }
		}
	}
}