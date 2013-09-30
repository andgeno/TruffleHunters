using UnityEngine;
using System.Collections;

public class VisibilityObject : MonoBehaviour
{
	bool visible = true;
	Renderer[] renderers;
	
	void Awake()
	{
		renderers = GetComponentsInChildren<Renderer>();
	}
	
	void OnEnable()
	{
		VisibilityManager.instance.objects.Add(this);
	}
	
	void OnDisable()
	{
		VisibilityManager.instance.objects.Remove(this);
	}
	
	public void SetVisible(bool visible)
	{
		if (this.visible != visible)
		{
			this.visible = visible;
			foreach (var renderer in renderers)
				renderer.enabled = visible;
		}
	}
}
