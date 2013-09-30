using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Batch
{
	public static GameObject CombineMeshes(GameObject root)
	{
		//Group filters based on material
		var groups = new Dictionary<string, List<MeshFilter>>();
		var materials = new Dictionary<string, Material>();
		var allFilters = root.GetComponentsInChildren<MeshFilter>();
		foreach (var filter in allFilters)
		{
			var material = filter.renderer.sharedMaterial;
			if (!groups.ContainsKey(material.name))
			{
				groups.Add(material.name, new List<MeshFilter>());
				materials.Add(material.name, material);
			}
			groups[material.name].Add(filter);
		}

		//Create the new root
		var newRoot = new GameObject(root.name + " (Combined)");

		//Combine each group into a mesh
		foreach (var group in groups)
		{
			//Create an object inside the new root
			var obj = new GameObject(group.Key);
			obj.transform.parent = newRoot.transform;
			obj.transform.localPosition = Vector3.zero;

			//Create combination array
			var combines = new CombineInstance[group.Value.Count];
			for (int i = 0; i < combines.Length; i++)
			{
				combines[i].mesh = group.Value[i].sharedMesh;
				combines[i].transform = group.Value[i].transform.localToWorldMatrix;
			}

			//Create the combined mesh
			var mesh = new Mesh();
			mesh.name = group.Key;
			mesh.CombineMeshes(combines);
			mesh.Optimize();

			//Add the mesh to the object
			var filter = obj.AddComponent<MeshFilter>();
			filter.mesh = mesh;

			//Add the renderer to the object
			var renderer = obj.AddComponent<MeshRenderer>();
			renderer.material = materials[group.Key];
		}

		//Deactivate the old object and return the new
		root.SetActive(false);
		return newRoot;
	}
}
