using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestGenerator : MonoBehaviour 
{
	
	// generation details
	public Vector2 size;
	[Range(0f, 1f)] public float perlinScale;
	[Range(0f, 1f)] public float treeThreshold;
	[Range(0f, 1f)] public float treeInnerPercentage;
	[Range(0f, 1f)] public float treeOuterPercentage;
	
	// amount of stuff
	public Vector2 fencePadding;
	public float hFencePerMeter;
	public float vFencePerMeter;
	public float grassTurfPerMeter;
	
	// mushrooms
	public int totalMushrooms;
	
	// prefabs
	public Transform playerPrefab;
	public PolyMesh ground;
	public Transform treePrefab;
	public Transform deadTreePrefab;
	public Transform stumpPrefab;
	public Transform rockPrefab;
	public Transform hFencePrefab;
	public Transform vFencePrefab;
	public Transform grassTurfPrefab;
	public Transform gatePrefab;
	public Transform mushroomPrefab;
	
	// roots
	public Transform treeRoot;
	public Transform rockRoot;
	public Transform grassRoot;
	public Transform fenceRoot;
	
	// internal
	Vector2 perlinOffset;
	
	void Awake()
	{
		perlinOffset = new Vector2(Rand.Float(10000), Rand.Float(10000));
		
		// spawn the player
		playerPrefab.Spawn(new Vector3(0, 0, size.y - fencePadding.y));
	}
	
	void Start()
	{
		StartCoroutine(Generate());	
	}
	
	IEnumerator Generate()
	{
		// generate the ground
		ground.keyPoints[0] = new Vector3(size.x + 20, size.y + 20);
		ground.keyPoints[1] = new Vector3(size.x + 20 , -size.y - 20);
		ground.keyPoints[2] = new Vector3(-size.x - 20, -size.y - 20);
		ground.keyPoints[3] = new Vector3(-size.x - 20, size.y + 20);
		ground.BuildMesh();
		
		// top perimeter
		for (float i = 2; i < size.x - fencePadding.x; i += hFencePerMeter)
			hFencePrefab.Spawn(new Vector3(i, 0, size.y - fencePadding.y)).parent = fenceRoot;
		for (float i = -2 - hFencePerMeter; i > -(size.x - fencePadding.x + hFencePerMeter); i -= hFencePerMeter)
			hFencePrefab.Spawn(new Vector3(i, 0, size.y - fencePadding.y)).parent = fenceRoot;
		
		// spawn the bottom perimeter
		for (float i = - size.x + fencePadding.x; i < size.x - fencePadding.x; i += hFencePerMeter)
			hFencePrefab.Spawn(new Vector3(i, 0, -size.y + fencePadding.y)).parent = fenceRoot;
		
		// side parimeters
		for (float i = - size.y + fencePadding.y; i < size.y - fencePadding.y; i += vFencePerMeter)
		{
			vFencePrefab.Spawn(new Vector3(-size.x + fencePadding.x, 0, i)).parent = fenceRoot;
			vFencePrefab.Spawn(new Vector3(size.x - fencePadding.x, 0, i)).parent = fenceRoot;
		}
		
		// place edge entrace fence
		for (float i = 0; i < fencePadding.y + vFencePerMeter; i += vFencePerMeter)
		{
			vFencePrefab.Spawn(new Vector3(-2, 0, size.y - i)).parent = fenceRoot;	
			vFencePrefab.Spawn(new Vector3(2, 0, size.y - i)).parent = fenceRoot;	
		}
		
		gatePrefab.Spawn (new Vector3(-1.5f, 0, size.y));
		
		// generate trees and rocks
		for (float i = -size.x; i < size.x; i ++)
		{
			for (float j = - size.y; j < size.y + 20; j += 2)
			{
				// don't spawn near entrance
				if (!(j > size.y - 20 && i > -4 && i < 4))
				{
					// make sure the root of the tree/rock doesn't overlap the fence
					float fenceDistX = 0.60f;
					float fenceDistY = 0.10f;
					
					// don't spawn near fence
					if ((i < -size.x + fencePadding.x - fenceDistX || i > -size.x + fencePadding.x + fenceDistX)
						&& (i < size.x - fencePadding.x - fenceDistX || i > size.x - fencePadding.x + fenceDistX)
						&& (j < -size.y + fencePadding.y - fenceDistY || j > -size.y + fencePadding.y + fenceDistY)
						&& (j < size.y - fencePadding.y - fenceDistY || j > size.y - fencePadding.y + fenceDistY))
					{
						float sample = GetPerlinSample(i, j);
						Vector3 stagger = new Vector3(Rand.Float(-0.50f, 0.50f), 0, Rand.Float(-0.50f, 0.50f));
						
						if (treeThreshold > sample)
						{
							if (Rand.Chance(treeInnerPercentage))
								SetTree (new Vector3(i, 0, j) + stagger);
						}
						else
						{
							if (sample - treeThreshold < 0.01f)
								rockPrefab.Spawn(new Vector3(i, 0, j) + stagger).parent = rockRoot;
							
							if (Rand.Chance(treeOuterPercentage))
								SetTree (new Vector3(i, 0, j) + stagger);
						}
					}
				}
			}
		}
		
		// place random grass turf
		float grassTurfCount = size.x * 2 * size.y * 2 * grassTurfPerMeter;
		for (int i = 0; i < grassTurfCount; i ++)
			grassTurfPrefab.Spawn(new Vector3(Rand.Float(-size.x, size.x), -.25f, Rand.Float(-size.y, size.y))).parent = grassRoot;
		
		// generate mushroom areas
		List<Vector3> mushroomSpawns = new List<Vector3>();
		
		for (float i = -size.x + fencePadding.x; i < size.x - fencePadding.x; i += 3)
		{
			for (float j = -size.y + fencePadding.y; j < size.y - fencePadding.y; j += 5)
			{
				// don't spawn in entrance
				if (!(j > size.y - 20 && i > -6 && i < 6))
				{
					// only spawn mushrooms in non-tree areas
					float sample = GetPerlinSample(i, j);
					float difference = Mathf.Abs(sample - treeThreshold);
					if (treeThreshold < sample && (difference > 0.05f && difference < 0.1f))
					{
						mushroomSpawns.Add (new Vector3(i, 0, j));
					}
				}
			}
		}
		
		// place the mushrooms
		for (int i = 0; i < totalMushrooms; i ++)
		{
			Vector3 spawn = mushroomSpawns.Choose();
			mushroomSpawns.Remove(spawn);
			mushroomPrefab.Spawn(spawn);
		}
		
		yield return 0;
	}
	
	float GetPerlinSample(float x, float y)
	{
		return Mathf.PerlinNoise(perlinOffset.x + x * perlinScale, perlinOffset.y + y * perlinScale);	
	}
	
	void SetTree(Vector3 position)
	{
		if (Rand.Chance(0.90f))
			treePrefab.Spawn(position).parent = treeRoot;
		else if (Rand.Chance(0.50f))
			deadTreePrefab.Spawn(position + new Vector3(0, -0.15f)).parent = treeRoot;
		else
			stumpPrefab.Spawn(position + new Vector3(0, -0.22f)).parent = treeRoot;
	}
}
