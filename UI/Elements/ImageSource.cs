
using System;
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
			if (icon == null)
				throw new ArgumentNullException("icon");
			
			this.icon = icon;
		}

		public virtual Sprite Image { get { return this.icon; } }
	}
}