using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public Sprite openSprite;
	public Sprite closedSprite;
	public bool isOpen = false;

	// Use this for initialization
	void Awake () {
		// set initial state of the door
		if (isOpen)
			Open ();
		else
			Close ();
	}

	public void TryOpen ()
	{
		if (isOpen) {
			Close ();
		} else {
			Open ();
		}
	}

	private void Open ()
	{
		gameObject.GetComponent <SpriteRenderer> ().sprite = openSprite;
		gameObject.GetComponent <BoxCollider2D> ().enabled = false;
		isOpen = true;
	}

	private void Close ()
	{
		gameObject.GetComponent <SpriteRenderer> ().sprite = closedSprite;
		gameObject.GetComponent <BoxCollider2D> ().enabled = true;
		isOpen = false;
	}
}
