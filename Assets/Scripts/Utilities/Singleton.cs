using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	static T _instance;

	protected virtual void Awake()
	{
		_instance = this as T;
	}

	public static T instance
	{
		get
		{
			if (_instance != null)
				return _instance;
			var obj = new GameObject("_" + typeof(T).Name);
			return obj.AddComponent<T>();
		}
	}

	#pragma warning disable 0108
	public static Transform transform
	{
		get { return instance.transform; }
	}
	#pragma warning restore 0108
}
