using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 0.5f;
	public GameObject projectile;
	public float projectileForce = 100f;
	[HideInInspector]
	public GridUnit playerGrid;

	private Animator anim;
	private Rigidbody2D rb2d;
	private Transform lookDir; // used to keep track of which direction player is aiming/moving. Y axis points toward last movement.

	// Use this for initialization
	void Awake ()
	{
		anim = gameObject.GetComponent <Animator> ();
		rb2d = gameObject.GetComponent <Rigidbody2D> ();
		lookDir = transform.GetChild (0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		float inputX = Input.GetAxisRaw ("Horizontal");
		float inputY = Input.GetAxisRaw ("Vertical");

		anim.SetFloat ("SpeedX", inputX);
		anim.SetFloat ("SpeedY", inputY);

		Vector2 v = new Vector2 (inputX * speed, inputY * speed);

		rb2d.velocity = Vector2.ClampMagnitude (v, speed);

		if (Input.GetKeyDown (KeyCode.Space)) {
			Attack ();
		}
	}

	void FixedUpdate ()
	{
		float lastInputX = Input.GetAxisRaw ("Horizontal");
		float lastInputY = Input.GetAxisRaw ("Vertical");

		if (lastInputX != 0 || lastInputY != 0) {
			if (lastInputX != 0 || lastInputY != 0) // only set aim if actually receiving input
				lookDir.localRotation = Quaternion.LookRotation (Vector3.forward, new Vector3 (lastInputX, lastInputY));

			anim.SetBool ("Walking", true);
			anim.SetFloat ("LastMoveX", lastInputX);
			anim.SetFloat ("LastMoveY", lastInputY);
		} else {
			anim.SetBool ("Walking", false);
		}
	}

	void Attack ()
	{
		GameObject prj = GameObject.Instantiate (projectile, lookDir.position, lookDir.rotation);
		prj.GetComponent <Rigidbody2D> ().AddForce (projectileForce * lookDir.transform.up.normalized);
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.name.StartsWith ("GridUnit"))
			playerGrid = col.GetComponent<GridUnit> ();
	}
}
