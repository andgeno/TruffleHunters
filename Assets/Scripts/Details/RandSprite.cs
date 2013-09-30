using UnityEngine;
using System.Collections;

public class RandSprite : MonoBehaviour {
	
	public string prefix;
	public int count;
	
	tk2dSprite sprite;
	
	// Use this for initialization
	void Start ()
	{
		sprite = GetComponent<tk2dSprite>();
		sprite.SetSprite(prefix + Rand.Int(count));
	}

}
