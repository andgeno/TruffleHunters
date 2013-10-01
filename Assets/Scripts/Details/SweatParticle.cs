using UnityEngine;
using System.Collections;

public class SweatParticle : MonoBehaviour 
{
	
	public static void Create(SweatParticle prefab, Transform parent, Vector3 position)
	{
		var particle = prefab.Spawn();
		particle.transform.parent = parent;
		particle.transform.localPosition = position;
		particle.StartCoroutine(particle.Move());
	}
	
	IEnumerator Move()
	{
		int direction = Rand.Choose(-1, 1);
		
		Vector3 control = transform.localPosition + new Vector3(direction * 0.50f, 1f, 0);
		Vector3 target = transform.localPosition + new Vector3(direction * 1, 0, 0);
		yield return StartCoroutine(transform.CurveTo(control, target, 0.50f));
		
		this.Recycle();
	}
}
