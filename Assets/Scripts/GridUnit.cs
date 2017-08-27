using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUnit : MonoBehaviour
{

	public int row;
	public int col;
	public bool canNavigateTo = true;

	private NavGrid script;

	void OnDrawGizmos()
	{
		script = gameObject.GetComponentInParent<NavGrid> ();
		if (script.drawAlways) {
			DrawGrid ();
		}
	}

	void OnDrawGizmosSelected()
	{
		script = gameObject.GetComponentInParent<NavGrid> ();
		if (!script.drawAlways) {
			DrawGrid ();
		}
	}

	void DrawGrid()
	{
		if (canNavigateTo) {
			Gizmos.color = new Color (0f, 1f, 0f, 0.4f); // green
		} else {
			Gizmos.color = new Color (1f, 0f, 0f, 0.4f); // red
		}
		Gizmos.DrawCube (transform.position, new Vector3 (script.gridSize, script.gridSize, 0f));

	}
}
