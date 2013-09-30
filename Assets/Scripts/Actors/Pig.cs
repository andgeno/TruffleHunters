using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pig : MonoBehaviour
{
	
	enum State { Idle, Walk }
	State state = State.Walk;
	
	public float speed;
	public float minPlayerDistance;
	
	void Awake()
	{
		SetState(state);
	}
	
	int SetState(State newState)
	{
		StopCoroutine(state.ToString());
		state = newState;
		StartCoroutine(state.ToString());
		return 0;
	}
	
	IEnumerator Idle()
	{
		while (true)
		{
			yield return 0;	
		}
	}
	
	IEnumerator Walk()
	{
		while (true)
		{
			if (transform.DistanceTo(Player.transform) > minPlayerDistance)
				transform.MoveTowards(Player.transform, speed * Time.deltaTime);
			yield return 0;	
		}
	}
}
