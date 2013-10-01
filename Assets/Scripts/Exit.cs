using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour
{
	public string scene;
	public GamePhase phase;
	
	void Awake()
	{
		if (renderer != null)
			Destroy(renderer);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Player>() != null)
		{
			if(phase==GamePhase.ExitGame)
			{
				Debug.Log("Goodbye!");
				Application.Quit();
			}
			else
			{
				GameCamera.instance.FadeToScene(scene, phase);
			}
			enabled = false;
		}
	}
}
