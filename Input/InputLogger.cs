
using UnityEngine;

namespace Entity
{
	public class InputLogger : MonoBehaviour
	{
		public bool Enabled { get; set; }

		public void Update()
		{
			if (!this.Enabled)
				return;

			for (int i = 0; i < 20; i++)
			{
				if (Input.GetKeyUp("joystick button " + i))
					Debug.Log("Pressed: joystick button " + i);
			}
		}
	}
}