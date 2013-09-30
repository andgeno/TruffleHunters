using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	void LateUpdate()
	{
		transform.rotation = Camera.main.transform.rotation;	
	}
}
