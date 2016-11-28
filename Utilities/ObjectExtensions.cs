
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

		public static IUnityBehavior AddBehavior(this GameObject gameObj)
		{
			return gameObj.AddComponent<UnityBehavior>();
		}

		public static void Run(this GameObject gameObj, string coroutineId, IEnumerator coroutine)
		{
			RunOn(gameObj, gameObj, coroutineId, coroutine);
		}

		private static void RunAlways(this GameObject gameObj, string coroutineId, IEnumerator coroutine)
		{
			RunOn(coroutineSource, gameObj, coroutineId, coroutine);
		}

		private static void RunOn(GameObject source, GameObject gameObj,
			string coroutineId, IEnumerator coroutine)
		{
			if (source == null || gameObj == null || coroutine == null || coroutineId == null)
				return;

			var metadata = gameObj.GetOrAddComponent<Metadata>();
			var srcMetadata = source.GetOrAddComponent<Metadata>();
			var info = metadata.GetOrAdd(() => new List<CoroutineInfo>());

			CoroutineInfo existing = null;
			for (var i = 0; i < info.Count; i++)
			{
				var current = info[i];
				if (current.Id == coroutineId)
				{
					existing = current;
					break;
				}
			}

			if (existing != null)
			{
				info.Remove(existing);
				srcMetadata.StopCoroutine(existing.Enumerator);
			}

			info.Add(new CoroutineInfo(coroutine, coroutineId));

			srcMetadata.StartCoroutine(coroutine);
		}

		public static T GetOrAddComponent<T>(this GameObject gameObj) where T : MonoBehaviour
		{
			var component = gameObj.GetComponent<T>();
			if (component == null)
				component = gameObj.AddComponent<T>();

			return component;
		}

		private class CoroutineInfo
		{
			public IEnumerator Enumerator { get; private set; }
			public string Id { get; private set; }

			public CoroutineInfo(IEnumerator enumerator, string id)
			{
				this.Enumerator = enumerator;
				this.Id = id;
			}
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


		public static float VerticalAngleTo(this Transform transform, Vector3 point)
		{
			var inverseRot = Quaternion.Inverse(transform.rotation);
			var targetDir = inverseRot * (point - transform.position);
			var forward = inverseRot * transform.forward;

			var targetDirNormalize = targetDir.normalized;
			var to = new Vector3(0, -targetDirNormalize.y, targetDirNormalize.z);
			var rot = Quaternion.FromToRotation(forward, to);

			var verticalAngle = rot.eulerAngles.x;
			if (verticalAngle > 180f)
				verticalAngle = verticalAngle - 360;

			return verticalAngle;
		}

		public static void Move(this GameObject gameObj,
			Vector3 position, float durationSecs = 0f, bool animateIndependentOfTime = false)
		{
			if (Equal(gameObj.transform.position, position))
				return;

			if (durationSecs <= 0f)
			{
				gameObj.transform.position = position;
				return;
			}

			gameObj.RunAlways("_LerpPos", LerpPos(gameObj,
				position, durationSecs, animateIndependentOfTime));
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
			Vector3 position, float durationSecs = 0f, bool animateIndependentOfTime = false)
		{
			if (Equal(gameObj.transform.localPosition, position))
				return;

			if (durationSecs <= 0f)
			{
				gameObj.transform.localPosition = position;
				return;
			}

			gameObj.RunAlways("_LerpPosLocal", LerpPosLocal(gameObj,
				position, durationSecs, animateIndependentOfTime));
		}

		private static IEnumerator LerpPos(GameObject gameObj,
			Vector3 position, float durationSecs, bool animateIndependentOfTime)
		{
			var transform = gameObj.transform;
			var elapsedTime = animateIndependentOfTime ? Time.unscaledDeltaTime : Time.deltaTime;
			var start = transform.position;

			while (true)
			{
				var percent = elapsedTime / durationSecs;
				if (percent >= 1.0f)
				{
					transform.position = position;
					break;
				}

				transform.position = Vector3.Lerp(start, position, percent);
				var deltaTime = animateIndependentOfTime ? Time.unscaledDeltaTime : Time.deltaTime;
				elapsedTime += deltaTime;

				yield return null;
			}
		}

		private static IEnumerator LerpPosLocal(GameObject gameObj,
			Vector3 position, float durationSecs, bool animateIndependentOfTime)
		{
			var transform = gameObj.transform;
			var elapsedTime = animateIndependentOfTime ? Time.unscaledDeltaTime : Time.deltaTime;
			var start = transform.localPosition;

			while (true)
			{
				var percent = elapsedTime / durationSecs;
				if (percent >= 1.0f)
				{
					transform.localPosition = position;
					break;
				}

				transform.localPosition = Vector3.Lerp(start, position, percent);
				var deltaTime = animateIndependentOfTime ? Time.unscaledDeltaTime : Time.deltaTime;
				elapsedTime += deltaTime;

				yield return null;
			}
		}

		public static void Scale(this GameObject gameObj, float scale,
			float durationSecs = 0f, bool animateIndependentOfTime = false)
		{
			var destScale = new Vector3(scale, scale, scale);
			if (Equal(gameObj.transform.localScale, destScale))
				return;

			if (durationSecs <= 0f)
			{
				gameObj.transform.localScale = destScale;
				return;
			}

			gameObj.RunAlways("_LerpScale", LerpScale(
				gameObj, destScale, durationSecs, animateIndependentOfTime));
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

		private static IEnumerator LerpScale(GameObject gameObj,
			Vector3 destScale, float durationSecs, bool animateIndependentOfTime)
		{
			var transform = gameObj.transform;
			var elapsedTime = animateIndependentOfTime ? Time.unscaledDeltaTime : Time.deltaTime;
			var start = transform.localScale;

			while (true)
			{
				var percent = elapsedTime / durationSecs;
				if (percent >= 1.0f)
				{
					transform.localScale = destScale;
					break;
				}

				transform.localScale = Vector3.Lerp(start, destScale, percent);
				var deltaTime = animateIndependentOfTime ? Time.unscaledDeltaTime : Time.deltaTime;
				elapsedTime += deltaTime;

				yield return null;
			}
		}

		public static Vector3 GetAimPoint(this Transform transform, Vector3 targetPos,
			Vector3 targetVelocity, float projectileVelocity)
		{
			if (transform == null)
				return Vector3.zero;

			//distance in between in meters
			var pos = transform.position;
			var distance = Vector3.Distance(pos, targetPos);

			// Adjust velocity based on super close or super far distances.
			if (distance < 30)
				projectileVelocity = projectileVelocity - (30 - distance);
			
			var flightTime = distance / projectileVelocity;

			//time in seconds the shot would need to arrive at the target
			var travelTime = distance / projectileVelocity;

			var aimPoint = targetPos + targetVelocity * travelTime;

			// Second Pass, calculate distance based on first pass' result
			// Using point between aimPoint and target
			var distance2 = Vector3.Distance(pos, targetPos + ((targetPos - aimPoint) / 2F));
			var flightTime2 = distance2 / projectileVelocity;
			aimPoint = targetPos + targetVelocity * flightTime2;


			// Calculate vertical drop.
			var initVerticalVelocity = Vector3.zero;
			var heightDrop = (initVerticalVelocity * flightTime) +
				(0.5f * Physics.gravity * (flightTime * flightTime));

			aimPoint = aimPoint - heightDrop;

			return aimPoint;
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

			public T GetOrAdd<T>(Func<T> factory) where T : class
			{
				var item = Get<T>();
				if (item != null)
					return item;

				var newItem = factory();
				Add(newItem);
				return newItem;
			}

			public T Remove<T>() where T : class
			{
				var item = Get<T>();
				if (item != null)
					this.data.Remove(item);

				return item;
			}
		}

		private class UnityBehavior : MonoBehaviour, IUnityBehavior
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