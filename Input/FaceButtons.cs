
using System;

namespace Entity
{
	public interface IFaceButtons
	{
		IButton Top { get; }
		IButton Bottom { get; }
		IButton Left { get; }
		IButton Right { get; }
	}

	public class FaceButtons : IFaceButtons
	{
		private readonly IButton top;
		private readonly IButton bottom;
		private readonly IButton left;
		private readonly IButton right;

		public FaceButtons(IButton top, IButton bottom,
			IButton left, IButton right)
		{
			if (top == null)
				throw new ArgumentNullException("top");
			if (bottom == null)
				throw new ArgumentNullException("bottom");
			if (left == null)
				throw new ArgumentNullException("left");
			if (right == null)
				throw new ArgumentNullException("right");

			this.top = top;
			this.bottom = bottom;
			this.left = left;
			this.right = right;
		}

		public IButton Top { get { return this.top; } }
		public IButton Bottom { get { return this.bottom; } }
		public IButton Left { get { return this.left; } }
		public IButton Right { get { return this.right; } }
	}
}