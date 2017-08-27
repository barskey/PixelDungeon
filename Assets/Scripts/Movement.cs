using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	private EnemyController enemyComp;
	private Animator anim;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		enemyComp = gameObject.GetComponent<EnemyController> ();
		anim = gameObject.GetComponent<Animator> ();
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
	}

	public void Move (Vector2 moveTo, float speed)
	{
		Vector2 moveDir = (moveTo - (Vector2)transform.position).normalized;

		float speedX = moveDir.x * speed;
		float speedY = moveDir.y * speed;

		anim.SetFloat ("SpeedX", speedX);
		anim.SetFloat ("SpeedY", speedY);
		anim.SetBool ("Walking", true);

		rb2d.velocity = new Vector2 (speedX, speedY);

		if (rb2d.velocity.magnitude > 0) // only sets lookDir when moving, so it retains previous lookDir when still.
			enemyComp.lookDir = moveDir;
	}

	public void Stop ()
	{
		rb2d.velocity = Vector2.zero;
	}

}
