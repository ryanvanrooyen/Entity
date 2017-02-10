
using System;
using UnityEngine;
using UnityEngine.UI;

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
		private readonly CanvasScaler scaler;
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

			this.canvas = this.gameObj.AddComponent<UnityEngine.Canvas>();
			//this.canvas.pixelPerfect = true;
			
			this.scaler = this.gameObj.AddComponent<CanvasScaler>();
			this.scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			this.scaler.referenceResolution = new Vector2(1920, 1080);

			this.rect = this.gameObj.GetComponent<RectTransform>();
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

		protected Vector2? GetCanvasPosition(Vector3 point)
		{
			var screenPoint = GetScreenPos(point);

			var screenX = screenPoint.x;
			var screenY = screenPoint.y;
			var screenZ = screenPoint.z;
			//var screenZ = 0;

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

			return new Vector2(screenX, screenY) - this.rect.sizeDelta / 2;
		}

		protected Vector2 GetClampedCanvasPositionCircle(
			Vector3 point, Vector2 screenPadding, out bool isClamped)
		{
			return GetClampedCanvasPosition(point, screenPadding,
				out isClamped, GetMaxRadialScreenDist);
		}

		protected Vector2 GetClampedCanvasPositionEllipse(
			Vector3 point, Vector2 screenPadding, out bool isClamped)
		{
			return GetClampedCanvasPosition(point, screenPadding,
				out isClamped, GetMaxEllipseScreenDist);
		}

		private Vector2 GetClampedCanvasPosition(Vector3 point, Vector2 screenPadding,
			out bool isClamped, Func<Vector2, Vector2, float> maxDistFunc)
		{
			isClamped = false;

			var screenPoint = GetScreenPos(point);

			var screenX = screenPoint.x;
			var screenY = screenPoint.y;
			var screenZ = screenPoint.z;
			//var screenZ = 0;

			// If the point is behind the camera, adjust the x/y coordinates.
			if (screenZ < 0)
			{
				screenX = -screenX;
				screenY = -screenY;
			}

			var uiPoint = new Vector2(screenX, screenY) - this.rect.sizeDelta / 2;

			var maxDist = maxDistFunc(screenPadding, uiPoint);

			if (uiPoint.magnitude > maxDist)
			{
				isClamped = true;
				return uiPoint.normalized * maxDist;
			}

			return uiPoint;
		}

#pragma warning disable RECS0154 // Parameter is never used

		private float GetMaxRadialScreenDist(Vector2 screenPadding, Vector2 screenPoint)
#pragma warning restore RECS0154 // Parameter is never used
		{
			var width = (this.rect.sizeDelta.x / 2) - screenPadding.x;
			var height = (this.rect.sizeDelta.y / 2) - screenPadding.y;

			if (width < height)
				return width;

			return height;
		}

		private float GetMaxEllipseScreenDist(Vector2 screenPadding, Vector2 screenPoint)
		{
			var width = (this.rect.sizeDelta.x / 2) - screenPadding.x;
			var height = (this.rect.sizeDelta.y / 2) - screenPadding.y;

			// See: http://mathworld.wolfram.com/Ellipse-LineIntersection.html
			// For more information on calculating the point intersecting an ellipse
			var f1 = width * width * screenPoint.y * screenPoint.y;
			var f2 = height * height * screenPoint.x * screenPoint.x;
			var mainEquationFactor = (width * height) / Mathf.Sqrt(f1 + f2);

			var ellipsisX = mainEquationFactor * screenPoint.x;
			var ellipsisY = mainEquationFactor * screenPoint.y;

			var ellipsisPoint = new Vector2(ellipsisX, ellipsisY);
			return ellipsisPoint.magnitude;
		}

		private Vector3 GetScreenPos(Vector3 worldPos)
		{
			var screenPos = this.camera.WorldToScreenPoint(worldPos);

			var scaleFactorX = this.scaler.referenceResolution.x / Screen.width;
			var scaleFactorY = this.scaler.referenceResolution.y / Screen.height;

			var scaledPos = new Vector3(screenPos.x * scaleFactorX, screenPos.y * scaleFactorY, screenPos.z);

			return scaledPos;
		}
	}
}