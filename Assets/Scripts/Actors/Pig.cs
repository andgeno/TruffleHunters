using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pig : MonoBehaviour
{
	public enum State { Idle, Carry, Throw }
	State state = State.Idle;
	
	public Transform body;
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
	public int SetState(State newState, object value)
	{
		StopCoroutine(state.ToString());
		state = newState;
		StartCoroutine(state.ToString(), value);
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
		StartCoroutine(transform.MoveTo(Vector3.zero, 0.2f));
		yield return StartCoroutine(body.MoveTo(new Vector3(0, 1.2f, -0.1f), 0.2f, Ease.BackOut));
		
		while (true)
			yield return 0;
	}
	
	IEnumerator Throw(Vector3 direction)
	{
		var duration = 0.5f;
		var distance = 4f;
		
		var start = body.localPosition;
		var end = Vector3.zero;
		var control = Calc.BezierControl(start, end, start.y * 3);
		var velocity = direction * (Vector3.Distance(Vector3.zero, direction * distance) / duration);
		
		for (float time = 0; time < duration; time += Time.deltaTime)
		{
			body.localPosition = Calc.Bezier(start, control, end, time / duration);
			controller.Move(velocity * Time.deltaTime);
			yield return 0;
		}
		
		body.localPosition = Vector3.zero;
		SetState(State.Idle);
	}
	
	void OnStartCarry()
	{
		SetState(State.Carry);
	}
	
	void OnStopCarry(Vector3 direction)
	{
		SetState(State.Throw, direction);
	}
}
