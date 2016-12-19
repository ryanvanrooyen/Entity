
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
	public interface IScreenCanvas : ICanvas
	{ }

	public interface IWorldCanvas : IVisible, ILateUpdate
	{
		bool Enabled { get; set; }
		GameObject Add(string prefab, Func<Vector3?> location,
			float? offscreenIconDistance = null);
	}

	public class ScreenCanvas : Canvas, IScreenCanvas, IWorldCanvas
	{
		private readonly GameObject worldCanvasParent;
		private readonly List<WorldUIObject> allAddedObjects;
		private readonly List<WorldUIObject> removedObjects;

		public ScreenCanvas(Camera camera, IObjects gameObjFactory)
			: base("ScreenCanvas", camera, gameObjFactory)
		{
			this.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this.canvas.worldCamera = camera;

			this.worldCanvasParent = gameObjFactory.New("WorldCanvas", this.gameObj);
			this.allAddedObjects = new List<WorldUIObject>();
			this.removedObjects = new List<WorldUIObject>();
			this.Enabled = true;
		}

		private bool enabled;
		public bool Enabled
		{
			get { return this.enabled; }
			set
			{
				this.enabled = value;
				if (this.enabled)
					return;
				
				foreach (var obj in this.allAddedObjects)
				{
					if (obj.UI != null)
						obj.UI.SetActive(false);
				}
			}
		}

		public GameObject Add(string prefab, Func<Vector3?> location,
			float? offscreenIconDistance = null)
		{
			if (location == null)
				return null;

			var obj = Add(prefab);
			if (obj == null)
				return obj;

			obj.transform.parent = this.worldCanvasParent.transform;
			GameObject arrowIcon = null;

			if (offscreenIconDistance != null)
			{
				arrowIcon = Add("UI/OffscreenArrow");
				arrowIcon.transform.parent = this.worldCanvasParent.transform;
			}

			this.allAddedObjects.Add(new WorldUIObject(obj, location,
				offscreenIconDistance, arrowIcon));

			return obj;
		}

		public void LateUpdate()
		{
			if (!this.Enabled)
				return;

			UpdateObjectList();

			// Go thru the objects list by farthest away objects first.
			var objectsByDistance = this.allAddedObjects.OrderByDescending(obj => Distance(obj));

			for (var i = 0; i < objectsByDistance.Length; i++)
			{
				var obj = objectsByDistance[i];
				Vector2? screenPoint = null;
				bool drawOffscreenIcon = false;

				if (obj.Location.HasValue)
				{
					screenPoint = GetCanvasPosition(obj.Location.Value,
						obj.OffscreenIconDistance, out drawOffscreenIcon);
				}

				if (screenPoint.HasValue)
				{
					/*var cameraPlane = new Plane(this.camera.transform.forward,
	                    this.camera.transform.position);
	                var globalScaleIncrease = 6;
	                var dist = cameraPlane.GetDistanceToPoint(obj.Location.Value);
	                var newScale = (obj.InitialScale * globalScaleIncrease) / (1 + dist);
	                obj.Transform.localScale = newScale;*/

					obj.Transform.anchoredPosition = screenPoint.Value;

					if (obj.OffscreenIcon != null)
					{
						if (drawOffscreenIcon)
							DrawOffscreenIcon(obj, screenPoint.Value);
						else
							obj.OffscreenIcon.SetActive(false);
					}

					if (!obj.UI.activeSelf)
						obj.UI.SetActive(true);
				}
				else
				{
					if (obj.UI.activeSelf)
						obj.UI.SetActive(false);

					//obj.Transform.localScale = obj.InitialScale;
				}

				obj.Transform.SetAsLastSibling();
			}
		}

		private void DrawOffscreenIcon(WorldUIObject obj, Vector2 screenPos)
		{
			var transform = obj.OffscreenIconTransform;
			var distance = obj.OffscreenIconDistance ?? 30;
			var iconOffset = screenPos.normalized * distance;
			transform.anchoredPosition = screenPos + iconOffset;

			var rads = Math.Atan2(-screenPos.y, screenPos.x);
			var degrees = rads * (180 / Math.PI);
			var iconRotation = (float)-degrees;
			transform.localRotation = Quaternion.Euler(0, 0, iconRotation);

			obj.OffscreenIcon.SetActive(true);
		}

		private void UpdateObjectList()
		{
			this.removedObjects.Clear();

			for (var i = 0; i < this.allAddedObjects.Count; i++)
			{
				var obj = this.allAddedObjects[i];

				// If the ui object has been destroyed, remove it from the list.
				if (obj.UI == null)
				{
					this.removedObjects.Add(obj);
					continue;
				}

				obj.UpdateLocation();
			}

			for (var i = 0; i < this.removedObjects.Count; i++)
				this.allAddedObjects.Remove(this.removedObjects[i]);
		}

		private float Distance(WorldUIObject uiObj)
		{
			if (uiObj == null || uiObj.Location == null)
				return 100000;

			var dist = uiObj.Location.Value - this.camera.transform.position;
			return dist.magnitude;
		}

		private class WorldUIObject
		{
			private readonly Func<Vector3?> locationSrc;

			public GameObject UI { get; private set; }
			public Vector3? Location { get; private set; }
			public float? OffscreenIconDistance { get; private set; }
			public GameObject OffscreenIcon { get; private set; }
			public RectTransform OffscreenIconTransform { get; private set; }
			public RectTransform Transform { get; private set; }
			public Vector3 InitialScale { get; private set; }

			public WorldUIObject(GameObject ui, Func<Vector3?> locationSrc,
				float? offscreenIconDistance, GameObject arrowIcon)
			{
				this.UI = ui;
				this.locationSrc = locationSrc;
				this.OffscreenIconDistance = offscreenIconDistance;
				this.OffscreenIcon = arrowIcon;
				this.Transform = ui.GetComponent<RectTransform>();
				this.InitialScale = ui.transform.localScale;

				if (this.OffscreenIcon != null)
				{
					this.OffscreenIconTransform =
						this.OffscreenIcon.GetComponent<RectTransform>();
				}
			}

			public void UpdateLocation()
			{
				this.Location = this.locationSrc();
			}
		}
	}
}