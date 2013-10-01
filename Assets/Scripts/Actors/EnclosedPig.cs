using UnityEngine;
using System.Collections;

public class EnclosedPig : MonoBehaviour
{
	public tk2dSpriteAnimator animator;
	public float minPauseTime;
	public float maxPauseTime;
	public float wanderRadius;
	public LayerMask obstacleMask;
	
	CharacterController controller;
	Pig pig;
	
	void Start()
	{
		controller = GetComponent<CharacterController>();
		pig = GetComponent<Pig>();
		
		StartCoroutine(Wander());
	}
	
	IEnumerator Wander()
	{
		Vector3 target;
		while (true)
		{
			animator.Play("PigIdleSide");
			
			yield return StartCoroutine(Auto.Wait(Rand.Float(minPauseTime, maxPauseTime)));
			
			while (!TryFindTargetPosition(out target))
				yield return 0;
			
			animator.Play("PigWalkSide");
			
			while (!transform.IsAt(target))
			{
				if (transform.position.x - target.x < 0)
					animator.Sprite.FlipX = true;
				else
					animator.Sprite.FlipX = false;
				
				transform.MoveTowards(target, pig.data.speed * Time.deltaTime);
				yield return 0;
			}
		}
	}
	
	void OnStartLift()
	{
		StopAllCoroutines();
		animator.Play("PigPickup");
	}
	
	void OnStartCarry()
	{
		animator.Play("PigCarry");
	}
	
	void OnThrowEnd()
	{
		StartCoroutine(Wander());
	}
	
	bool TryFindTargetPosition(out Vector3 target)
	{
		var offset = new Vector3(Rand.Float(wanderRadius), 0);
		offset = Quaternion.Euler(0, Rand.angle, 0) * offset;
		offset.y *= 2;
		target = transform.position + offset;
		var ray = new Ray(transform.position, offset.normalized);
		return !Physics.SphereCast(ray, controller.radius, offset.magnitude, obstacleMask);
	}
}
