using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pig : MonoBehaviour
{
	public enum State { Idle, Carry }
	State state = State.Idle;
	
	public float speed;
	public float minPlayerDistance;
	
	CharacterController controller;
	
	void Awake()
	{
		controller = GetComponent<CharacterController>();
		SetState(state);
	}
	
	public int SetState(State newState)
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
	
	IEnumerator Carry()
	{
		while (true)
		{
			transform.position = Player.transform.position + new Vector3(0, 1);
			yield return 0;	
		}
	}
}
