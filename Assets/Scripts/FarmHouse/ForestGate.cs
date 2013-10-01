using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Gate))]
public class ForestGate : MonoBehaviour
{
	void Start()
	{
		if (Game.phase != GamePhase.BeforeHunt)
			collider.enabled = false;
	}
}
