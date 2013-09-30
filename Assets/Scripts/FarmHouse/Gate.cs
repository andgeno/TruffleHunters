using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour
{
	public Transform pivot;
	public float openAngle;
	public float openTime;
	public EaseType easeType;
	
	int targetState;
	
	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Player>() != null)
			targetState = other.transform.position.z  > transform.position.z ? 1 : -1;
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Player>() != null)
		{
			targetState = 0;
		}
	}
	
	void Start()
	{
		StartCoroutine(UpdateState());
	}
	
	IEnumerator UpdateState()
	{
		var state = targetState;
		while (true)
		{
			if (state != targetState)
			{
				state = targetState;
				yield return StartCoroutine(pivot.RotateTo(Quaternion.Euler(0, state * openAngle, 0), openTime, easeType));
			}
			yield return 0;	
		}
	}
}
