using UnityEngine;
using System.Collections;

public class GameCamera : Singleton<GameCamera>
{
	public float moveRate;
	
	Transform target;
	
	public void SetTarget(Transform target)
	{
		this.target = target;
	}
	
	void LateUpdate()
	{
		//transform.position = Vector3.Lerp(transform.position, target.position, moveRate * Time.deltaTime);
		transform.position = target.position;
	}
}
