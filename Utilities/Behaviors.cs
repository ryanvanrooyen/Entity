
using UnityEngine;
using System;

namespace Entity
{
	public interface IUpdate
	{
		void Update();
	}

	public interface ILateUpdate
	{
		void LateUpdate();
	}

	public interface IUnityBehavior
	{
		Action OnUpdate { get; set; }
		Action OnLateUpdate { get; set; }
		Action<Collider> OnTriggerEntered { get; set; }
		Action<Collider> OnTriggerExited { get; set; }
		Action<Collision> OnCollisionEntered { get; set; }
		Action DrawGizmos { get; set; }
	}
}
