using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float moveSpeed;
	[HideInInspector]
	public Vector2 moveTo;

	void Awake ()
	{
		moveTo = new Vector2 (transform.position.x, transform.position.y);
	}

	void Update ()
	{
		if ((Vector2)transform.position == moveTo)
			Time.timeScale = 1;

		float step = moveSpeed * Time.unscaledDeltaTime;

		Vector3 newPos = new Vector3 (moveTo.x, moveTo.y, transform.position.z);
		transform.position = Vector3.MoveTowards (transform.position, newPos, step);
	}

}
