
using UnityEngine;

namespace Entity
{
	public class InputLogger : MonoBehaviour
	{
		public void Update()
		{
			for (int i = 0; i < 20; i++)
			{
				if (Input.GetKeyUp("joystick button " + i))
					Debug.Log("Pressed: joystick button " + i);
			}

			for (int i = 1; i < 8; i++)
			{
				var value = Input.GetAxis("Axis" + i);
				if (Mathf.Abs(value) > 0.05f)
					Debug.Log("Axis" + i + ": " + value);
			}
		}
	}
}