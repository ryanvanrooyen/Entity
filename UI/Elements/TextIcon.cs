
using UnityEngine;

namespace Entity
{
	public interface ITextIcon : IVisible, IAlpha
	{
		IImage Icon { get; }
		IText Text { get; }
		void Set(IImgSource iconSource, string text);
	}

	public class TextIcon : Visible, ITextIcon
	{
		private readonly IImage icon;
		private readonly IText text;

		public TextIcon(GameObject obj, float alpha = 1f) : base(obj)
		{
			this.icon = new Img(obj);
			this.text = new Txt(obj.Get("Text"));
			this.Alpha = alpha;
		}

		public float Alpha
		{
			get { return this.icon.Alpha; }
			set
			{
				this.icon.Alpha = value;
				this.text.Alpha = value;
			}
		}

		public void Set(IImgSource iconSource, string text)
		{
			this.icon.Source = iconSource;
			this.text.Value = text;
			this.Alpha = 1f;
		}

		public IImage Icon { get { return this.icon; } }
		public IText Text { get { return this.text; } }
	}
}
