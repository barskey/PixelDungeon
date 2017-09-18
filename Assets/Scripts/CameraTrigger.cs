using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {

	public GameObject cam; // the camera this script will control
	public enum TriggerType
	{
		ChangeRooms,
		FollowX,
		FollowY
	}
	public TriggerType triggerType;
	public GameObject cameraPos;

	private CameraController camController;

	void Awake ()
	{
		camController = cam.GetComponent <CameraController> ();
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		switch (triggerType)
		{
		case TriggerType.ChangeRooms:
			ChangeRooms ();
			break;
		case TriggerType.FollowX:
			FollowX ();
			break;
		case TriggerType.FollowY:
			FollowY ();
			break;
		}
	}

	void ChangeRooms ()
	{
		// tell camera new position to move to - only using x and y to preserve camera z position
		camController.moveTo = (Vector2)cameraPos.transform.position;

		Time.timeScale = 0; // stop time until camera move is complete
	}

	void FollowX ()
	{
		
	}

	void FollowY ()
	{
		
	}
}
