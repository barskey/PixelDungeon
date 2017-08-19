using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // used for List.OrderBy name

public class EnemyController : MonoBehaviour {

	public float maxSpeed = 1f;
	public float patrolSpeed = 0.5f;

	private Animator anim;
	private Rigidbody2D rb2d;
	private int patrolIndex = 0;
	private List<GameObject> patrolPoints = new List<GameObject> ();

	// Use this for initialization
	void Start ()
	{
		anim = gameObject.GetComponent<Animator> ();
		rb2d = gameObject.GetComponent<Rigidbody2D> ();

		// find all patrol points (tagged with "patrolpoint" and sort the list by name
		patrolPoints = GameObject.FindGameObjectsWithTag ("PatrolPoint").OrderBy (point=>point.name).ToList();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Patrol ();

		// if close to current patrol point, incrememnt index
		if (Vector3.Distance (transform.position, patrolPoints [patrolIndex].transform.position) < 0.02f) {
			patrolIndex++;
			if (patrolIndex >= patrolPoints.Count())
				patrolIndex = 0;
		}
	}

	void Patrol ()
	{
		Vector3 moveDir = patrolPoints [patrolIndex].transform.position - transform.position;
		moveDir.Normalize ();

		float speedX = moveDir.x * patrolSpeed;
		float speedY = moveDir.y * patrolSpeed;

		anim.SetFloat ("SpeedX", speedX);
		anim.SetFloat ("SpeedY", speedY);

		rb2d.velocity = new Vector2 (speedX, speedY);
	}
}
