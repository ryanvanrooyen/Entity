
using UnityEngine;
using System;

namespace Entity
{
	public class Line
	{
		public static void Draw(Vector3[] points,
			Delegates.Action<Vector3, Vector3> drawLine,
			Action<Vector3> onPoint = null)
		{
			for (var i = 0; i < points.Length - 1; i++)
			{
				if (onPoint != null)
					onPoint(points[i]);

				drawLine(points[i], points[i + 1]);
			}

			if (onPoint != null && points.Length > 0)
				onPoint(points[points.Length - 1]);
		}

		public static Vector3[] Curve(Vector3 p1, Vector3 p2,
			Vector3 p3, int numberOfPoints)
		{
			var points = new Vector3[numberOfPoints];
			float t, oneMinusT;

			for (var i = 0; i < points.Length; i++)
			{
				t = (float)i / (float)(points.Length - 1);
				oneMinusT = 1 - t;

				points[i] = ((oneMinusT * oneMinusT) * p1) +
					(2 * t * oneMinusT * p2) + ((t * t) * p3);
			}

			return points;
		}

		public static Vector3[] Arc(Vector3 from, Vector3 center,
			Vector3 normal, float angle)
		{
			if (angle < 0)
			{
				angle = angle * -1;
				normal = normal * -1;
			}

			var degreeSteps = 0.5f;
			var points = new Vector3[(int)Mathf.Round(angle / degreeSteps) + 1];

			if (points.Length <= 1)
				return points;

			var rot = Quaternion.AngleAxis(degreeSteps, normal);
			var radius = from - center;
			points[0] = radius;

			for (var i = 1; i < points.Length; i++)
			{
				var prevPoint = points[i - 1];
				points[i] = (rot * prevPoint);
			}

			for (var i = 0; i < points.Length; i++)
				points[i] = center + points[i];

			return points;
		}

		public static Vector3[] Arc(Vector3 from, Vector3 to, Vector3 center)
		{
			var fromDist = from - center;
			var toDist = to - center;

			var normal = Vector3.Cross(fromDist, toDist);
			var angle = Vector3.Angle(fromDist, toDist);

			return Arc(from, center, normal, angle);
		}
	}
}