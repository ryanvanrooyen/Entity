
using System;
using System.Collections.Generic;

namespace Entity
{
	public interface IPool<T> where T : class
	{
		T GetInstance();
	}

	public class Pool<T> : IPool<T> where T : class
	{
		private readonly Delegates.Func<T> factory;
		private readonly IList<T> items;
		private int currentIndex = 0;

		public Pool(int number, Delegates.Func<T> factory)
		{
			if (number <= 0)
				throw new ArgumentException("number must be a positive value");
			if (factory == null)
				throw new ArgumentNullException("factory");

			this.factory = factory;
			this.items = new List<T>(number);
			for (var i = 0; i < number; i++)
				this.items.Add(null);
		}

		public T GetInstance()
		{
			var item = this.items[this.currentIndex];
			if (item == null)
			{
				item = this.factory();
				this.items[this.currentIndex] = item;
			}

			this.currentIndex = (this.currentIndex + 1) % this.items.Count;
			return item;
		}
	}
}