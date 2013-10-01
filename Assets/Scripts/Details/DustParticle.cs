using UnityEngine;
using System.Collections;

public class DustParticle : MonoBehaviour 
{
	
	public float distance;
	public float duration;
	public float scaleTo;
	tk2dSprite sprite;
	
	void Start()
	{
		
	}
	
	public static void Create(DustParticle prefab, Transform parent, Vector3 position)
	{
		for (int i = 0; i < 2; i ++)
		{
			var particle = prefab.Spawn();
			particle.transform.parent = parent;
			particle.transform.localPosition = position;
			particle.StartCoroutine(particle.BeginMove(i == 0 ? -1 : 1));	
		}
	}

	IEnumerator BeginMove(int direction)
	{
		if (sprite == null)
			sprite = GetComponentInChildren<tk2dSprite>();
		
		sprite.transform.localRotation = Quaternion.Euler(0, 0, Rand.Float(360));
		StartCoroutine(sprite.transform.RotateTo(Quaternion.Euler(0, 0, Rand.Float(360)), duration));
		
		StartCoroutine(FadeTo());
		StartCoroutine(transform.ScaleTo(new Vector3(scaleTo, scaleTo, 1), duration));
		
		Vector3 control = transform.localPosition + new Vector3(direction * distance / 2, -.2f, 0);
		Vector3 target = transform.localPosition + new Vector3(direction * distance, 0.25f, 0);
		yield return StartCoroutine(transform.CurveTo(control, target, duration, EaseType.CubeOut));
		
		this.Recycle();
	}
	
	IEnumerator FadeTo()
	{
		sprite.color = new Color(1, 1, 1, 1);
		for (float time = 0; time < duration; time += Time.deltaTime)
		{
			sprite.color = new Color(1, 1, 1, 1 - (time / duration));
			yield return 0;
		}
	}
}
