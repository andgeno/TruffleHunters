using UnityEngine;
using System.Collections;

public class Carryable : MonoBehaviour
{
	public void StartLift(Transform carrying)
	{
		transform.parent = carrying;
		gameObject.SendMessage("OnStartLift", SendMessageOptions.DontRequireReceiver);
	}
	
	public void StartCarry()
	{
		gameObject.SendMessage("OnStartCarry", SendMessageOptions.DontRequireReceiver);
	}
	
	public void StopCarry(Vector3 direction)
	{
		transform.parent = null;
		gameObject.SendMessage("OnStopCarry", direction, SendMessageOptions.DontRequireReceiver);
	}
}
