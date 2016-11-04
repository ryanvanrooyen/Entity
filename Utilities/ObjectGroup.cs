
using System;
using UnityEngine;

namespace Entity
{
	public interface IGameObjSource
	{
		GameObject GameObj { get; }
	}

	public interface IObjectGroup : IVisible
	{
		void Add(IGameObjSource gameObjSource);
	}

	public class ObjectGroup : Visible, IObjectGroup
	{
		private readonly GameObject container;

		public ObjectGroup(GameObject container, bool? initVisible = null)
			: base(container, initVisible)
		{
			this.container = container;
		}

		public void Add(IGameObjSource gameObjSource)
		{
			if (gameObjSource == null || gameObjSource.GameObj == null)
				return;

			var gameObj = gameObjSource.GameObj;
			gameObj.transform.SetParent(this.container.transform);
		}
	}

	public class GameObjSource : IGameObjSource
	{
		public GameObjSource(GameObject gameObj)
		{
			if (gameObj == null)
				throw new ArgumentNullException("gameObj");

			this.GameObj = gameObj;
		}

		public GameObject GameObj { get; protected set; }
	}
}