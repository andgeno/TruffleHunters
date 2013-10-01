using UnityEngine;
using System.Collections;

public class GameCamera : Singleton<GameCamera>
{
	public Texture pixel;
	public float fadeTime;
	[HideInInspector] public Color color;
	
	void Start()
	{
		Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
		if (Player.exists)
			FollowTarget(Player.transform);
		StartCoroutine(FadeIn());
	}
	
	public void FollowTarget(Transform target)
	{
		StopAllCoroutines();
		StartCoroutine(DoFollowTarget(target));
	}
	IEnumerator DoFollowTarget(Transform target)
	{
		while (true)
		{
			transform.position = target.position;
			yield return 0;
		}
	}
	
	public void FadeToScene(string name)
	{
		StopAllCoroutines();
		StartCoroutine(DoFadeToScene(name));
	}
	IEnumerator DoFadeToScene(string name)
	{
		color = new Color(0, 0, 0, 0);
		var fadeSpeed = 1 / fadeTime;
		while (!Mathf.Approximately(color.a, 1))
		{
			color.a = Mathf.MoveTowards(color.a, 1, fadeSpeed * Time.deltaTime);
			yield return 0;
		}
		Application.LoadLevel(name);
	}
	
	IEnumerator FadeIn()
	{
		color = new Color(0, 0, 0, 1);
		var fadeSpeed = 1 / fadeTime;
		while (!Mathf.Approximately(color.a, 0))
		{
			color.a = Mathf.MoveTowards(color.a, 0, fadeSpeed * Time.deltaTime);
			yield return 0;
		}
	}
	
	void OnGUI()
	{
		if (!Mathf.Approximately(color.a, 0))
		{
			GUI.color = color;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), pixel);
		}
	}
}
