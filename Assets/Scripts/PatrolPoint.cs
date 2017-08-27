using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PatrolPoint : MonoBehaviour {

	public float searchInSecs = 3f;
	public GridUnit gridUnit;

	void Update ()
	{
		Collider2D col = Physics2D.OverlapCircle (transform.position, 0.01f);
		if (col) {
			if (col.name.StartsWith ("GridUnit")) {
				gridUnit = col.GetComponent<GridUnit> ();
			}
		}
	}
}
