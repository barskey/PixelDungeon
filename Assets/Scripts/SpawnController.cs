using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

	public GameObject enemy;
	public float spawnDelay = 3f;
	public int maxSpawn = 3;
	public float health = 100f;

	void Awake()
	{
		InvokeRepeating ("SpawnEnemy", 1f, spawnDelay);
	}

	void SpawnEnemy()
	{
		if (GameObject.FindGameObjectsWithTag ("Enemy").Length < maxSpawn) {
			Debug.Log ("Spawning enemy...");
			GameObject.Instantiate (enemy, gameObject.transform.position, Quaternion.identity);
		}
	}
}
