using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavGrid))]
public class NavGridEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		NavGrid myScript = (NavGrid)target;
		if(GUILayout.Button("Build Grid"))
		{
			myScript.CreateGrid();
		}
	}
}