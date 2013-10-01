using UnityEngine;
using System.Collections;

public class Player : Singleton<Player>
{
	public Transform body;
	public float maxSpeed;
	public float acceleration;
	public float zSpeedMult;
	public float grabRadius;
	public LayerMask noThrowMask;
	
	CharacterController controller;
	tk2dSpriteAnimator animator;
	
	enum State { None, Idle, Walk }
	State state = State.None;
	Vector3 inputAxis;
	Vector3 moveAxis;
	Vector3 velocity;
	Carryable carrying;
	Vector3 aimDirection;
	
	
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
			aimDirection = moveAxis;
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
			PlayAnim("PlayerIdle");
			
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
			PlayAnim("PlayerWalk");
			
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
	
	void PlayAnim(string prefix)
	{
		if (!Mathf.Approximately(aimDirection.x, 0))
		{
			animator.Play(prefix + "Side");
			animator.Sprite.FlipX = aimDirection.x > 0;
		}
		else if (aimDirection.z > 0)
			animator.Play(prefix + "Back");
		else
			animator.Play(prefix + "Front");
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
			if (!Physics.CheckSphere(transform.position, controller.radius, noThrowMask))
			{
				carrying.StopCarry(aimDirection);
				carrying = null;
				return true;
			}
		}
		return false;
	}
}
