using UnityEngine;
using System.Collections;

public class RandSprite : MonoBehaviour {
	
	public string prefix;
	public int count;
	public bool randFlipX = false;
	public bool randFlipY = false;
	
	tk2dSprite sprite;
	
	// Use this for initialization
	void Start ()
	{
		sprite = GetComponent<tk2dSprite>();
		sprite.SetSprite(prefix + (count > 0 ? "" + Rand.Int(count) : ""));
		
		if (randFlipX)
			sprite.FlipX = Rand.Int(2) == 0;
		if (randFlipY)
			sprite.FlipY = Rand.Int(2) == 0;
	}

}
