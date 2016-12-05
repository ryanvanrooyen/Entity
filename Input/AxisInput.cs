
using UnityEngine;
using System;

namespace Entity
{
	public interface IAxisValue
	{
		float Value { get; }
	}

	public interface IAxisInput : IAxisValue
	{
		string Name { get; }
	}

	public interface IAxis : IAxisValue, IImgSource
	{
		IImgSource NextIcon { get; }
		IImgSource PreviousIcon { get; }

		IAxis Invert();
		IAxis Debounce(float maxThreshold, TimeSpan? debounceTime = null);
		IAxis AddButtonInput(IButton posButton, IButton negativeButton);
	}

	public class AxisInput : IAxisInput
	{
		private readonly string axisName;
		private readonly string label;

		public AxisInput(IInputName axisName)
		{
			if (axisName == null)
				throw new ArgumentNullException("axisName");

			this.axisName = axisName.Name;
			this.label = axisName.Label;
		}

		public string Name
		{
			get { return this.axisName; }
		}

		public float Value
		{
			get { return Input.GetAxis(this.axisName); }
		}

		public override string ToString()
		{
			return this.label;
		}
	}

	public class Axis : IAxis
	{
		private readonly IAxisInput axis;
		private readonly IAxisIcon axisIcon;

		public Axis(IAxisInput axis, IAxisIcon axisIcon)
		{
			if (axis == null)
				throw new ArgumentNullException("axis");
			if (axisIcon == null)
				throw new ArgumentNullException("axisIcon");

			this.axis = axis;
			this.axisIcon = axisIcon;
		}

		public IImgSource NextIcon
		{
			get { return new ImgSource(this.axisIcon.PositiveIcon); }
		}

		public IImgSource PreviousIcon
		{
			get { return new ImgSource(this.axisIcon.NegativeIcon); }
		}

		public float Value
		{
			get { return this.axis.Value; }
		}

		public Sprite Image
		{
			get
			{
				//if (this.Value > 0.5f)
				//    return this.axisIcon.PositiveIcon;
				//else if (this.Value < -0.5f)
				//    return this.axisIcon.NegativeIcon;
				//else
				//    return this.axisIcon.DefaultIcon;

				return this.axisIcon.DefaultIcon;
			}
		}

		public IAxis Invert()
		{
			return new Axis(new InvertedAxisInput(this.axis), this.axisIcon);
		}

		public IAxis Debounce(float maxThreshold, TimeSpan? debounceTime = null)
		{
			return new Axis(new DebouncedAxisInput(
				this.axis, maxThreshold, debounceTime), this.axisIcon);
		}

		public IAxis AddButtonInput(IButton posButton, IButton negativeButton)
		{
			return new Axis(new CompositeAxisInput(this.axis,
				posButton.ToAxisInput(1), negativeButton.ToAxisInput(-1)), this.axisIcon);
		}

		public override string ToString()
		{
			return this.axis.ToString();
		}
	}

	public class AxisButtonInput : IButtonInput
	{
		private readonly IAxisInput axis;
		private bool wasPressed = false;
		private int frameCount = 0;
		private float lastValue = 0f;
		private float threshold = 0f;

		public AxisButtonInput(IAxisInput axis, float threshold)
		{
			if (axis == null)
				throw new ArgumentNullException("axis");

			this.axis = axis;
			this.threshold = threshold;
		}

		public string Name
		{
			get { return this.axis.Name; }
		}

		public float Value
		{
			get { return this.axis.Value; }
		}

		public bool IsPressed
		{
			get { return IsGreaterThanThreashold(this.Value); }
		}

		public bool WasPressed
		{
			get
			{
				var currentFrame = Time.frameCount;
				if (currentFrame == this.frameCount)
					return this.wasPressed;

				this.wasPressed = !this.IsPressed &&
					IsGreaterThanThreashold(this.lastValue);

				this.lastValue = this.Value;
				this.frameCount = currentFrame;

				return this.wasPressed;
			}
		}

		private bool IsGreaterThanThreashold(float value)
		{
			if (this.threshold >= 0)
				return value > this.threshold;
			else
				return value < this.threshold;
		}

		public override string ToString()
		{
			return this.axis.ToString();
		}
	}

	public class ButtonAxisInput : IAxisInput
	{
		private readonly IButtonInput button;
		private readonly float pressedAxisValue;

		public ButtonAxisInput(IButtonInput button, float pressedAxisValue = 1f)
		{
			if (button == null)
				throw new ArgumentNullException("button");

			this.button = button;
			this.pressedAxisValue = pressedAxisValue;
		}

		public string Name
		{
			get { return this.button.Name; }
		}

		public float Value
		{
			get { return this.button.IsPressed ? this.pressedAxisValue : 0; }
		}

		public override string ToString()
		{
			return this.button.ToString();
		}
	}

	public class InvertedAxisInput : IAxisInput
	{
		private readonly IAxisInput axis;

		public InvertedAxisInput(IAxisInput axis)
		{
			if (axis == null)
				throw new ArgumentNullException("axis");

			this.axis = axis;
		}

		public string Name
		{
			get { return this.axis.Name; }
		}

		public float Value
		{
			get { return this.axis.Value * -1; }
		}

		public override string ToString()
		{
			return this.axis.ToString();
		}
	}

	public class DebouncedAxisInput : IAxisInput
	{
		private readonly IAxisInput axis;
		private float threshold;
		private TimeSpan debounceTime;
		private bool hasPassedThreshold;
		private DateTime lastThresholdPassed = DateTime.Now;

		public DebouncedAxisInput(IAxisInput axis,
			float threshold, TimeSpan? debounceTime = null)
		{
			if (axis == null)
				throw new ArgumentNullException("axis");

			this.axis = axis;
			this.threshold = threshold;
			this.debounceTime = debounceTime ?? TimeSpan.MaxValue;
		}

		public string Name
		{
			get { return this.axis.Name; }
		}

		public float Value
		{
			get
			{
				var axisValue = this.axis.Value;
				var absValue = Mathf.Abs(axisValue);
				var timeSinceLastThreshold = DateTime.Now - this.lastThresholdPassed;

				if (absValue > this.threshold && this.hasPassedThreshold &&
				    timeSinceLastThreshold < this.debounceTime)
					return 0;

				this.hasPassedThreshold = absValue > this.threshold;
				if (this.hasPassedThreshold)
					this.lastThresholdPassed = DateTime.Now;
				
				return axisValue;
			}
		}

		public override string ToString()
		{
			return this.axis.ToString();
		}
	}

	public class CompositeAxisInput : IAxisInput
	{
		private readonly IAxisInput[] axises;

		public CompositeAxisInput(params IAxisInput[] axises)
		{
			if (axises == null)
				throw new ArgumentNullException("axises");

			this.axises = axises.Where(axis => axis != null);
		}

		public string Name
		{
			get
			{
				var name = "CompositeAxis(";
				if (this.axises.Length > 0)
				{
					name += this.axises[0].Name;
					for (var i = 1; i < this.axises.Length; i++)
						name += ", or " + this.axises[i].Name;
				}

				return name + ")";
			}
		}

		public float Value
		{
			get
			{
				// Return the first axis with a non 0 value.
				for (var i = 0; i < this.axises.Length; i++)
				{
					var axisValue = this.axises[i].Value;
					if (Mathf.Abs(axisValue) > 0)
						return axisValue;
				}

				return 0;
			}
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}