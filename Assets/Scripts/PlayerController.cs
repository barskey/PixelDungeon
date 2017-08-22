using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;

	private Animator anim;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start ()
	{
		anim = gameObject.GetComponent<Animator> ();
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float inputX = Input.GetAxisRaw ("Horizontal");
		float inputY = Input.GetAxisRaw ("Vertical");

		anim.SetFloat ("SpeedX", inputX);
		anim.SetFloat ("SpeedY", inputY);

		rb2d.velocity = new Vector2 (inputX * speed, inputY * speed);
	}

	void FixedUpdate ()
	{
		float lastInputX = Input.GetAxisRaw ("Horizontal");
		float lastInputY = Input.GetAxisRaw ("Vertical");

		if (lastInputX != 0 || lastInputY != 0) {
			anim.SetBool ("Walking", true);

			if (lastInputX > 0) {
				anim.SetFloat ("LastMoveX", 1f);
			} else if (lastInputX < 0) {
				anim.SetFloat ("LastMoveX", -1f);
			} else {
				anim.SetFloat ("LastMoveX", 0);
			}

			if (lastInputY > 0) {
				anim.SetFloat ("LastMoveY", 1f);
			} else if (lastInputY < 0) {
				anim.SetFloat ("LastMoveY", -1f);
			} else {
				anim.SetFloat ("LastMoveY", 0);
			}
		} else {
			anim.SetBool ("Walking", false);
		}
	}
}
