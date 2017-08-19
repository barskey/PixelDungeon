using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

	public GameObject enemy;
	public float spawnDelay = 3f;
	public int maxSpawn = 3;
	public float health = 100f;

	void Awake ()
	{
		InvokeRepeating ("SpawnEnemy", 1f, spawnDelay);
	}

	void SpawnEnemy ()
	{
		if (GetEnemyCount () < maxSpawn) {
			Debug.Log ("Spawning enemy...");
			GameObject.Instantiate (enemy, gameObject.transform.position, Quaternion.identity);
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
