
using UnityEngine;

namespace Entity
{
	public interface ICanvas : IVisible
	{
		GameObject Add(string prefab, GameObject parent = null);
	}

	public abstract class Canvas : ICanvas
	{
		private static GameObject parent;
		private readonly IObjects gameObjFactory;
		private readonly IVisible visible;
		protected readonly Camera camera;
		protected readonly RectTransform rect;
		protected readonly GameObject gameObj;
		protected readonly UnityEngine.Canvas canvas;

		public Canvas(string title, Camera camera, IObjects gameObjFactory)
		{
			if (parent == null)
				parent = gameObjFactory.New("UI");

			this.camera = camera;
			this.gameObjFactory = gameObjFactory;
			this.gameObj = this.gameObjFactory.New(title, parent);
			this.visible = new Visible(this.gameObj, true);

			this.rect = this.gameObj.AddComponent<RectTransform>();
			this.canvas = this.gameObj.AddComponent<UnityEngine.Canvas>();
			this.canvas.pixelPerfect = false;
		}

		public bool IsVisible
		{
			get { return this.visible.IsVisible; }
			set { this.visible.IsVisible = value; }
		}

		public GameObject Add(string prefab, GameObject parent = null)
		{
			if (string.IsNullOrEmpty(prefab))
				return null;

			var obj = this.gameObjFactory.Create(prefab, this.gameObj);

			var rectTransform = obj.GetComponent<RectTransform>();

			RectTransform parentRect = null;
			if (parent != null)
				parentRect = parent.GetComponent<RectTransform>();
			if (parentRect == null)
				parentRect = this.rect;

			rectTransform.SetParent(parentRect, false);

			return obj;
		}

		protected Vector2? GetCanvasPosition(Vector3 point,
			float? offscreenIconDistance, out bool drawOffscreenIcon)
		{
			drawOffscreenIcon = false;
			var screenPoint = this.camera.WorldToScreenPoint(point);
			var screenX = screenPoint.x;
			var screenY = screenPoint.y;
			var screenZ = screenPoint.z;

			if (offscreenIconDistance != null)
			{
				// If the point is behind the camera, adjust the x/y coordinates.
				if (screenZ < 0)
				{
					screenX = -screenX;
					screenY = -screenY;
				}

				var uiPosition = new Vector2(screenX, screenY) - this.rect.sizeDelta / 2f;
				var radialDistance = GetRadialScreenSize(50);
				if (uiPosition.magnitude > radialDistance)
				{
					drawOffscreenIcon = true;
					return uiPosition.normalized * radialDistance;
				}

				return uiPosition;
			}
			else
			{
				// If the point is behind the camera, return null.
				if (screenZ < 0)
					return null;

				// OffscreenMargin is how far off screen the UI can go before
				// it's considered completely not visible by the camera.
				var offscreenMargin = 200;

				if (screenX > screenX + offscreenMargin)
					return null;
				if (screenY > screenY + offscreenMargin)
					return null;

				return new Vector2(screenX, screenY) - this.rect.sizeDelta / 2f;
			}
		}

		private float GetRadialScreenSize(float padding)
		{
			var screenWidth = this.rect.sizeDelta.x / 2;
			var screenHeight = this.rect.sizeDelta.y / 2;

			var screenSize = screenHeight;
			if (screenWidth < screenSize)
				screenSize = screenWidth;

			return screenSize - padding;
		}
	}
}