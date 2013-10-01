using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

public enum GamePhase { BeforeHunt, Hunt, AfterHunt }

public class Game : Singleton<Game>
{
	static XmlSerializer serializer;
	
	public static GamePhase phase = GamePhase.BeforeHunt;
	
	public GameData data = new GameData();
	
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
		while (true)
		{
			yield return 0;
		}
	}
	
	IEnumerator AfterHunt()
	{
		while (true)
		{
			yield return 0;
		}
	}
	
	public static void Save()
	{
		if (serializer == null)
			serializer = new XmlSerializer(typeof(GameData));
		var writer = XmlWriter.Create("GameData.xml");
		serializer.Serialize(writer, instance.data);
	}
	
	public static void Load()
	{
		if (serializer == null)
			serializer = new XmlSerializer(typeof(GameData));
		var reader = XmlReader.Create("GameData.xml");
		instance.data = (GameData)serializer.Deserialize(reader);
	}
}
