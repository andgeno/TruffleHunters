using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExitZone : Singleton<ExitZone>
{
	public List<Pig> pigs = new List<Pig>();
	
	void OnTriggerEnter(Collider other)
	{
		var pig = other.GetComponent<Pig>();
		if (pig != null)
			pigs.Add(pig);
	}
	
	void OnTriggerExit(Collider other)
	{
		var pig = other.GetComponent<Pig>();
		if (pig != null)
			pigs.Remove(pig);
	}
}
