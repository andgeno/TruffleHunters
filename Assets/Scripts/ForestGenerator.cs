using UnityEngine;
using System.Collections;

public class ForestGenerator : MonoBehaviour 
{
	
	public Vector2 size;
	[Range(0f, 1f)] public float treeThreshold;
	[Range(0f, 1f)] public float treePerlinScale;
	[Range(0f, 1f)] public float treeInnerPercentage;
	[Range(0f, 1f)] public float treeOuterPercentage;
	
	public Vector2 fencePadding;
	public float hFencePerMeter;
	public float vFencePerMeter;
	
	public float grassTurfPerMeter;
	
	public PolyMesh ground;
	public PolyMesh sky;
	public Transform treePrefab;
	public Transform rockPrefab;
	public Transform hFencePrefab;
	public Transform vFencePrefab;
	public Transform grassTurfPrefab;
	
	public Transform treeRoot;
	public Transform rockRoot;
	public Transform grassRoot;
	public Transform fenceRoot;
	
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
		
		// add the sky
		sky.keyPoints[0] = new Vector3(size.x, 10);
		sky.keyPoints[1] = new Vector3(size.x, 0);
		sky.keyPoints[2] = new Vector3(-size.x, 0);
		sky.keyPoints[3] = new Vector3(-size.x, 10);
		sky.BuildMesh();
		sky.transform.position = new Vector3(0, 0, size.y);
		
		// generate trees
		Vector2 treePerlinOffset = new Vector2(Rand.Float(10000), Rand.Float(10000));
		for (float i = -size.x; i < size.x; i ++)
		{
			for (float j = - size.y; j < size.y; j += 2)
			{
				float sample = Mathf.PerlinNoise(treePerlinOffset.x + i * treePerlinScale, treePerlinOffset.y + j * treePerlinScale);
				Vector3 stagger = new Vector3(Rand.Float(-0.50f, 0.50f), 0, Rand.Float(-0.50f, 0.50f));
				
				if (treeThreshold > sample)
				{
					
					if (Rand.Chance(treeInnerPercentage))
						treePrefab.Spawn(new Vector3(i, 0, j) + stagger).parent = treeRoot;
				}
				else
				{
					if (sample - treeThreshold < 0.01f)
						rockPrefab.Spawn(new Vector3(i, 0, j) + stagger).parent = rockRoot;
					
					if (Rand.Chance(treeOuterPercentage))
						treePrefab.Spawn(new Vector3(i, 0, j) + stagger).parent = treeRoot;
				}
			}
		}
		
		// place random grass turf
		float grassTurfCount = size.x * 2 * size.y * 2 * grassTurfPerMeter;
		for (int i = 0; i < grassTurfCount; i ++)
			grassTurfPrefab.Spawn(new Vector3(Rand.Float(-size.x, size.x), 0, Rand.Float(-size.y, size.y))).parent = grassRoot;
		
		// spawn the perimeter
		for (float i = - size.x + fencePadding.x; i < size.x - fencePadding.x; i += hFencePerMeter)
		{
			hFencePrefab.Spawn(new Vector3(i, 0, -size.y + fencePadding.y)).parent = fenceRoot;
			hFencePrefab.Spawn(new Vector3(i, 0, size.y - fencePadding.y)).parent = fenceRoot;
		}
		for (float i = - size.y + fencePadding.y; i < size.y - fencePadding.y; i += vFencePerMeter)
		{
			vFencePrefab.Spawn(new Vector3(-size.x + fencePadding.x, 0, i)).parent = fenceRoot;
			vFencePrefab.Spawn(new Vector3(size.x - fencePadding.x, 0, i)).parent = fenceRoot;
		}
		
		yield return 0;
	}
}
