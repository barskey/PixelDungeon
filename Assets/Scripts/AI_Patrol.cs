using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class AI_Patrol : MonoBehaviour {

	public enum States
	{
		Init,
		Patrol,
		Search,
		Rush,
		Attack
	}

	public float patrolSpeed = 0.5f; // speed while patroling
	[HideInInspector]
	public List<GameObject> patrolPoints = new List<GameObject> (); // empty list of all patrol points

	private EnemyController enemyComp;
	private Animator anim;
	private PlayerController player;
	private Movement moveComp;
	private Path pathComp;

	private StateMachine <States> sm;
	private float attackDist = 0.3f; // distance to keep while attacking
	private float speed; // current speed
	private int patrolIndex; // current index of next patrol point
	private Vector2 moveTo; // current point enemy is moving toward
	private float waitedInSecs = 0f; // time waited while searching
	private float SecSinceLastAttack = 0f; // seconds since last attack
	private int pathIndex; // current index of nav path

	// Use this for initialization
	void Awake ()
	{
		enemyComp = gameObject.GetComponent<EnemyController> ();
		pathComp = gameObject.GetComponent<Path> ();
		anim = gameObject.GetComponent<Animator> ();
		moveComp = gameObject.GetComponent<Movement> ();
		player = GameObject.Find ("Mage").GetComponent<PlayerController> (); // TODO change this to find players other than Mage

		// Inititalize State Machine Engine
		sm = StateMachine <States>.Initialize (this, States.Init);
	}

	private void Init_Update ()
	{
		patrolPoints = enemyComp.spawner.patrolPoints;

		if (patrolPoints.Count > 0) {
			patrolIndex = 0;
			sm.ChangeState (States.Patrol, StateTransition.Safe);
		}
	}

	private void Patrol_Enter ()
	{
		//Debug.Log ("Entered state Patrol.");

		// move to next patrol point
		if (enemyComp.currentGrid) {
			pathComp.FindNewPath (enemyComp.currentGrid, patrolPoints [patrolIndex].GetComponent<PatrolPoint> ().gridUnit);
			pathIndex = 0; 
			moveTo = pathComp.GetGridPos (pathComp.navPath[pathIndex]);
		}
		//Debug.Log (pathComp.GetGridUnit (moveTo));
	}

	private void Patrol_Update ()
	{
		//Debug.Log ("Update Patrol.");

		moveComp.Move (moveTo, patrolSpeed);

		float distToTarget = Vector2.Distance ((Vector2)transform.position, moveTo);

		if (SeesPlayer ()) {
			// go to prepare for attack state
			//Debug.Log ("Changing to Rush");
			sm.ChangeState (States.Rush, StateTransition.Safe);
		} else if (distToTarget < 0.02f) {
			if (pathIndex == pathComp.navPath.Count - 1) { // at the end of the navpath list
				patrolIndex++;
				if (patrolIndex >= patrolPoints.Count) // loop back at zero if end of patrol points
					patrolIndex = 0;
				Debug.Log ("Entering Search");
				sm.ChangeState (States.Search, StateTransition.Safe);
			} else { // increment index and get next point
				pathIndex++;
				moveTo = pathComp.GetGridPos (pathComp.navPath [pathIndex]);
			}
		}
	}

	private void Patrol_Exit ()
	{
		//Debug.Log ("Exit Patrol.");
	}


	private void Search_Enter ()
	{
		//Debug.Log ("Entered State Search");

		moveComp.Stop ();

		waitedInSecs = 0f; // reset to zero so we can count secs searching
	}

	private void Search_Update ()
	{
		//Debug.Log ("Update Search.");

		float search = patrolPoints [patrolIndex].GetComponent<PatrolPoint> ().searchInSecs / 4;

		if (waitedInSecs <= search * 1) { // wait 1/4 total wait time
			enemyComp.lookDir = Vector2.left; // look west
			//Debug.Log ("Looking west");
			anim.SetFloat ("LastMoveX", -1f);
			anim.SetFloat ("LastMoveY", 0f);
			anim.SetBool ("Walking", false);
		} else if (waitedInSecs <= search * 2) { // wait 1/4 total wait time
			enemyComp.lookDir = Vector2.up; // look north
			//Debug.Log ("Looking north");
			anim.SetFloat ("LastMoveX", 0f);
			anim.SetFloat ("LastMoveY", 1f);
			anim.SetBool ("Walking", false);
		} else if (waitedInSecs <= search * 3) { // wait 1/4 total wait time
			enemyComp.lookDir = Vector2.right; // look east
			//Debug.Log ("Looking east");
			anim.SetFloat ("LastMoveX", 1f);
			anim.SetFloat ("LastMoveY", 0f);
			anim.SetBool ("Walking", false);
		} else if (waitedInSecs <= search * 4) { // wait 1/4 total wait time
			enemyComp.lookDir = Vector2.down; // look south
			//Debug.Log ("Looking south");
			anim.SetFloat ("LastMoveX", 0f);
			anim.SetFloat ("LastMoveY", -1f);
			anim.SetBool ("Walking", false);
		} else if (waitedInSecs > search * 4) {
			//Debug.Log ("Changing to Search");
			sm.ChangeState (States.Patrol, StateTransition.Safe); // go back to patrol
		}

		if (SeesPlayer ()) {
			//Debug.Log ("Changing to Rush");
			sm.ChangeState (States.Rush, StateTransition.Safe);
		}

		waitedInSecs += Time.deltaTime;
	}

	private void Search_Exit ()
	{
		//Debug.Log ("Exit Search.");
	}

	private void Rush_Enter ()
	{
		//Debug.Log ("Entered state Rush.");

		//moveTo = (Vector2)player.transform.position; // move to player position
		pathComp.FindNewPath (enemyComp.currentGrid, player.playerGrid);
		pathIndex = 0;

		moveTo = pathComp.GetGridPos (pathComp.navPath[pathIndex]);
	}

	private void Rush_Update ()
	{
		//Debug.Log ("Update Rush.");

		moveComp.Move (moveTo, enemyComp.maxSpeed);

		// distance remaining to target
		float distToTarget = Vector2.Distance ((Vector2)transform.position, moveTo);
		//Debug.Log (distToTarget);

		// distance to player
		float distToPlayer = Vector2.Distance ((Vector2)transform.position, player.transform.position);

		if (distToPlayer < enemyComp.attackDist) {
			//Debug.Log ("Changing to Attack");
			sm.ChangeState (States.Attack, StateTransition.Safe);
		} else if (distToTarget < 0.03f) {
			if (pathIndex == pathComp.navPath.Count - 1) { // at the end of the navpath list
				//Debug.Log ("Entering Search");
				sm.ChangeState (States.Search, StateTransition.Safe);
			} else { // increment index and get next point
				pathIndex++;
				moveTo = pathComp.GetGridPos (pathComp.navPath[pathIndex]);
			}
		}

		// while enemy sees the player, update the moveto point
		if (SeesPlayer ()) {
			pathComp.FindNewPath (enemyComp.currentGrid, player.playerGrid);
			pathIndex = 0;
			moveTo = pathComp.GetGridPos (pathComp.navPath[pathIndex]);
		}
	}

	private void Rush_Exit ()
	{
		//Debug.Log ("Exit Rush.");
	}

	private void Attack_Enter ()
	{
		//Debug.Log ("Entered state Attack.");

		SecSinceLastAttack = 0f;

		moveComp.Stop ();
	}

	private void Attack_Update ()
	{
		if (SecSinceLastAttack >= enemyComp.secsBtwnAttack) {
			//Debug.Log ("Attack!");
			SecSinceLastAttack = 0f;
		}

		// distance remaining to target
		float dist = Vector2.Distance ((Vector2)transform.position, (Vector2)player.transform.position);

		if (dist >= attackDist) {
			sm.ChangeState (States.Rush, StateTransition.Safe);
		}

		SecSinceLastAttack += Time.deltaTime;
	}

	private void Attack_Exit ()
	{
		//Debug.Log ("Exit Attack.");
	}

	bool SeesPlayer()
	{
		// find the angle between player and lookDir to see if within cone of vision
		Vector2 playerDir = (Vector2)player.transform.position - (Vector2)transform.position;
		float playerAngle = Vector2.Angle (enemyComp.lookDir, playerDir);

		// find distance to player
		float playerDist = Vector3.Distance (player.transform.position, transform.position);

		// if player could be seen, check if there are obstacles (walls) between
		if (playerAngle <= enemyComp.seeAngle && playerDist <= enemyComp.seeDist) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, playerDir, playerDist, enemyComp.cantSeeThru);
			if (!hit) { // no walls in the way
				Debug.Log ("I see you!");
				return true;
			}
		}

		return false; // can't see player
	}
}
