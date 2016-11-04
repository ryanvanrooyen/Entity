
using UnityEngine;
using System;

namespace Entity
{
	public interface ITriggerInput
	{
		float Value { get; }
	}

	public interface ITrigger : ITriggerInput, IImgSource
	{
	}

	public class TriggerInput : ITriggerInput
	{
		private readonly string inputName;
		private readonly string label;

		public TriggerInput(IInputName inputName)
		{
			if (inputName == null)
				throw new ArgumentNullException("inputName");

			this.inputName = inputName.Name;
			this.label = inputName.Label;
		}

		public float Value
		{
			get
			{
				var trigger = Input.GetAxis(this.inputName);
				return trigger;
			}
		}

		public override string ToString()
		{
			return this.label;
		}
	}

	public class Trigger : ITrigger
	{
		private readonly ITriggerInput input;
		private readonly IButtonIcon icons;

		public Trigger(ITriggerInput input, IButtonIcon icons)
		{
			if (input == null)
				throw new ArgumentNullException("input");
			if (icons == null)
				throw new ArgumentNullException("icons");

			this.input = input;
			this.icons = icons;
		}

		public float Value
		{
			get { return this.input.Value; }
		}

		public Sprite Image
		{
			get
			{
				return this.input.Value > 0.5f ?
				  this.icons.Pressed : this.icons.Default;
			}
		}

		public override string ToString()
		{
			return this.input.ToString();
		}
	}
}