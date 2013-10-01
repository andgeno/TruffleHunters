using UnityEngine;
using System.Collections;

public class Player : Singleton<Player>
{
	public Transform body;
	public float maxSpeed;
	public float carrySpeed;
	public float acceleration;
	public float zSpeedMult;
	public float grabRadius;
	public LayerMask noThrowMask;
	public SweatParticle sweatParticlePrefab;
	
	CharacterController controller;
	tk2dSpriteAnimator animator;
	
	enum State { None, Idle, Walk, Lift, CarryIdle, CarryWalk }
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
		
		sweatParticlePrefab.CreatePool();
		InvokeRepeating("Sweat", 0.50f, 0.50f);
		
		SetState(State.Idle);
	}
	
	void Sweat()
	{
		if (carrying != null)
			SweatParticle.Create(sweatParticlePrefab, transform, new Vector3(0, 1f, -0.1f));
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
			PlayAnim("PlayerIdle");
			TryApplyVelocity();
			if (TryAccelerate(maxSpeed))
				yield return SetState(State.Walk);
			if (TryCarry())
				yield return SetState(State.Lift);
			yield return 0;	
		}
	}
	
	IEnumerator Walk()
	{
		while (true)
		{
			PlayAnim("PlayerWalk");
			TryApplyVelocity();
			if (!TryAccelerate(maxSpeed))
				yield return SetState(State.Idle);
			if (TryCarry())
				yield return SetState(State.Lift);
			yield return 0;
		}
	}
	
	IEnumerator Lift()
	{
		animator.AnimationCompleted = LiftEnd;
		animator.Play("PlayerLiftFront");
		while (true)
			yield return 0;
	}
	
	IEnumerator CarryIdle()
	{
		while (true)
		{
			PlayAnim("PlayerHoldIdle");
			TryApplyVelocity();
			if (TryAccelerate(carrySpeed))
				yield return SetState(State.CarryWalk);
			if (TryThrow())
				yield return SetState(State.Idle);
			yield return 0;	
		}
	}
	
	IEnumerator CarryWalk()
	{
		while (true)
		{
			PlayAnim("PlayerHoldWalk");
			TryApplyVelocity();
			if (!TryAccelerate(carrySpeed))
				yield return SetState(State.CarryIdle);
			if (TryThrow())
				yield return SetState(State.Idle);
			yield return 0;
		}
	}
	
	void LiftEnd(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		animator.AnimationCompleted = null;
		carrying.StartCarry();
		SetState(State.CarryIdle);
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
	
	bool TryAccelerate(float maxSpeed)
	{
		if (!moveAxis.IsZero())
		{
			velocity = Vector3.MoveTowards(velocity, moveAxis * maxSpeed, acceleration * Time.deltaTime);
			return true;
		}
		else
		{
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
					carrying.StartLift(transform);
					//carrying.StartCarry(transform);
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
