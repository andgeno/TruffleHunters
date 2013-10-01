using UnityEngine;
using System.Collections;

public class PigSpawner : MonoBehaviour
{
	public Pig pigPrefab;
	
	void Start()
	{
		if (renderer != null)
			Destroy(renderer);
		
		var scale = transform.localScale / 2;
		for (int i = 0; i < Game.instance.data.pigs.Count; i++)
			Pig.Spawn(pigPrefab, i, transform.position + new Vector3(Rand.Float(-scale.x, scale.x), 0, Rand.Float(-scale.z, scale.z)));
	}
}
