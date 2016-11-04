
using System.Collections.Generic;

namespace Entity
{
	public static class MathHelpers
	{
		public static float Map(float x, float in_min,
			float in_max, float out_min, float out_max)
		{
			if (x < in_min)
				return out_min;
			if (x > in_max)
				x = in_max;

			return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
		}

		public static float Total(IEnumerable<float> items)
		{
			var total = 0f;
			foreach (var item in items)
				total += item;
			return total;
		}
	}
}