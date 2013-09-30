using UnityEngine;
using System.Collections;

public class ForestGenerator : MonoBehaviour 
{
	
	public Vector2 size;
	[Range(0f, 0.1f)] public float treesPerMeter;
	[Range(0f, 0.1f)] public float rocksPerMeter;
	public Vector2 fencePadding;
	public float hFencePerMeter;
	public float vFencePerMeter;
	
	public PolyMesh ground;
	public Transform treePrefab;
	public Transform rockPrefab;
	public Transform hFencePrefab;
	public Transform vFencePrefab;
	
	void Awake()
	{
	}
	
	void Start()
	{
		StartCoroutine(Generate());	
	}
	
	IEnumerator Generate()
	{
		// generate the ground
		ground.keyPoints[0] = new Vector3(size.x, size.y);
		ground.keyPoints[1] = new Vector3(size.x , -size.y);
		ground.keyPoints[2] = new Vector3(-size.x, -size.y);
		ground.keyPoints[3] = new Vector3(-size.x, size.y);
		ground.BuildMesh();
		
		// get the total amount of trees
		float trees = size.x * 2 * size.y * 2 * treesPerMeter;
		for (int i = 0; i < trees; i ++)
		{
			treePrefab.Spawn(new Vector3(Rand.Float(-size.x, size.x), 0, Rand.Float(-size.y, size.y)));	
		}
		
		float rocks = size.x * 2 * size.y * 2 * rocksPerMeter;
		for (int i = 0; i < rocks; i ++)
		{
			rockPrefab.Spawn(new Vector3(Rand.Float(-size.x, size.x), 0, Rand.Float(-size.y, size.y)));	
		}
		
		// spawn the perimeter
		for (float i = - size.x + fencePadding.x; i < size.x - fencePadding.x; i += hFencePerMeter)
		{
			hFencePrefab.Spawn(new Vector3(i, 0, -size.y + fencePadding.y));
			hFencePrefab.Spawn(new Vector3(i, 0, size.y - fencePadding.y));
		}
		for (float i = - size.y + fencePadding.y; i < size.y - fencePadding.y; i += vFencePerMeter)
		{
			vFencePrefab.Spawn(new Vector3(-size.x + fencePadding.x, 0, i));
			vFencePrefab.Spawn(new Vector3(size.x - fencePadding.x, 0, i));
		}
		
		yield return 0;
	}
}
