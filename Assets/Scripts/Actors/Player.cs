using UnityEngine;
using System.Collections;

public class Player : Singleton<Player>
{
	public Transform body;
	public float maxSpeed;
	public float acceleration;
	public float zSpeedMult;
	public float grabRadius;
	
	CharacterController controller;
	tk2dSpriteAnimator animator;
	
	enum State { None, Idle, Walk }
	State state = State.None;
	Vector3 inputAxis;
	Vector3 moveAxis;
	Vector3 velocity;
	Carryable carrying;
	Vector3 throwDirection;
	
	protected override void Awake ()
	{
		base.Awake();
		
		controller = GetComponent<CharacterController>();
		animator = GetComponentInChildren<tk2dSpriteAnimator>();
		
		SetState(State.Idle);
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
		{
			moveAxis.Normalize();
			throwDirection = moveAxis;
		}
	}
	
	int SetState(State newState)
	{
		if (state != newState)
		{
			StopAllCoroutines();
			state = newState;
			StartCoroutine(state.ToString());
		}
		return 0;
	}
	
	IEnumerator Idle()
	{
		while (true)
		{
			//Update animation
			animator.Play("PlayerIdle");
			
			TryApplyVelocity();
			if (TryAccelerate())
				yield return SetState(State.Walk);
			if (TryCarry())
				yield return 0;
			if (TryThrow())
				yield return 0;
			yield return 0;	
		}
	}
	
	IEnumerator Walk()
	{
		while (true)
		{
			//Update animation
			if (!Mathf.Approximately(moveAxis.x, 0))
			{
				animator.Play("PlayerWalkSide");
				animator.Sprite.FlipX = moveAxis.x > 0;
			}
			else if (moveAxis.z > 0)
				animator.Play("PlayerWalkBack");
			else
				animator.Play("PlayerWalkFront");
			
			TryApplyVelocity();
			if (!TryAccelerate())
				yield return SetState(State.Idle);
			if (TryCarry())
				yield return 0;
			if (TryThrow())
				yield return 0;
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
	
	bool TryCarry()
	{
		if (carrying == null && Input.GetButtonDown("A"))
		{
			var hits = Physics.OverlapSphere(transform.position, grabRadius);
			foreach (var hit in hits)
			{
				var carryable = hit.GetComponent<Carryable>();
				if (carryable != null)
				{
					carrying = carryable;
					carrying.StartCarry(transform);
					return true;
				}
			}
		}
		return false;
	}
	
	bool TryThrow()
	{
		if (carrying != null && Input.GetButtonDown("A"))
		{
			carrying.StopCarry(throwDirection);
			carrying = null;
			return true;
		}
		return false;
	}
}
