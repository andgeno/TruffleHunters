using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

public enum GamePhase { BeforeHunt, Hunt, AfterHunt, ExitGame }

public class Game : Singleton<Game>
{
	static XmlSerializer serializer;
	
	public static GamePhase phase = GamePhase.BeforeHunt;
	
	public GameData data = new GameData();
	public int startingPigs;
	public float huntDuration;
	
	protected override void Awake()
	{
		base.Awake();
		Load();
	}
	
	void Start()
	{
		StartCoroutine(phase.ToString());
	}
	
	IEnumerator BeforeHunt()
	{
		while (true)
		{
			yield return 0;
		}
	}
	
	IEnumerator Hunt()
	{
		var time = 0f;
		while (time < huntDuration)
		{
			time = Mathf.MoveTowards(time, huntDuration, Time.deltaTime);
			yield return 0;
		}
		GameCamera.instance.FadeToScene("FarmHouse", GamePhase.AfterHunt);
	}
	
	IEnumerator AfterHunt()
	{
		while (true)
		{
			yield return 0;
		}
	}
	
	PigData CreateNewPig()
	{
		var pig = new PigData();
		pig.name = GetComponent<NameGenerator>().GetName();
		
		pig.weightTier = Rand.Int(0, 3);
		if (pig.weightTier == 0)
			pig.weight = Rand.Int(16, 25);
		else if (pig.weightTier == 1)
			pig.weight = Rand.Int(26, 35);
		else
			pig.weight = Rand.Int(36, 45);
		
		pig.speed = Calc.Map(pig.weight, 16, 44, 2, 1);
		pig.smellRange = Rand.Float(8, 12);
		
		return pig;
	}
	
	void NewGame()
	{
		while (data.pigs.Count < startingPigs)
			data.pigs.Add(CreateNewPig());
		//Save();
	}
	
	public void Save()
	{
		if (serializer == null)
			serializer = new XmlSerializer(typeof(GameData));
		var writer = XmlWriter.Create("GameData.xml");
		serializer.Serialize(writer, instance.data);
	}
	
	public void Load()
	{
		if (serializer == null)
			serializer = new XmlSerializer(typeof(GameData));
		if (System.IO.File.Exists("GameData.xml"))
		{
			var reader = XmlReader.Create("GameData.xml");
			instance.data = (GameData)serializer.Deserialize(reader);
		}
		else
			NewGame();
	}
}
