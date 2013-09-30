using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisibilityManager : Singleton<VisibilityManager>
{
	public float updateFrequency = 0.1f;
	[HideInInspector] public List<VisibilityObject> objects = new List<VisibilityObject>();
	
	void Start()
	{
		StartCoroutine(UpdateVisibility());
	}
	
	IEnumerator UpdateVisibility()
	{
		while (true)
		{
			yield return StartCoroutine(Auto.Wait(updateFrequency));
			foreach (var obj in objects)
			{
				var pos = Camera.main.WorldToScreenPoint(obj.transform.localPosition);
				obj.SetVisible(pos.y > -50);
			}
		}
	}
}
