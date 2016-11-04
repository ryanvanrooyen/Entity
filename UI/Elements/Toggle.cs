
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Entity
{
	public interface IToggle : IVisible, IAlpha
	{
		bool Value { get; set; }
	}

	public class ToggleComp : Visible, IToggle
	{
		private readonly Toggle toggle;

		public ToggleComp(GameObject toggleObj)
			: this(toggleObj, toggleObj.GetComponentInChildren<Toggle>())
		{
		}

		public ToggleComp(Toggle toggle)
			: this(toggle.gameObject, toggle)
		{
		}

		private ToggleComp(GameObject toggleObj, Toggle toggle)
			: base(toggleObj)
		{
			if (toggle == null)
				throw new ArgumentNullException("toggle");

			this.toggle = toggle;
		}

		public bool Value
		{
			get { return this.toggle.isOn; }
			set { this.toggle.isOn = value; }
		}

		public float Alpha
		{
			get { return this.toggle.image.color.a; }
			set { SetAlpha(value); }
		}

		private void SetAlpha(float alpha)
		{
			Color c = this.toggle.image.color;
			c.a = alpha;
			this.toggle.image.color = c;
		}
	}

	public class ToggleDecorator : IToggle
	{
		private readonly IToggle toggle;
		private readonly Action<bool> onToggleChanged;

		public ToggleDecorator(IToggle toggle,
			Action<bool> onToggleChanged)
		{
			if (toggle == null)
				throw new ArgumentNullException("toggle");
			if (onToggleChanged == null)
				throw new ArgumentNullException("onToggleChanged");

			this.toggle = toggle;
			this.onToggleChanged = onToggleChanged;
		}

		public bool Value
		{
			get { return this.toggle.Value; }
			set
			{
				if (this.toggle.Value == value)
					return;

				this.toggle.Value = value;
				this.onToggleChanged(value);
			}
		}

		public bool IsVisible
		{
			get { return this.toggle.IsVisible; }
			set { this.toggle.IsVisible = value; }
		}

		public float Alpha
		{
			get { return this.toggle.Alpha; }
			set { this.toggle.Alpha = value; }
		}
	}

	public class ToggleNavItem : NavItem
	{
		public ToggleNavItem(IToggle toggle)
			: base(() => toggle.Value = !toggle.Value)
		{
			if (toggle == null)
				throw new ArgumentNullException("toggle");
		}
	}
}