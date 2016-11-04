
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
	public interface IGameObjectSorter<T>
	{
		GameObject GetClosest(out float distance, out T additionalData);
		void Add(GameObject gameObj, T additionalData);
		void Remove(GameObject gameObj);
	}

	public class PlayerCameraDistanceSorter<T> : IGameObjectSorter<T>
	{
		private readonly Transform player;
		private readonly Transform camera;
		private readonly float maxDistance;
		private readonly IList<Entry<T>> entries;
		private readonly IList<Entry<T>> removedEntries;

		public PlayerCameraDistanceSorter(
			Transform player, Transform camera, float maxDistance)
		{
			this.player = player;
			this.camera = camera;
			this.maxDistance = maxDistance;
			this.entries = new List<Entry<T>>();
			this.removedEntries = new List<Entry<T>>();
		}

		public void Add(GameObject gameObj, T additionalData)
		{
			this.entries.Add(new Entry<T>(gameObj, additionalData));
		}

		public GameObject GetClosest(out float distance, out T additionalData)
		{
			distance = 0;
			additionalData = default(T);
			UpdateEntriesList();

			var entry = this.entries
				.Where(i =>
				{
					return i.PlayerAngle < 90 && i.CameraAngle < 25 && i.Distance < maxDistance;
				})
				.OrderBy(i => i.Distance + i.PlayerAngle + (i.CameraAngle * 2))
				.FirstOrDefault();

			if (entry == null)
				return null;

			distance = entry.Distance;
			additionalData = entry.AdditionalData;
			return entry.GameObj;
		}

		public void Remove(GameObject gameObj)
		{
			if (gameObj == null)
				return;

			Entry<T> entry = null;
			for (var i = 0; i < this.entries.Count; i++)
			{
				var current = this.entries[i];
				if (current != null && current.GameObj == gameObj)
				{
					entry = current;
					break;
				}
			}

			if (entry != null)
				this.entries.Remove(entry);
		}

		private void UpdateEntriesList()
		{
			this.removedEntries.Clear();

			for (var i = 0; i < this.entries.Count; i++)
			{
				var entry = this.entries[i];
				if (entry == null || entry.GameObj == null)
				{
					this.removedEntries.Add(entry);
					continue;
				}

				entry.Update(this.player, this.camera);
			}

			for (var i = 0; i < this.removedEntries.Count; i++)
				this.entries.Remove(this.removedEntries[i]);
		}

		private class Entry<U>
		{
			public U AdditionalData { get; private set; }
			public GameObject GameObj { get; private set; }
			public float Distance { get; private set; }
			public float PlayerAngle { get; private set; }
			public float CameraAngle { get; private set; }

			public Entry(GameObject gameObj, U additionalData)
			{
				this.AdditionalData = additionalData;
				this.GameObj = gameObj;
			}

			public void Update(Transform player, Transform camera)
			{
				if (this.GameObj == null)
				{
					this.Distance = 10000;
					this.PlayerAngle = 360;
					this.CameraAngle = 360;
				}

				var pos = this.GameObj.transform.position;
				this.Distance = (pos - player.position).magnitude;
				this.PlayerAngle = player.AngleTo(pos);
				this.CameraAngle = camera.AngleTo(pos);
			}
		}
	}
}