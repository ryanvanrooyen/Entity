
using UnityEngine;

namespace Entity
{
	public static class CanvasExtensions
	{
		public static GameObject Add(this IWorldCanvas canvas, string prefab,
			GameObject trackedObj, Vector3 offset, float? offscreenIconDistance = null, Vector2? offscreenIconOffset = null)
		{
			GameObject obj = null;
			obj = canvas.Add(prefab, () =>
			{
				if (trackedObj == null)
				{
					Object.Destroy(obj);
					return null;
				}

				return trackedObj.transform.position + offset;

			}, offscreenIconDistance, offscreenIconOffset);

			return obj;
		}
	}
}