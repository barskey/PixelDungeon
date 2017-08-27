using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float maxSpeed = 1f; // max speed, used when rushing for attack
	public float secsBtwnAttack = 1f; // seconds between attacks
	public float seeAngle = 30f; // angle in degrees for how wide enemy can see player
	public float seeDist = 0.3f; // how far can see player
	public float attackDist = 0.3f; // distance to keep while attacking
	public LayerMask cantSeeThru; // layers that cant see thru when sighting player

	[HideInInspector]
	public Vector2 lookDir; // direction enemy is looking - used because transform does not rotate when turning
	[HideInInspector]
	public SpawnController spawner;
	[HideInInspector]
	public GridUnit currentGrid;

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.red;

		Vector3 ray = Quaternion.Euler (0f, -seeAngle, -seeAngle) * (Vector3)lookDir * seeDist;
		Gizmos.DrawRay(transform.position, ray);

		ray = Quaternion.Euler (0f, seeAngle, seeAngle) * (Vector3)lookDir * seeDist;
		Gizmos.DrawRay(transform.position, ray);
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.name.StartsWith ("GridUnit"))
			currentGrid = col.GetComponent<GridUnit> ();
	}
}
