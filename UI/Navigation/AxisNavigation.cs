
using System;

namespace Entity
{
	public class AxisNavigation : Navigation<IAxis>
	{
		public const float DebounceThreshold = 0.7f;
		public static readonly TimeSpan DebounceTime = TimeSpan.FromSeconds(0.4f);

		private readonly IAxis axis;

		public AxisNavigation(IButton selectButton,
			IButton cancelButton, IAxis axis, ISound2d onMoveSound,
			INavEvents events, INavItemSource itemSource,
			bool childNavsCaptureInput = true, bool autoResolveFocus = false)
			: base(selectButton, cancelButton, itemSource, onMoveSound, events,
				childNavsCaptureInput, autoResolveFocus)
		{
			if (axis == null)
				throw new ArgumentNullException("axis");

			this.axis = axis.Debounce(DebounceThreshold, DebounceTime);
			
			this.PreviousInput = this.axis;
			this.NextInput = this.axis;
		}

		protected override void CheckInput()
		{
			var value = this.axis.Value;
			if (value < (-DebounceThreshold))
				MoveNext();
			else if (value > DebounceThreshold)
				MovePrevious();
		}
	}

	public class HorizontalNavigation : AxisNavigation
	{
		public HorizontalNavigation(IButton selectButton, IButton cancelButton,
			IDualAxis axis, IFaceButtons dpad, ISound2d onMoveSound,
			INavEvents events, INavItemSource itemSource,
			bool childNavsCaptureInput = true, bool autoResolveFocus = false)
			: base(selectButton, cancelButton,
				axis.Horizontal.Invert().AddButtonInput(dpad.Left, dpad.Right),
				onMoveSound, events, itemSource, childNavsCaptureInput, autoResolveFocus)
		{ }
	}

	public class VerticalNavigation : AxisNavigation
	{
		public VerticalNavigation(IButton selectButton, IButton cancelButton,
			IDualAxis axis, IFaceButtons dpad, ISound2d onMoveSound,
			INavEvents events, INavItemSource itemSource,
			bool childNavsCaptureInput = true, bool autoResolveFocus = false)
			: base(selectButton, cancelButton,
				axis.Vertical.AddButtonInput(dpad.Top, dpad.Bottom),
				onMoveSound, events, itemSource, childNavsCaptureInput, autoResolveFocus)
		{ }
	}

	public class GridNavigation : Navigation<IAxis>
	{
		private const float threshold = 0.7f;
		private readonly IAxis rowAxis;
		private readonly IAxis columnAxis;
		private readonly int itemsPerRow;

		public GridNavigation(IButton selectButton, IButton cancelButton,
			IDualAxis axis, IFaceButtons dpad, int itemsPerRow, ISound2d onMoveSound,
			INavEvents events, INavItemSource itemSource,
			bool childNavsCaptureInput = true, bool autoResolveFocus = false)
			: base(selectButton, cancelButton, itemSource, onMoveSound, events,
				childNavsCaptureInput, autoResolveFocus)
		{
			if (axis == null)
				throw new ArgumentNullException("axis");
			if (dpad == null)
				throw new ArgumentNullException("dpad");
			
			this.rowAxis = axis.Horizontal.Invert()
				.AddButtonInput(dpad.Left, dpad.Right)
				.Debounce(AxisNavigation.DebounceThreshold, AxisNavigation.DebounceTime);
			
			this.columnAxis = axis.Vertical
				.AddButtonInput(dpad.Top, dpad.Bottom)
				.Debounce(AxisNavigation.DebounceThreshold, AxisNavigation.DebounceTime);

			this.itemsPerRow = itemsPerRow;
			this.PreviousInput = this.rowAxis;
			this.NextInput = this.rowAxis;
		}

		protected override void CheckInput()
		{
			var value = this.rowAxis.Value;
			if (value < (-threshold))
				MoveNext();
			else if (value > threshold)
				MovePrevious();

			value = this.columnAxis.Value;
			if (value < (-threshold))
				NextColumn();
			else if (value > threshold)
				PreviousColumn();
		}

		private void NextColumn()
		{
			for (var i = 0; i < this.itemsPerRow; i++)
				MoveNext();
		}

		private void PreviousColumn()
		{
			for (var i = 0; i < this.itemsPerRow; i++)
				MovePrevious();
		}
	}

	public class AxisIconNavigation : IconNavigation<IAxis>
	{
		public AxisIconNavigation(INavigation<IAxis> navigation,
			IImage selectIcon, IImage cancelIcon, IImage nextIcon, IImage previousIcon)
			: base(navigation, selectIcon, cancelIcon, nextIcon, previousIcon)
		{
		}

		public override void CheckNavigation()
		{
			if (!this.HasFocus)
				return;

			this.selectIcon.Source = this.SelectButton;
			this.cancelIcon.Source = this.CancelButton;

			var axis = this.NextInput;
			this.nextIcon.Source = axis.NextIcon;
			this.nextIcon.Alpha = 0.1f;

			this.previousIcon.Source = axis.PreviousIcon;
			this.previousIcon.Alpha = 0.1f;

			this.nextIcon.IsVisible = this.HasNext;
			this.previousIcon.IsVisible = this.HasPrevious;

			//var value = axis.Value;
			//this.nextIcon.Alpha = GetAlpha(value, 1);
			//this.previousIcon.Alpha = GetAlpha(value, -1);

			this.navigation.CheckNavigation();
		}

		private float GetAlpha(float value, float max)
		{
			if (max < 0)
			{
				max = max * -1;
				value = value * -1;
			}

			if (value < 0)
				value = 0;

			return Map(value, 0, max, 0.2f, 1f);
		}

		private float Map(float x, float in_min, float in_max, float out_min, float out_max)
		{
			return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
		}
	}
}
