
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

	public interface IBehavior
	{
		Delegates.Action OnUpdate { get; set; }
		Delegates.Action OnLateUpdate { get; set; }
		Action<Collider> OnTriggerEntered { get; set; }
		Action<Collider> OnTriggerExited { get; set; }
		Action<Collision> OnCollisionEntered { get; set; }
		Delegates.Action DrawGizmos { get; set; }
	}
}
