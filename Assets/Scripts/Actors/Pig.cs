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
	public LayerMask preventThrowMask;
	
	CharacterController controller;
	
	void Awake()
	{
		controller = GetComponent<CharacterController>();
		SetState(state);
	}
	
	public int SetState(State newState)
	{
		StopAllCoroutines();
		state = newState;
		StartCoroutine(state.ToString());
		return 0;
	}
	public int SetState(State newState, object value)
	{
		StopAllCoroutines();
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
		//StartCoroutine(transform.MoveTo(Vector3.zero, 0.2f));
		yield return StartCoroutine(transform.MoveTo(new Vector3(0, 1.2f, -0.1f), 0.2f, Ease.BackOut));
		
		while (true)
			yield return 0;
	}
	
	IEnumerator Throw(Vector3 target)
	{
		var distance = Vector3.Distance(transform.position, target);
		var control = Calc.BezierControl(transform.position, target, distance);
		yield return StartCoroutine(transform.CurveTo(control, target, 0.5f));
		SetState(State.Idle);
	}
	
	void OnStartCarry()
	{
		SetState(State.Carry);
	}
	
	void OnStopCarry(Vector3 direction)
	{
		//Find the throw target
		var throwDist = 4f;
		var ray = new Ray(Vector3.zero, Vector3.down);
		while (!Mathf.Approximately(throwDist, 0))
		{
			var target = transform.position + direction * throwDist;
			target.y = 0;
			ray.origin = target + new Vector3(0, 10);
			if (!Physics.SphereCast(ray, controller.radius, 10, preventThrowMask))
			{
				SetState(State.Throw, target);
				return;
			}
			throwDist = Mathf.MoveTowards(throwDist, 0, controller.radius);
		}
		SetState(State.Throw, new Vector3(transform.position.x, 0, transform.position.z));
	}
}
