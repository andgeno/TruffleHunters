using UnityEngine;
using System.Collections;

public class GameCamera : Singleton<GameCamera>
{
	void Start()
	{
		if (Player.exists)
			FollowTarget(Player.transform);
	}
	
	public void FollowTarget(Transform target)
	{
		StartCoroutine(DoFollowTarget(target));
	}
	
	IEnumerator DoFollowTarget(Transform target)
	{
		while (true)
		{
			transform.position = target.position;
			yield return 0;
		}
	}
}
