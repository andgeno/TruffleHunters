using UnityEngine;
using System.Collections;

public class Player : Singleton<Player>
{
	enum State { Idle, Walk }
	
	public Transform body;
	
	public float maxSpeed;
	public float acceleration;
	
	State state = State.Idle;
	Vector3 inputAxis;
	Vector3 moveAxis;
	Vector3 velocity;
	
	CharacterController controller;
	
	protected override void Awake ()
	{
		base.Awake();
		
		controller = GetComponent<CharacterController>();
		SetState(state);
	}
	
	void Update()
	{
		//Get the input axis
		inputAxis.x = Input.GetAxis("MoveX");
		inputAxis.z = Input.GetAxis("MoveY");
		
		//Get the move axis
		moveAxis = Camera.main.transform.TransformDirection(inputAxis);
		moveAxis.y = 0;
		if (!moveAxis.IsZero())
			moveAxis.Normalize();
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
			TryApplyVelocity();
			if (TryAccelerate())
				yield return SetState(State.Walk);
			yield return 0;	
		}
	}
	
	IEnumerator Walk()
	{
		while (true)
		{
			TryApplyVelocity();
			if (!TryAccelerate())
				yield return SetState(State.Idle);
			yield return 0;	
		}
	}
	
	bool TryAccelerate()
	{
		if (!moveAxis.IsZero())
		{
			//Accelerate
			velocity = Vector3.MoveTowards(velocity, moveAxis * maxSpeed, acceleration * Time.deltaTime);
			return true;
		}
		else
		{
			//Slowdown
			velocity = Vector3.MoveTowards(velocity, Vector3.zero, acceleration * Time.deltaTime);
			return false;
		}
	}
	
	bool TryApplyVelocity()
	{
		if (!velocity.IsZero())
		{
			controller.Move(velocity * Time.deltaTime);
			return true;
		}
		else
			return false;
	}
}
