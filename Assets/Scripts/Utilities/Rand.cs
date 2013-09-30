using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Rand
{
	public static T Choose<T>(T a, T b)
	{
		if (Random.value < 0.5f)
			return a;
		return b;
	}
	public static T Choose<T>(T a, T b, T c)
	{
		if (Random.value < 1f / 3)
			return a;
		if (Random.value < 1f / 2)
			return b;
		return c;
	}
	public static T Choose<T>(T a, T b, T c, T d)
	{
		if (Random.value < 1f / 4)
			return a;
		if (Random.value < 1f / 3)
			return b;
		if (Random.value < 1f / 2)
			return c;
		return d;
	}
	public static T Choose<T>(T a, T b, T c, T d, T e)
	{
		if (Random.value < 1f / 5)
			return a;
		if (Random.value < 1f / 4)
			return b;
		if (Random.value < 1f / 3)
			return c;
		if (Random.value < 1f / 2)
			return d;
		return e;
	}

	public static Vector3 Direction()
	{
		return direction;
	}
	public static Vector3 Direction(float upAngle)
	{
		return Vector3.RotateTowards(direction, Vector3.up, upAngle * Mathf.Deg2Rad, 0);
	}
	public static Vector3 Direction(float minUpAngle, float maxUpAngle)
	{
		return Vector3.RotateTowards(direction, Vector3.up, Random.Range(minUpAngle, maxUpAngle) * Mathf.Deg2Rad, 0);
	}

	public static int Int(int max)
	{
		return Random.Range(0, max);
	}
	public static int Int(int min, int max)
	{
		return Random.Range(min, max);
	}

	public static float Float(float max)
	{
		return Random.Range(0, max);
	}
	public static float Float(float min, float max)
	{
		return Random.Range(min, max);
	}
	
	public static bool Chance(float percent)
	{
		return Random.value < percent;
	}

	public static float value
	{
		get { return Random.value; }
	}

	public static float angle
	{
		get { return Random.Range(0f, 360f); }
	}

	public static Vector3 direction
	{
		get
		{
			var angle = Random.value * Mathf.PI * 2f;
			return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
		}
	}
}