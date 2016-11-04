
using UnityEngine;

namespace Entity
{
	public interface IImgSource
	{
		Sprite Image { get; }
	}

	public class ImgSource : IImgSource
	{
		private readonly Sprite icon;

		public ImgSource(Sprite icon)
		{
			this.icon = icon;
		}

		public virtual Sprite Image { get { return this.icon; } }
	}
}