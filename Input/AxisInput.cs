
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
		IAxis Invert();
		IImgSource NextIcon { get; }
		IImgSource PreviousIcon { get; }
		IAxis Debounce(float maxThreshold);
	}

	public interface IButtonAxisInput : IAxisInput, IButtonInput
	{
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
				throw new ArgumentNullException("axisName");
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

		public IAxis Debounce(float maxThreshold)
		{
			return new Axis(new DebouncedAxisInput(
				this.axis, maxThreshold), this.axisIcon);
		}

		public override string ToString()
		{
			return this.axis.ToString();
		}
	}

	public class ButtonAxisInput : IButtonAxisInput
	{
		private readonly IAxisInput axis;
		private bool wasPressed = false;
		private int frameCount = 0;
		private float lastValue = 0f;
		private float threshold = 0f;

		public ButtonAxisInput(IAxisInput axis, float threshold)
		{
			if (axis == null)
				throw new ArgumentNullException("axisName");

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

	public class InvertedAxisInput : IAxisInput
	{
		private readonly IAxisInput axis;

		public InvertedAxisInput(IAxisInput axis)
		{
			if (axis == null)
				throw new ArgumentNullException("axisName");

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
		private bool hasPassedThreshold;

		public DebouncedAxisInput(IAxisInput axis, float threshold)
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
			get
			{
				var value = this.axis.Value;
				var absValue = Mathf.Abs(value);
				if (absValue > this.threshold && this.hasPassedThreshold)
					return 0;

				this.hasPassedThreshold = absValue > this.threshold;
				return value;
			}
		}

		public override string ToString()
		{
			return this.axis.ToString();
		}
	}
}