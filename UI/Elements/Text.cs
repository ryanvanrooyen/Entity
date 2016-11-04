
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Entity
{
	public interface IText : IVisible, IColor
	{
		string Value { get; set; }
	}

	public class Txt : Visible, IText
	{
		private readonly Text text;

		public Txt(GameObject textObj)
			: this(textObj, textObj.GetComponent<Text>())
		{
		}

		public Txt(Text text) : this(text.gameObject, text)
		{
		}

		private Txt(GameObject textObj, Text text) : base(textObj)
		{
			if (text == null)
				throw new ArgumentNullException("text");

			this.text = text;
		}

		public string Value
		{
			get
			{
				return this.text.text;
			}
			set
			{
				this.text.text = value;
			}
		}

		public float Alpha
		{
			get { return this.text.color.a; }
			set { SetAlpha(value); }
		}

		public Color Color
		{
			get { return this.text.color; }
			set { this.text.color = value; }
		}

		private void SetAlpha(float alpha)
		{
			Color c = this.text.color;
			c.a = alpha;
			this.text.color = c;
		}
	}

	public class TxtWithBackground : IText
	{
		private readonly IText text;
		private readonly IImage background;

		public TxtWithBackground(IText text, IImage background)
		{
			if (text == null)
				throw new ArgumentNullException("text");
			if (background == null)
				throw new ArgumentNullException("background");

			this.text = text;
			this.background = background;
		}

		public string Value
		{
			get { return this.text.Value; }
			set { this.text.Value = value; }
		}

		public bool IsVisible
		{
			get { return this.text.IsVisible; }
			set
			{
				this.text.IsVisible = value;
				this.background.IsVisible = value;
			}

		}

		public float Alpha
		{
			get { return this.text.Alpha; }
			set { this.text.Alpha = value; }
		}

		public Color Color
		{
			get { return this.text.Color; }
			set { this.text.Color = value; }
		}
	}

	public class NoText : NullVisible, IText
	{
		public float Alpha { get; set; }
		public Color Color { get; set; }
		public string Value { get; set; }
	}

	public class CompositeTxt : Visible, IText
	{
		private readonly Text[] texts;

		public CompositeTxt(GameObject textObj) : base(textObj)
		{
			if (textObj == null)
				throw new ArgumentNullException("textObj");

			this.texts = textObj.GetComponentsInChildren<Text>();
		}

		public string Value
		{
			get
			{
				var text = this.texts.FirstOrDefault();
				return text != null ? text.text : null;
			}
			set
			{
				foreach (var text in this.texts)
					text.text = value;
			}
		}

		public float Alpha
		{
			get
			{
				var text = this.texts.FirstOrDefault();
				return text != null ? text.color.a : 0;
			}
			set
			{
				foreach (var text in this.texts)
					SetAlpha(text, value);
			}
		}

		public Color Color
		{
			get { return this.texts.FirstOrDefault().color; }
			set
			{
				foreach (var text in this.texts)
					text.color = value;
			}
		}

		private void SetAlpha(Text text, float alpha)
		{
			Color c = text.color;
			c.a = alpha;
			text.color = c;
		}
	}
}