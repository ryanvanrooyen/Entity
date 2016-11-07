
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
	public static class GameObjectExtensions
	{
		private static GameObject coroutineSource;

		static GameObjectExtensions()
		{
			var name = "Coroutines";
			coroutineSource = GameObject.Find(name);

			if (coroutineSource == null)
			{
				coroutineSource = new GameObject(name);
				coroutineSource.AddComponent<DontDestroyBetweenScenes>();
			}
		}

		public static void AddMetadata<T>(this GameObject gameObj, T data) where T : class
		{
			if (data == null)
				return;
			var metadata = gameObj.GetComponent<Metadata>();
			if (metadata == null)
				metadata = gameObj.AddComponent<Metadata>();

			metadata.Add(data);
		}

		public static T RemoveMetadata<T>(this GameObject gameObj) where T : class
		{
			var metadata = gameObj.GetComponent<Metadata>();
			if (metadata == null)
				return null;

			return metadata.Remove<T>();
		}

		public static T GetMetadata<T>(this GameObject gameObj,
			bool walkParentTree = false) where T : class
		{
			var metadata = gameObj.GetComponent<Metadata>();
			if (metadata == null)
			{
				if (!walkParentTree)
					return null;

				var parent = gameObj.transform.parent;
				while (parent != null)
				{
					metadata = parent.gameObject.GetComponent<Metadata>();
					if (metadata != null)
						break;

					parent = parent.parent;
				}

				if (metadata == null)
					return null;
			}

			return metadata.Get<T>();
		}

		public static IBehavior AddBehavior(this GameObject gameObj)
		{
			return gameObj.AddComponent<UnityBehavior>();
		}

		public static void Run(this GameObject gameObj, IEnumerator coroutine)
		{
			RunOn(gameObj, coroutine);
		}

		private static void RunAlways(this GameObject gameObj, IEnumerator coroutine)
		{
			RunOn(coroutineSource, coroutine);
		}

		private static void RunOn(GameObject source, IEnumerator coroutine)
		{
			if (source == null)
				return;

			var coroutines = source.GetComponent<Metadata>();
			if (coroutines == null)
				coroutines = source.AddComponent<Metadata>();

			coroutines.StartCoroutine(coroutine);
		}

		public static GameObject Get(this GameObject gameObj, string name)
		{
			if (gameObj == null)
				return null;

			return Get(gameObj.transform, name);
		}

		public static GameObject Get(this Transform transform, string name)
		{
			if (transform == null || string.IsNullOrEmpty(name))
				return null;

			var transforms = transform.GetComponentsInChildren<Transform>();
			for (var i = 0; i < transforms.Length; i++)
			{
				var t = transforms[i];
				if (t.name == name)
					return t.gameObject;
			}

			return null;
			//var transform = gameObject.transform.Find(name);
			//if (transform == null)
			//    return null;

			//return transform.gameObject;
		}

		public static Vector3 RightLocal(this Transform transform)
		{
			if (transform == null)
				return Vector3.right;

			return Vector3.Cross(transform.up.normalized, transform.forward.normalized);
		}

		public static Vector3 LeftLocal(this Transform transform)
		{
			if (transform == null)
				return -Vector3.right;

			return -RightLocal(transform);
		}

		public static float AngleTo(this Transform transform, Vector3 point)
		{
			var directionToPoint = point - transform.position;
			return Vector3.Angle(transform.forward, directionToPoint);
		}

		public static float HorizontalAngleTo(this Transform transform, Vector3 point)
		{
			var inverseRot = Quaternion.Inverse(transform.rotation);
			var targetDir = inverseRot * (point - transform.position);
			var forward = inverseRot * transform.forward;

			var targetDirNormalize = targetDir.normalized;
			var to = new Vector3(targetDirNormalize.x, 0, targetDirNormalize.z);
			var rot = Quaternion.FromToRotation(forward, to);

			var horizontalAngle = rot.eulerAngles.y;
			if (horizontalAngle > 180f)
				horizontalAngle = horizontalAngle - 360;

			return horizontalAngle;
		}

		public static void Move(this GameObject gameObj,
			Vector3 position, float animationSpeed = 0f)
		{
			if (Equal(gameObj.transform.position, position))
				return;

			if (animationSpeed <= 0f)
			{
				gameObj.transform.position = position;
				return;
			}

			gameObj.RunAlways(LerpPos(gameObj, position, animationSpeed));
		}

		public static bool HasParent(this GameObject gameObj, GameObject parent)
		{
			if (parent == null)
				return false;

			return HasParent(gameObj, parent.transform);
		}

		public static bool HasParent(this GameObject gameObj, Transform parent)
		{
			if (gameObj == null)
				return false;

			return HasParent(gameObj.transform, parent);
		}

		public static bool HasParent(this Transform transform, Transform parent)
		{
			if (transform == null || parent == null)
				return false;

			var currentParent = transform;

			while (currentParent != null)
			{
				if (currentParent == parent)
					return true;

				currentParent = currentParent.parent;
			}

			return false;
		}

		public static bool HasParentTag(this GameObject gameObj, string tag)
		{
			if (gameObj == null || tag == null)
				return false;

			var currentParent = gameObj.transform;

			while (currentParent != null)
			{
				if (currentParent.tag == tag)
					return true;

				currentParent = currentParent.parent;
			}

			return false;
		}

		public static void MoveLocal(this GameObject gameObj,
			Vector3 position, float animationSpeed = 0f)
		{
			if (Equal(gameObj.transform.localPosition, position))
				return;

			if (animationSpeed <= 0f)
			{
				gameObj.transform.localPosition = position;
				return;
			}

			gameObj.RunAlways(LerpPosLocal(gameObj, position, animationSpeed));
		}

		private static IEnumerator LerpPos(GameObject gameObj, Vector3 position, float speed)
		{
			var transform = gameObj.transform;
			var waitSecs = 0.01f * speed;
			var progress = 0f;
			var start = transform.position;

			while (!Equal(transform.position, position))
			{
				progress += waitSecs;
				transform.position = Vector3.Lerp(start, position, progress);
				yield return null;
			}
		}

		private static IEnumerator LerpPosLocal(GameObject gameObj, Vector3 position, float speed)
		{
			var transform = gameObj.transform;
			var waitSecs = 0.01f * speed;
			var progress = 0f;
			var start = transform.localPosition;

			while (!Equal(transform.localPosition, position))
			{
				progress += waitSecs;
				transform.localPosition = Vector3.Lerp(start, position, progress);
				yield return null;
			}
		}

		public static void Scale(this GameObject gameObj, float scale, float animationSpeed = 0f)
		{
			var destScale = new Vector3(scale, scale, scale);
			if (Equal(gameObj.transform.localScale, destScale))
				return;

			if (animationSpeed <= 0f)
			{
				gameObj.transform.localScale = destScale;
				return;
			}

			gameObj.RunAlways(LerpScale(gameObj, destScale, animationSpeed));
		}

		public static void SetLayerRecursively(this GameObject obj, int newLayer)
		{
			if (null == obj)
				return;

			obj.layer = newLayer;
			foreach (Transform child in obj.transform)
			{
				if (null == child)
					continue;
				SetLayerRecursively(child.gameObject, newLayer);
			}
		}

		private static IEnumerator LerpScale(GameObject gameObj, Vector3 destScale, float speed)
		{
			var transform = gameObj.transform;
			var waitSecs = 0.01f * speed;
			var progress = 0f;
			var start = transform.localScale;

			while (!Equal(transform.localScale, destScale))
			{
				progress += waitSecs;
				transform.localScale = Vector3.Lerp(start, destScale, progress);
				yield return null;
			}

			//Debug.Log(string.Format("Finished lerping to scale: {0} {1}", gameObj.name, transform.localScale));
		}

		private static bool Equal(Vector3 v1, Vector3 v2)
		{
			return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
		}

		private class Metadata : MonoBehaviour
		{
			private readonly List<object> data;
			public Metadata() { this.data = new List<object>(); }

			public void Add<T>(T data) where T : class
			{
				this.data.Add(data);
			}

			public T Get<T>() where T : class
			{
				for (var i = 0; i < this.data.Count; i++)
				{
					var item = this.data[i] as T;
					if (item != null)
						return item;
				}

				return null;
			}

			public T Remove<T>() where T : class
			{
				var item = Get<T>();
				if (item != null)
					this.data.Remove(item);

				return item;
			}
		}

		private class UnityBehavior : MonoBehaviour, IBehavior
		{
			public Action OnUpdate { get; set; }
			public Action OnLateUpdate { get; set; }
			public Action<Collider> OnTriggerEntered { get; set; }
			public Action<Collider> OnTriggerExited { get; set; }
			public Action<Collision> OnCollisionEntered { get; set; }
			public Action DrawGizmos { get; set; }

			// Update is called once per frame
			public void Update()
			{
				if (this.OnUpdate != null)
					this.OnUpdate();
			}

			public void LateUpdate()
			{
				if (this.OnLateUpdate != null)
					this.OnLateUpdate();
			}

			public void OnTriggerEnter(Collider other)
			{
				if (this.OnTriggerEntered != null)
					this.OnTriggerEntered(other);
			}

			public void OnTriggerExit(Collider other)
			{
				if (this.OnTriggerExited != null)
					this.OnTriggerExited(other);
			}

			public void OnCollisionEnter(Collision collision)
			{
				if (this.OnCollisionEntered != null)
					this.OnCollisionEntered(collision);
			}

			public void OnDrawGizmos()
			{
				if (this.DrawGizmos != null)
					this.DrawGizmos();
			}
		}

		private class DontDestroyBetweenScenes : MonoBehaviour
		{
			public void Awake()
			{
				DontDestroyOnLoad(this.gameObject);
			}
		}
	}
}