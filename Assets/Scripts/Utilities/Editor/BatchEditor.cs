using UnityEngine;
using UnityEditor;
using System.Collections;

public static class BatchEditor
{
	[MenuItem("GameObject/Combine Meshes")]
	static void CombineMeshes()
	{
		Batch.CombineMeshes(Selection.activeGameObject);
	}
	[MenuItem("GameObject/Combine Meshes", true)]
	static bool ValidateCombineMeshes()
	{
		return Selection.activeGameObject != null;
	}
}
