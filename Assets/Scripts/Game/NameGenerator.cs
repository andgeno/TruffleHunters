using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NameGenerator : MonoBehaviour
{
	static List<string> maleNames = new List<string>();
	static List<string> femaleNames = new List<string>();
	
	public TextAsset maleNamesAsset;
	public TextAsset femaleNamesAsset;
	
	public string GetMaleName()
	{
		if (maleNames.Count == 0)
			maleNames.AddRange(maleNamesAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries));
		return maleNames.Pop(Rand.Int(maleNames.Count));
	}
	
	public string GetFemaleName()
	{
		if (femaleNames.Count == 0)
			femaleNames.AddRange(femaleNamesAsset.text.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries));
		return femaleNames.Pop(Rand.Int(femaleNames.Count));
	}
	
	public string GetName()
	{
		if (Rand.Chance(0.5f))
			return GetMaleName();
		else
			return GetFemaleName();
	}
}
