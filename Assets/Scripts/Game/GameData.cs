using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
	public int day = 0;
	public int money = 0;
	public List<PigData> pigs = new List<PigData>();
}

[System.Serializable]
public class PigData
{
	public string name;
	public int age;
	public int weightTier;
	public int weight;
	public float speed;
	public float smellRange;
	
}