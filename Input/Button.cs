
using UnityEngine;
using System;

namespace Entity
{
	public interface IPressable
	{
		bool IsPressed { get; }
		bool WasPressed { get; }
	}

	public interface IButtonInput : IPressable
	{
		string Name { get; }
	}

	public interface IButton : IPressable, IImgSource, IEquatable<IButton>
	{
		bool Enabled { get; set; }
		IButton New();
		IButton Debounce(TimeSpan debounceTime);
	}

	public interface IInputName
	{
		string Name { get; }
		string Label { get; }
	}

	public class InputName : IInputName
	{
		public InputName(string name, string label)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (name == string.Empty)
				throw new ArgumentException("Input \"name\" cannot be the empty string");
			if (label == null)
				throw new ArgumentNullException("label");
			if (label == string.Empty)
				throw new ArgumentException("Input \"label\" cannot be the empty string");

			this.Name = name;
			this.Label = label;
		}

		public string Name { get; private set; }
		public string Label { get; private set; }
	}

	public class ButtonInput : IButtonInput
	{
		private readonly string inputName;
		private readonly string label;

		public ButtonInput(IInputName inputName)
		{
			if (inputName == null)
				throw new ArgumentNullException("inputName");

			this.inputName = inputName.Name;
			this.label = inputName.Label;
		}

		public string Name
		{
			get { return this.inputName; }
		}

		public bool IsPressed
		{
			get { return Input.GetKey(this.inputName); }
		}

		public bool WasPressed
		{
			get { return Input.GetKeyUp(this.inputName); }
		}

		public override string ToString()
		{
			return this.label;
		}
	}

	public class Button : IButton
	{
		private readonly IButtonInput input;
		private readonly IButtonIcon icon;

		public Button(IButtonInput input, IButtonIcon icon)
		{
			if (input == null)
				throw new ArgumentNullException("input");
			if (icon == null)
				throw new ArgumentNullException("icons");

			this.input = input;
			this.icon = icon;
			this.Enabled = true;
		}

		public bool IsPressed
		{
			get { return this.Enabled && this.input.IsPressed; }
		}

		public bool WasPressed
		{
			get { return this.Enabled && this.input.WasPressed; }
		}

		public bool Enabled { get; set; }

		public Sprite Image
		{
			get
			{
				if (!this.Enabled)
					return this.icon.Disabled;

				return this.input.IsPressed ?
					this.icon.Pressed : this.icon.Default;
			}
		}

		public IButton New()
		{
			return new Button(this.input, this.icon);
		}

		public IButton Debounce(TimeSpan debounceTime)
		{
			return new DebouncedButton(this, debounceTime);
		}

		public bool Equals(IButton button)
		{
			if (button == null)
				return false;

			var other = button as Button;
			if (other == null)
				return button.Equals(this);

			return string.Equals(
				this.input.Name, other.input.Name,
				StringComparison.OrdinalIgnoreCase);
		}

		public override string ToString()
		{
			return this.input.ToString();
		}
	}

	public class ButtonCombo : IButton
	{
		private readonly IButton[] buttons;

		public ButtonCombo(params IButton[] buttons)
		{
			if (buttons.Length < 2)
				throw new ArgumentException("a button combo must have at least 2 buttons");

			this.buttons = buttons;
			this.Enabled = true;
		}

		public bool Enabled { get; set; }

		public bool IsPressed
		{
			get { return this.Enabled && this.buttons.All(b => b.IsPressed); }
		}

		public bool WasPressed
		{
			get { return this.Enabled && this.buttons.All(b => b.WasPressed); }
		}

		public Sprite Image
		{
			get { return null; }
		}

		public override string ToString()
		{
			return ConcatNames(this.buttons);
		}

		private string ConcatNames(object[] items)
		{
			var str = string.Empty;
			foreach (var item in items)
			{
				str += item.ToString() + "+";
			}

			return str;
		}

		public IButton New()
		{
			return new ButtonCombo(this.buttons);
		}

		public IButton Debounce(TimeSpan debounceTime)
		{
			return new DebouncedButton(this, debounceTime);
		}

		public bool Equals(IButton button)
		{
			if (button == null)
				return false;

			var other = button as ButtonCombo;
			if (other == null)
				return false;

			if (this.buttons.Length != other.buttons.Length)
				return false;

			for (var i = 0; i < this.buttons.Length; i++)
			{
				if (!this.buttons[i].Equals(other.buttons[i]))
					return false;
			}

			return true;
		}
	}

	public abstract class ButtonDecorator : IButton
	{
		protected readonly IButton button;

		public ButtonDecorator(IButton button)
		{
			if (button == null)
				throw new ArgumentNullException("button");

			this.button = button;
		}

		public bool Enabled
		{
			get { return this.button.Enabled; }
			set { this.button.Enabled = value; }
		}

		public Sprite Image
		{
			get { return this.button.Image; }
		}

		public virtual bool IsPressed
		{
			get { return this.button.IsPressed; }
		}

		public virtual bool WasPressed
		{
			get { return this.button.WasPressed; }
		}

		public IButton Debounce(TimeSpan debounceTime)
		{
			return this.button.Debounce(debounceTime);
		}

		public virtual bool Equals(IButton button)
		{
			return this.button.Equals(button);
		}

		public abstract IButton New();

		public override string ToString()
		{
			return this.button.ToString();
		}
	}

	public class DebouncedButton : ButtonDecorator
	{
		private readonly TimeSpan debounceTime;
		private DateTime timeLastPressed;

		public DebouncedButton(IButton button, TimeSpan debounceTime)
			: base(button)
		{
			this.debounceTime = debounceTime;
			this.timeLastPressed = DateTime.MinValue;
		}

		public override bool WasPressed
		{
			get
			{
				if (this.button.WasPressed)
				{
					var timeSinceLastPressed = DateTime.Now - this.timeLastPressed;
					if (timeSinceLastPressed > this.debounceTime)
					{
						this.timeLastPressed = DateTime.Now;
						return true;
					}
				}

				return false;
			}
		}

		public override IButton New()
		{
			return new DebouncedButton(this.button, this.debounceTime);
		}
	}

	public class NullButton : IButton
	{
		public string Name { get { return "null"; } }
		public bool IsPressed { get { return false; } }
		public bool WasPressed { get { return false; } }
		public bool Enabled { get; set; }
		public Sprite Image { get { return null; } }

		public IButton New()
		{
			return this;
		}

		public IButton Debounce(TimeSpan debounceTime)
		{
			return this;
		}

		public override string ToString()
		{
			return "(No Button)";
		}

		public bool Equals(IButton button)
		{
			return false;
		}
	}

	public class ProxyButton : IButton
	{
		private static int count = 0;
		private readonly int id;

		public ProxyButton()
		{
			count++;
			this.id = count;
		}

		public string Name
		{
			get { return string.Format("Proxy{0}", this.id); }
		}

		public Delegates.Func<bool> IsPressedProxy { get; set; }
		public bool IsPressed
		{
			get
			{
				if (this.IsPressedProxy != null)
					return this.Enabled && this.IsPressedProxy();
				return false;
			}
		}

		public Delegates.Func<bool> WasPressedProxy { get; set; }
		public bool WasPressed
		{
			get
			{
				if (this.WasPressedProxy != null)
					return this.Enabled && this.WasPressedProxy();
				return false;
			}
		}

		public bool Enabled { get; set; }
		public Sprite Image { get; private set; }

		public IButton New()
		{
			var button = new ProxyButton();
			button.IsPressedProxy = this.IsPressedProxy;
			button.WasPressedProxy = this.WasPressedProxy;
			return button;
		}

		public bool Equals(IButton button)
		{
			return false;
		}

		public IButton Debounce(TimeSpan debounceTime)
		{
			return this;
		}

		public override string ToString()
		{
			return "ProxyButton";
		}
	}
}