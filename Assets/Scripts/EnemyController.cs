using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // used for List.OrderBy name
using MonsterLove.StateMachine;

public class EnemyController : MonoBehaviour {

	public enum States
	{
		Patrol,
		Search,
		Rush,
		Attack
	}

	public float maxSpeed = 1f; // max speed, used when rushing for attack
	public float patrolSpeed = 0.5f; // speed while patroling
	public float SecsBtwnAttack = 1f; // seconds between attacks
	public float seeAngle = 30f; // angle in degrees for how wide enemy can see player
	public float seeDist = 0.3f; // how far can see player
	public LayerMask cantSeeThru; // layers that cant see thru when sighting player
	public float searchInSecs = 2f; // how long enemy searches for enemy before continuing

	private Animator anim;
	private Rigidbody2D rb2d;
	private GameObject player;


	private StateMachine <States> sm;
	private float attackDist = 0.3f; // distance to keep while attacking
	private float speed; // current speed
	private int patrolIndex = 0; // current index of next patrol point
	private List<GameObject> patrolPoints = new List<GameObject> (); // list of all patrol points
	private Vector2 lookDir; // direction enemy is looking - used because transform does not rotate when turning
	private Vector2 moveTo; // current point enemy is moving toward
	private float waitedInSecs = 0f; // time waited while searching
	float SecSinceLastAttack = 0f; // seconds since last attack

	// Use this for initialization
	void Awake ()
	{
		anim = gameObject.GetComponent<Animator> ();
		rb2d = gameObject.GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("Mage"); // TODO change this to find players other than Mage

		// find all patrol points tagged with "patrolpoint" and sort the list by name
		patrolPoints = GameObject.FindGameObjectsWithTag ("PatrolPoint").OrderBy (point=>point.name).ToList();

		// Inititalize State Machine Engine
		sm = StateMachine <States>.Initialize (this, States.Patrol);
	}

	private void Patrol_Enter ()
	{
		Debug.Log ("Entered state Patrol.");

		moveTo = patrolPoints [patrolIndex].transform.position; // move to next patrol point
		speed = patrolSpeed; // set patrol speed
	}

	private void Patrol_Update ()
	{
		Debug.Log ("Update Patrol.");

		Move ();

		if (SeesPlayer ()) {
			// go to prepare for attack state
			Debug.Log ("Changing to Rush");
			sm.ChangeState (States.Rush, StateTransition.Safe);
		} else if (Vector2.Distance ((Vector2) transform.position, moveTo) < 0.02f) { // if at (close to) current patrol point
			// increment patrol
			patrolIndex++;
			if (patrolIndex >= patrolPoints.Count())
				patrolIndex = 0;
			// and go to search state
			Debug.Log ("Changing to Search");
			sm.ChangeState (States.Search, StateTransition.Safe);
		}
	}

	private void Patrol_Exit ()
	{
		Debug.Log ("Exit Patrol.");
	}
		

	private void Search_Enter ()
	{
		Debug.Log ("Entered State Search");

		rb2d.velocity = Vector2.zero; // stop

		waitedInSecs = 0f; // reset to zero so we can count secs searching
	}

	private void Search_Update ()
	{
		Debug.Log ("Update Search.");

		float search = searchInSecs / 4;

		if (waitedInSecs <= search * 1) { // wait 1/4 total wait time
			lookDir = Vector2.left; // look west
			Debug.Log ("Looking west");
			anim.SetFloat ("LastMoveX", -1f);
			anim.SetFloat ("LastMoveY", 0f);
			anim.SetBool ("Walking", false);
		} else if (waitedInSecs <= search * 2) { // wait 1/4 total wait time
			lookDir = Vector2.up; // look north
			Debug.Log ("Looking north");
			anim.SetFloat ("LastMoveX", 0f);
			anim.SetFloat ("LastMoveY", 1f);
			anim.SetBool ("Walking", false);
		} else if (waitedInSecs <= search * 3) { // wait 1/4 total wait time
			lookDir = Vector2.right; // look east
			Debug.Log ("Looking east");
			anim.SetFloat ("LastMoveX", 1f);
			anim.SetFloat ("LastMoveY", 0f);
			anim.SetBool ("Walking", false);
		} else if (waitedInSecs <= search * 4) { // wait 1/4 total wait time
			lookDir = Vector2.down; // look south
			Debug.Log ("Looking south");
			anim.SetFloat ("LastMoveX", 0f);
			anim.SetFloat ("LastMoveY", -1f);
			anim.SetBool ("Walking", false);
		} else if (waitedInSecs > searchInSecs) {
			Debug.Log ("Changing to Search");
			sm.ChangeState (States.Patrol, StateTransition.Safe); // go back to patrol
		}

		if (SeesPlayer ()) {
			Debug.Log ("Changing to Rush");
			sm.ChangeState (States.Rush, StateTransition.Safe);
		}

		waitedInSecs += Time.deltaTime;
	}

	private void Search_Exit ()
	{
		Debug.Log ("Exit Search.");
	}

	private void Rush_Enter ()
	{
		Debug.Log ("Entered state Rush.");

		moveTo = (Vector2)player.transform.position; // move to player position
		speed = maxSpeed; // move at max speed
	}

	private void Rush_Update ()
	{
		Debug.Log ("Update Rush.");

		Move ();

		// distance remaining to target
		float dist = Vector2.Distance ((Vector2)transform.position, moveTo);

		if (dist < attackDist) {
			Debug.Log ("Changing to Attack");
			sm.ChangeState (States.Attack);
		}

		// while enemy sees the player, update the moveto point
		if (SeesPlayer ()) {
			moveTo = (Vector2)player.transform.position;
		} else { // otherwise search for player
			Debug.Log ("Changing to Search");
			sm.ChangeState (States.Search, StateTransition.Safe);
		}
	}

	private void Rush_Exit ()
	{
		Debug.Log ("Exit Rush.");
	}

	private void Attack_Enter ()
	{
		Debug.Log ("Entered state Attack.");

		SecSinceLastAttack = 0f;

		rb2d.velocity = Vector2.zero; // stop
	}

	private void Attack_Update ()
	{
		if (SecSinceLastAttack >= SecsBtwnAttack) {
			Debug.Log ("Attack!");
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
		Debug.Log ("Exit Attack.");
	}

	void Move ()
	{
		Vector2 moveDir = (moveTo - (Vector2)transform.position).normalized;

		float speedX = moveDir.x * speed;
		float speedY = moveDir.y * speed;

		anim.SetFloat ("SpeedX", speedX);
		anim.SetFloat ("SpeedY", speedY);
		anim.SetBool ("Walking", true);

		rb2d.velocity = new Vector2 (speedX, speedY);

		if (rb2d.velocity.magnitude > 0) // only sets lookDir when moving, so it retains previous lookDir when still.
			lookDir = moveDir;
	}

	bool SeesPlayer()
	{
		// find the angle between player and lookDir to see if within cone of vision
		Vector2 playerDir = (Vector2)player.transform.position - (Vector2)transform.position;
		float playerAngle = Vector2.Angle (lookDir, playerDir);

		// find distance to player
		float playerDist = Vector3.Distance (player.transform.position, transform.position);

		// if player could be seen, check if there are obstacles (walls) between
		if (playerAngle <= seeAngle && playerDist <= seeDist) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, playerDir, playerDist, cantSeeThru);
			if (!hit) { // no walls in the way
				Debug.Log ("I see you!");
				return true;
			}
		}

		return false; // can't see player
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.red;

		Vector3 ray = Quaternion.Euler (0f, -seeAngle, -seeAngle) * (Vector3)lookDir * seeDist;
		Gizmos.DrawRay(transform.position, ray);

		ray = Quaternion.Euler (0f, seeAngle, seeAngle) * (Vector3)lookDir * seeDist;
		Gizmos.DrawRay(transform.position, ray);
	}
}
