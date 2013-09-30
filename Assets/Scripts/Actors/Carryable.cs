using UnityEngine;
using System.Collections;

public class Carryable : MonoBehaviour
{
	public void StartCarry(Transform carrying)
	{
		transform.parent = carrying;
		gameObject.SendMessage("OnStartCarry", SendMessageOptions.DontRequireReceiver);
	}
	
	public void StopCarry(Vector3 direction)
	{
		transform.parent = null;
		gameObject.SendMessage("OnStopCarry", direction, SendMessageOptions.DontRequireReceiver);
	}
}
