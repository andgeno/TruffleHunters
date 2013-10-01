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
	public DustParticle dustParticlePrefab;
	
	CharacterController controller;
	int dataIndex;
	
	void Awake()
	{
		controller = GetComponent<CharacterController>();
	}
	
	public void SetData(int index)
	{
		dataIndex = index;
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
			yield return 0;
	}
	
	IEnumerator Carry()
	{
		//StartCoroutine(transform.MoveTo(Vector3.zero, 0.2f));
		yield return StartCoroutine(transform.MoveTo(new Vector3(0, 0.7f, -0.1f), 0.2f, Ease.BackOut));
		
		while (true)
			yield return 0;
	}
	
	IEnumerator Throw(Vector3 target)
	{
		var distance = Vector3.Distance(transform.position, target);
		var heightMult = 0.5f;
		var control = Calc.BezierControl(transform.position, target, distance * heightMult);
		yield return StartCoroutine(transform.CurveTo(control, target, 0.5f));
		DustParticle.Create(dustParticlePrefab, transform, new Vector3(0, 0, -0.1f));
		SetState(State.Idle);
		gameObject.SendMessage("OnThrowEnd", SendMessageOptions.DontRequireReceiver);
	}
	
	void OnStartLift()
	{
		
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
	
	public PigData data
	{
		get { return Game.instance.data.pigs[dataIndex]; }
	}
	
	public static Pig Spawn(Pig prefab, int dataIndex, Vector3 position)
	{
		var pig = prefab.Spawn(position);
		pig.SetData(dataIndex);
		pig.name += "(" + pig.data.name + ")";
		return pig;
	}
}
