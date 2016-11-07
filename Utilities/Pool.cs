
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
		private readonly Func<T> factory;
		private readonly IList<T> items;
		private int currentIndex = 0;

		public string TestProp { get; private set; }

		public Pool(int number, Func<T> factory)
		{
			if (number <= 0)
				throw new ArgumentException("number must be a positive value");
			if (factory == null)
				throw new ArgumentNullException("factory");

			var str = "";//$"blah{factory.ToString()}";
			this.TestProp = str;

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