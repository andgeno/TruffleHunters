using UnityEngine;
using System.Collections;

public static class Ref
{
	public static void Swap<T>(ref T a, ref T b)
	{
		var temp = a;
		a = b;
		b = temp;
	}
	
	public static bool Alternate(ref bool toggle)
	{
		toggle = !toggle;
		return !toggle;
	}
}
