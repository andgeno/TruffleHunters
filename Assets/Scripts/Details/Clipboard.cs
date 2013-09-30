using UnityEngine;
using System.Collections;

public class Clipboard : MonoBehaviour
{
	[Range(0, 1)] public float amount;
	
	void LateUpdate()
	{
		var rotate = Quaternion.FromToRotation(Vector3.forward, Camera.main.transform.forward);
		rotate = Quaternion.Lerp(rotate, rotate * rotate, amount);
		transform.rotation = rotate;
	}
}
