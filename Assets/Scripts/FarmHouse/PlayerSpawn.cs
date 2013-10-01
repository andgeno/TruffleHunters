using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour
{
	public GamePhase phase;
	public Player playerPrefab;
	
	void Awake()
	{
		if (renderer != null)
			Destroy(renderer);
		if (Game.phase == phase)
			playerPrefab.Spawn(transform.position);
	}
}
