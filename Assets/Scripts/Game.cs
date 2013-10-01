using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

public class Game : Singleton<Game>
{
	static XmlSerializer serializer;
	
	public GameData data = new GameData();
	
	protected override void Awake()
	{
		base.Awake();
		Load();
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
