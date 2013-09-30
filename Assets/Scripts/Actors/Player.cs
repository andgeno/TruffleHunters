using UnityEngine;
using System.Collections;

public class Player : Singleton<Player>
{
	enum State { Idle, Walk }
	
	public GameCamera gameCameraPrefab;
	public Transform body;
	
	public float maxSpeed;
	public float acceleration;
	public float zSpeedMult;
	
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
		
		var gameCamera = gameCameraPrefab.Spawn();
		gameCamera.SetTarget(transform);
	}
	
	void Update()
	{
		//Get the input axis
		inputAxis.x = Input.GetAxis("MoveX");
		inputAxis.z = Input.GetAxis("MoveY");
		
		//Get the move axis
		moveAxis = GameCamera.transform.TransformDirection(inputAxis);
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
			var move = velocity;
			move.z *= zSpeedMult;
			controller.Move(move * Time.deltaTime);
			transform.SetY(0);
			return true;
		}
		else
			return false;
	}
}
