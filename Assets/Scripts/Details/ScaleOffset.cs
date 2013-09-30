using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaleOffset : MonoBehaviour 
{	
	public float scaleOffsetMin;
	public float scaleOffsetMax;
	
	// Use this for initialization
	void Start () 
	{
		float scaleBy = Rand.Float(scaleOffsetMin, scaleOffsetMax);
		transform.localScale = new Vector3(scaleBy, scaleBy, 1);
	}
}
