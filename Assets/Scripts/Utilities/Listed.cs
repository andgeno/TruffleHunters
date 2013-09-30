using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Listed<T> : MonoBehaviour where T : MonoBehaviour
{
	static List<T> list = new List<T>();
	static bool removeNulls;
	
	protected virtual void OnEnable()
	{
		if (removeNulls)
		{
			list.RemoveNulls();
			removeNulls = false;
		}
		list.Add(this as T);
	}

	protected virtual void OnDisable()
	{
		list.Remove(this as T);
	}

	protected virtual void OnDestroy()
	{
		removeNulls = true;
	}

	public static void DestroyAll()
	{
		while (list.Count > 0)
			list[0].Recycle();
	}

	public static T Get(int index)
	{
		return list[index];
	}

	public static int count
	{
		get { return list.Count; }
	}

	public static IEnumerable<T> collection
	{
		get { return list; }
	}
}
