
using System;
using System.Collections.Generic;

// This snipped allows for extension methods in .net 2.0
// you need this once (only), and it must be in this namespace
namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class ExtensionAttribute : Attribute { }
}

namespace Entity
{
	// Adds basic Func/Actions to .net 2.0
	public class Delegates
	{
		public delegate T Func<T>();
		public delegate T Func<T1, T>(T1 param1);
		public delegate void Action();
		public delegate void Action<T1>(T1 param1);
		public delegate void Action<T1, T2>(T1 param1, T2 param2);
	}

	// Adds basic LINQ style operators on arrays to .net 2.0
	public static class LinqOperators
	{
		public static bool Any<T>(this T[] items, Delegates.Func<T, bool> condition)
		{
			if (items == null || condition == null)
				return false;

			for (var i = 0; i < items.Length; i++)
			{
				var item = items[i];
				if (condition(item))
					return true;
			}

			return false;
		}

		public static bool Any<T>(this ICollection<T> items, Delegates.Func<T, bool> condition)
		{
			return items.ToArray().Any(condition);
		}

		public static bool All<T>(this T[] items, Delegates.Func<T, bool> condition)
		{
			// This is how LINQ's .All handles the empty case.
			if (items == null || condition == null || items.Length == 0)
				return true;

			for (var i = 0; i < items.Length; i++)
			{
				var item = items[i];
				if (!condition(item))
					return false;
			}

			return true;
		}

		public static bool All<T>(this ICollection<T> items, Delegates.Func<T, bool> condition)
		{
			return items.ToArray().All(condition);
		}

		public static T[] Where<T>(this T[] items, Delegates.Func<T, bool> condition)
		{
			if (items == null || condition == null)
				return new T[0];

			if (items.Length == 0)
				return items;

			return Array.FindAll(items, item => condition(item));
		}

		public static T[] Where<T>(this ICollection<T> items, Delegates.Func<T, bool> condition)
		{
			return items.ToArray().Where(condition);
		}

		public static T FirstOrDefault<T>(this T[] items, Delegates.Func<T, bool> condition = null)
		{
			var filteredItems = items;

			if (condition != null)
				filteredItems = items.Where(condition);

			if (filteredItems == null || filteredItems.Length == 0)
				return default(T);

			return filteredItems[0];
		}

		public static T FirstOrDefault<T>(this ICollection<T> items, Delegates.Func<T, bool> condition = null)
		{
			return items.ToArray().FirstOrDefault(condition);
		}

		public static T[] OrderBy<T, TKey>(this T[] items,
			Delegates.Func<T, TKey> keySelector, IComparer<TKey> comparer = null)
		{
			if (items == null || keySelector == null)
				return new T[0];

			if (items.Length == 0)
				return items;

			var keys = new TKey[items.Length];
			for (var i = 0; i < keys.Length; i++)
				keys[i] = keySelector(items[i]);

			if (comparer == null)
				Array.Sort(keys, items);
			else
				Array.Sort(keys, items, comparer);

			return items;
		}

		public static T[] OrderBy<T, TKey>(this ICollection<T> items,
			Delegates.Func<T, TKey> keySelector, IComparer<TKey> comparer = null)
		{
			return items.ToArray().OrderBy(keySelector, comparer);
		}

		public static T[] OrderByDescending<T, TKey>(this T[] items,
			Delegates.Func<T, TKey> keySelector, IComparer<TKey> comparer = null)
		{
			if (items == null || keySelector == null)
				return new T[0];

			OrderBy(items, keySelector, comparer);
			Array.Reverse(items);
			return items;
		}

		public static T[] OrderByDescending<T, TKey>(this ICollection<T> items,
			Delegates.Func<T, TKey> keySelector, IComparer<TKey> comparer = null)
		{
			return items.ToArray().OrderByDescending(keySelector, comparer);
		}

		public static T[] ToArray<T>(this ICollection<T> items)
		{
			if (items == null)
				return null;

			var itemsArray = new T[items.Count];
			items.CopyTo(itemsArray, 0);
			return itemsArray;
		}
	}
}
