
using UnityEngine;
using UnityEngine.UI;

namespace Entity
{
	public interface IMenuCanvas : ICanvas
	{ }

	public class MenuCanvas : Canvas, IMenuCanvas
	{
		public MenuCanvas(Camera camera, IObjects gameObjFactory)
			: base("MenuCanvas", camera, gameObjFactory)
		{
			this.canvas.renderMode = RenderMode.ScreenSpaceCamera;
			this.canvas.worldCamera = camera;
			this.canvas.planeDistance = 1f;

			var scaler = this.gameObj.AddComponent<CanvasScaler>();
			scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
			scaler.referenceResolution = new Vector2(3440, 1000);
			scaler.matchWidthOrHeight = 1f;
		}
	}
}