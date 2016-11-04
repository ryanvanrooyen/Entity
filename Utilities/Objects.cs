
using UnityEngine;

namespace Entity
{
	public interface IObjects
	{
		GameObject New(string name, GameObject parent = null);
		GameObject Create(GameObject prefab, GameObject parent = null);
		GameObject Create(GameObject prefab, Vector3 pos, Quaternion rot, GameObject parent = null);
		GameObject Create(string prefabFile, GameObject parent = null);
		GameObject Create(string prefabFile, Vector3 pos, Quaternion rot, GameObject parent = null);
	}

	public class Objects : IObjects
	{
		private readonly GameObject rootParent;

		public Objects()
		{
			this.rootParent = new GameObject("Dynamic Objects");
		}

		public GameObject New(string name, GameObject parent = null)
		{
			return SetParent(parent, new GameObject(name));
		}

		public GameObject Create(GameObject prefab, GameObject parent = null)
		{
			return SetParent(parent, Object.Instantiate(prefab));
		}

		public GameObject Create(GameObject prefab, Vector3 pos, Quaternion rot, GameObject parent = null)
		{
			return SetParent(parent, (GameObject)Object.Instantiate(prefab, pos, rot));
		}

		public GameObject Create(string prefabFile, GameObject parent = null)
		{
			return Create(Resources.Load<GameObject>(prefabFile), parent);
		}

		public GameObject Create(string prefabFile, Vector3 pos, Quaternion rot, GameObject parent = null)
		{
			return Create(Resources.Load<GameObject>(prefabFile), pos, rot, parent);
		}

		private GameObject SetParent(GameObject parent, GameObject obj)
		{
			if (parent == null)
				parent = this.rootParent;

			obj.transform.SetParent(parent.transform, false);
			return obj;
		}
	}
}