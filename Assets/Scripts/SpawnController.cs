using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

	public GameObject enemy; // Which enemy is spawned
	public float spawnDelay = 3f; // Seconds between spawning new enemies
	public int maxSpawn = 3; // max # of enemies allowed on screen
	public float health = 100f;
	public List<GameObject> patrolPoints = new List<GameObject> ();

	private Animator anim;

	void Awake ()
	{
		anim = gameObject.GetComponent<Animator> ();

		InvokeRepeating ("SpawnEnemy", spawnDelay, spawnDelay);
	}

	void TakeDamage (float dmg)
	{
		health -= dmg;

		anim.SetFloat ("Health", health);

		// if health < 0, destroy itself
	}

	void SpawnEnemy ()
	{
		if (GetEnemyCount () < maxSpawn) {
			Debug.Log ("Spawning enemy...");
			// set enemy list of patrol points so it keeps them after this is destroyed
			GameObject newEnemy = GameObject.Instantiate (enemy, gameObject.transform.position, Quaternion.identity);
			newEnemy.GetComponent<EnemyController> ().spawner = this;
		}
	}

	int GetEnemyCount ()
	{
		int i = 0;

		var enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject e in enemies)
		{
			if (e.name.StartsWith (enemy.name))
				i++;
		}

		return i;
	}
}
