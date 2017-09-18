using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrigger : MonoBehaviour {

	public GameObject actionItem; // game object on which to perform action
	public enum ActionType
	{
		Nothing,
		Door
	};
	public ActionType actionType = ActionType.Nothing;

	private bool canDoAction = false;

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.E)) {
			bool actionResult = TryAction ();
		}
	}

	public bool TryAction ()
	{
		if (canDoAction) {
			switch (actionType) {
			case ActionType.Door:
				Door door = actionItem.GetComponent <Door> ();
				door.TryOpen ();
				return true;
				break;
			default:
				break;
			}
		}

		return false;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		PlayerController player = col.GetComponent <PlayerController> ();
		if (player) {
			canDoAction = true;
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		PlayerController player = col.GetComponent <PlayerController> ();
		if (player) {
			canDoAction = false;
		}
	}
}
