using UnityEngine;
using System.Collections;

public class AfterHuntStats : MonoBehaviour {
	public TextMesh totalMushrooms;
	public TextMesh totalGarbage;
	public TextMesh totalTotal;
	
	// Use this for initialization
	IEnumerator Start () {
		if (Game.phase == GamePhase.AfterHunt)
		{
			Vector3 defaultScale = totalTotal.transform.localScale;
			transform.localScale=Vector3.zero;
			totalTotal.transform.localScale=Vector3.zero;
			totalMushrooms.transform.localScale=Vector3.zero;
			totalGarbage.transform.localScale=Vector3.zero;
			
			yield return new WaitForSeconds(0.5f);
			
			StartCoroutine(transform.MoveTo(transform.position + new Vector3(0,1f,0),0.25f,Ease.CubeInOut));
			yield return StartCoroutine(transform.ScaleTo(Vector3.one,1.2f,Ease.ElasticOut));
			
			StartCoroutine(totalMushrooms.transform.ScaleTo(defaultScale,0.9f,Ease.ElasticOut));
			yield return new WaitForSeconds(0.1f);
			StartCoroutine(totalGarbage.transform.ScaleTo(defaultScale,0.9f,Ease.ElasticOut));
			yield return new WaitForSeconds(0.1f);
			StartCoroutine(totalTotal.transform.ScaleTo(defaultScale,0.9f,Ease.ElasticOut));
			yield return new WaitForSeconds(2.0f);
			
			StartCoroutine(transform.MoveTo(transform.position + new Vector3(0,-1.5f,0),0.25f,Ease.CubeInOut));
			yield return StartCoroutine(transform.ScaleTo(new Vector3(1,0,1),0.25f,Ease.CubeIn));
		}
		Destroy(gameObject);
	}
}
