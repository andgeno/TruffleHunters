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
						treePrefab.Spawn(new Vector3(i, 0, j) + stagger);
				}
				else if (Rand.Chance(treeOuterPercentage))
					treePrefab.Spawn(new Vector3(i, 0, j) + stagger);
					
			}
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
