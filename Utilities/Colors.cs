
using UnityEngine;

namespace Entity
{
	public static class Colors
	{
		public static Color Hex(string hex)
		{
			Color color;
			if (ColorUtility.TryParseHtmlString("#" + hex, out color))
				return color;

			return Color.white;
		}
	}
}