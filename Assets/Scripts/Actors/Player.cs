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
	tk2dSpriteAnimator animator;
	
	protected override void Awake ()
	{
		base.Awake();
		
		controller = GetComponent<CharacterController>();
		animator = GetComponentInChildren<tk2dSpriteAnimator>();
		
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
		StopAllCoroutines();
		state = newState;
		StartCoroutine(state.ToString());
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
