using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Player>() != null)
		{
			
			enabled = false;
		}
	}
}
