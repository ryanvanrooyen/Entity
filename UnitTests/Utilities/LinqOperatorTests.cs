
using System.Collections.Generic;
using Entity;
using NUnit.Framework;

namespace UnitTests
{
	[TestFixture]
	public class LinkOperators
	{
		[Test]
		public void Any()
		{
			var numbs = new int[] { 1, 2, 1, 2, 1, 3 };
			Assert.IsTrue(numbs.Any(x => x == 1));
			Assert.IsTrue(numbs.Any(x => x == 2));
			Assert.IsTrue(numbs.Any(x => x == 3));
			Assert.IsFalse(numbs.Any(x => x == 4));
			Assert.IsFalse(numbs.Any(x => x == 5));

			var numbList = new List<int>();
			Assert.IsFalse(numbList.Any(x => x == 1));

			numbList.AddRange(new int[] { 1, 2, 1, 2, 1, 3 });
			Assert.IsTrue(numbList.Any(x => x == 1));
			Assert.IsTrue(numbList.Any(x => x == 2));
			Assert.IsTrue(numbList.Any(x => x == 3));
			Assert.IsFalse(numbList.Any(x => x == 4));
			Assert.IsFalse(numbList.Any(x => x == 5));
		}

		[Test]
		public void All()
		{
			Assert.IsTrue(new int[] { }.All(x => x == 1));
			Assert.IsTrue(new int[] { 1 }.All(x => x == 1));
			Assert.IsTrue(new int[] { 1, 1 }.All(x => x == 1));
			Assert.IsTrue(new int[] { 1, 1, 1 }.All(x => x == 1));

			Assert.IsFalse(new int[] { 2, 2, 2 }.All(x => x == 1));
			Assert.IsFalse(new int[] { 1, 2, 2 }.All(x => x == 1));
			Assert.IsFalse(new int[] { 1, 1, 2 }.All(x => x == 1));

			Assert.IsTrue(new List<int>().All(x => x == 1));
			Assert.IsTrue(new List<int>(new int[] { }).All(x => x == 1));
			Assert.IsTrue(new List<int>(new int[] { 1 }).All(x => x == 1));
			Assert.IsTrue(new List<int>(new int[] { 1, 1 }).All(x => x == 1));
			Assert.IsTrue(new List<int>(new int[] { 1, 1, 1 }).All(x => x == 1));

			Assert.IsFalse(new List<int>(new int[] { 2, 2, 2 }).All(x => x == 1));
			Assert.IsFalse(new List<int>(new int[] { 1, 2, 2 }).All(x => x == 1));
			Assert.IsFalse(new List<int>(new int[] { 1, 1, 2 }).All(x => x == 1));
		}

		[Test]
		public void Where()
		{
			var numbs = new int[] { 4, 2, 3, 5, 1 };
			Assert.AreEqual(numbs.Where(x => x > 5), new int[] { });
			Assert.AreEqual(numbs.Where(x => x > 4), new int[] { 5 });
			Assert.AreEqual(numbs.Where(x => x > 3), new int[] { 4, 5 });
			Assert.AreEqual(numbs.Where(x => x > 2), new int[] { 4, 3, 5 });
			Assert.AreEqual(numbs.Where(x => x > 1), new int[] { 4, 2, 3, 5 });
			Assert.AreEqual(numbs.Where(x => x > 0), new int[] { 4, 2, 3, 5, 1 });

			var numbList = new List<int>(new int[] { 4, 2, 3, 5, 1 });
			Assert.AreEqual(numbList.Where(x => x > 5), new int[] { });
			Assert.AreEqual(numbList.Where(x => x > 4), new int[] { 5 });
			Assert.AreEqual(numbList.Where(x => x > 3), new int[] { 4, 5 });
			Assert.AreEqual(numbList.Where(x => x > 2), new int[] { 4, 3, 5 });
			Assert.AreEqual(numbList.Where(x => x > 1), new int[] { 4, 2, 3, 5 });
			Assert.AreEqual(numbList.Where(x => x > 0), new int[] { 4, 2, 3, 5, 1 });
		}

		[Test]
		public void FirstOrDefault()
		{
			var numbs = new int[] { 4, 2, 3, 5, 1 };
			Assert.AreEqual(numbs.FirstOrDefault(x => x > 5), 0);
			Assert.AreEqual(numbs.FirstOrDefault(x => x > 4), 5);
			Assert.AreEqual(numbs.FirstOrDefault(x => x > 3), 4);
			Assert.AreEqual(numbs.FirstOrDefault(x => x > 2), 4);
			Assert.AreEqual(numbs.FirstOrDefault(x => x > 1), 4);
			Assert.AreEqual(numbs.FirstOrDefault(x => x > 0), 4);

			var numbList = new List<int>(new int[] { 4, 2, 3, 5, 1 });
			Assert.AreEqual(numbList.FirstOrDefault(x => x > 5), 0);
			Assert.AreEqual(numbList.FirstOrDefault(x => x > 4), 5);
			Assert.AreEqual(numbList.FirstOrDefault(x => x > 3), 4);
			Assert.AreEqual(numbList.FirstOrDefault(x => x > 2), 4);
			Assert.AreEqual(numbList.FirstOrDefault(x => x > 1), 4);
			Assert.AreEqual(numbList.FirstOrDefault(x => x > 0), 4);
		}

		[Test]
		public void OrderBy()
		{
			var numbs = new int[] { 4, 2, 3, 5, 1 };
			var orderedNumbs = numbs.OrderBy(x => x);

			Assert.AreSame(orderedNumbs, numbs);
			Assert.AreEqual(orderedNumbs, new int[] { 1, 2, 3, 4, 5 });

			var numbList = new List<int>();
			numbList.AddRange(new int[] { 4, 2, 3, 5, 1 });

			orderedNumbs = numbList.OrderBy(x => x);
			Assert.AreEqual(orderedNumbs, new int[] { 1, 2, 3, 4, 5 });
		}

		[Test]
		public void OrderByDescending()
		{
			var numbs = new int[] { 4, 2, 3, 5, 1 };
			var orderedNumbs = numbs.OrderByDescending(x => x);

			Assert.AreSame(orderedNumbs, numbs);
			Assert.AreEqual(orderedNumbs, new int[] { 5, 4, 3, 2, 1 });

			var numbList = new List<int>();
			numbList.AddRange(new int[] { 4, 2, 3, 5, 1 });

			orderedNumbs = numbList.OrderByDescending(x => x);
			Assert.AreEqual(orderedNumbs, new int[] { 5, 4, 3, 2, 1 });
		}
	}
}
