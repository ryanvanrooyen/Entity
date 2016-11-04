
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

namespace Entity
{
	public interface ISlider : IVisible
	{
		float Value { get; set; }
		float MinValue { get; set; }
		float MaxValue { get; set; }
	}

	public class SliderComp : Visible, ISlider
	{
		private readonly Slider slider;

		public SliderComp(GameObject sliderObj)
			: this(sliderObj, sliderObj.GetComponent<Slider>())
		{
		}

		public SliderComp(Slider slider) : this(slider.gameObject, slider)
		{
		}

		private SliderComp(GameObject sliderObj, Slider slider) : base(sliderObj)
		{
			if (slider == null)
				throw new ArgumentNullException("slider");

			this.slider = slider;
		}

		public float Value
		{
			get { return this.slider.value; }
			set { this.slider.value = value; }
		}

		public float MinValue
		{
			get { return this.slider.minValue; }
			set { this.slider.minValue = value; }
		}

		public float MaxValue
		{
			get { return this.slider.maxValue; }
			set { this.slider.maxValue = value; }
		}
	}

	public interface IAnimatedSlider : ISlider
	{
		bool UseAnimation { get; set; }
	}

	public class AnimatedSlider : IAnimatedSlider
	{
		private readonly GameObject coroutineSrc;
		private readonly ISlider slider;
		private float value;

		public AnimatedSlider(GameObject coroutineSrc, ISlider slider)
		{
			this.coroutineSrc = coroutineSrc;
			this.slider = slider;
			this.value = this.slider.Value;
			this.UseAnimation = true;
		}

		public bool UseAnimation { get; set; }

		public float Value
		{
			get { return this.value; }
			set
			{
				this.value = value;
				if (this.IsVisible && this.UseAnimation)
					this.coroutineSrc.Run(UpdateSliderValue());
				else
					this.slider.Value = this.value;
			}
		}

		public float MinValue
		{
			get { return this.slider.MinValue; }
			set
			{
				this.slider.MinValue = value;
				this.value = this.slider.Value;
			}
		}

		public float MaxValue
		{
			get { return this.slider.MaxValue; }
			set
			{
				this.slider.MaxValue = value;
				this.value = this.slider.Value;
			}
		}

		public bool IsVisible
		{
			get { return this.slider.IsVisible; }
			set { this.slider.IsVisible = value; }
		}

		private IEnumerator UpdateSliderValue()
		{
			var increment = Mathf.Abs(this.slider.MaxValue) / 100;

			while (this.slider != null && this.value != this.slider.Value)
			{
				if (Mathf.Abs(this.value - this.slider.Value) < increment * 1.5f)
				{
					this.slider.Value = this.value;
					break;
				}

				increment = Mathf.Abs(increment);
				if (this.value < this.slider.Value && increment > 0)
					increment = -increment;

				this.slider.Value = this.slider.Value + increment;
				yield return null;
			}
		}
	}

	public class SliderWithText : ISlider
	{
		private readonly ISlider slider;
		private readonly IText text;
		private readonly Delegates.Func<ISlider, string> formatter;

		public SliderWithText(ISlider slider, IText text,
			Delegates.Func<ISlider, string> formatter)
		{
			if (slider == null)
				throw new ArgumentNullException("slider");
			if (text == null)
				throw new ArgumentNullException("text");
			if (formatter == null)
				throw new ArgumentNullException("formatter");

			this.slider = slider;
			this.text = text;
			this.formatter = formatter;
			this.text.Value = this.formatter(this.slider);
		}

		public float Value
		{
			get { return this.slider.Value; }
			set
			{
				this.slider.Value = value;
				this.text.Value = this.formatter(this.slider);
			}
		}

		public float MinValue
		{
			get { return this.slider.MinValue; }
			set
			{
				this.slider.MinValue = value;
				this.text.Value = this.formatter(this.slider);
			}
		}

		public float MaxValue
		{
			get { return this.slider.MaxValue; }
			set
			{
				this.slider.MaxValue = value;
				this.text.Value = this.formatter(this.slider);
			}
		}

		public bool IsVisible
		{
			get { return this.slider.IsVisible; }
			set { this.slider.IsVisible = value; }
		}
	}
}