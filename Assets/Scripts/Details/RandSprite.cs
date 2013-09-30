using UnityEngine;
using System.Collections;

public class RandSprite : MonoBehaviour {
	
	public string prefix;
	public int count;
	public bool randFlipX;
	public bool randFlipY;
	public float staggerX;
	public float staggerZ;
	
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
		
		sprite.transform.SetX(sprite.transform.localPosition.x - staggerX + Rand.Float(staggerX * 2));
		sprite.transform.SetZ(sprite.transform.localPosition.z - staggerZ + Rand.Float(staggerZ * 2));
	}

}
