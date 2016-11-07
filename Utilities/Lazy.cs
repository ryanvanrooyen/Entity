
using System;

namespace Entity
{
	public interface ILazy<T> where T : class
	{
		T Value { get; }
	}

	public class Lazy<T> : ILazy<T> where T : class
	{
		private readonly Func<T> factory;
		private T instance;

		public Lazy(Func<T> factory)
		{
			if (factory == null)
				throw new ArgumentNullException("factory");

			this.factory = factory;
		}

		public T Value
		{
			get
			{
				if (this.instance == null)
					this.instance = this.factory();

				return this.instance;
			}
		}
	}
}