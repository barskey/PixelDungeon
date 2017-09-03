using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	public float startingHealth = 100;                          // The amount of health the enemy starts the game with.
	public float currentHealth;                                 // The current health the enemy has.
	//public AudioClip deathClip;                                 // The audio clip to play when the player dies.

	Animator anim;                                              // Reference to the Animator component.
	//AudioSource playerAudio;                                    // Reference to the AudioSource component.
	EnemyController enemyController;                          // Reference to the player controller.
	//PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
	bool isDead;                                                // Whether the player is dead.


	void Awake ()
	{
		// Setting up the references.
		anim = GetComponent <Animator> ();
		//playerAudio = GetComponent <AudioSource> ();
		//enemyController = GetComponent <EnemyController> ();
		//playerShooting = GetComponentInChildren <PlayerShooting> ();

		// Set the initial health of the player.
		currentHealth = startingHealth;

		// Set the health bar's value to the current health.
		//healthAnim.SetFloat ("Health", currentHealth);
	}

	public void TakeDamage (float amount)
	{
		// Reduce the current health by the damage amount.
		currentHealth -= amount;

		// Set the health bar's value to the current health.
		//healthAnim.SetFloat ("Health", currentHealth);

		// Set the animator to display the current damage
		anim.SetFloat ("Health", currentHealth);

		// Play the hurt sound effect.
		//playerAudio.Play ();

		// If the player has lost all it's health and the death flag hasn't been set yet...
		if(currentHealth <= 0 && !isDead) {
			// ... it should die.
			Death ();
		}
	}


	void Death ()
	{
		// Set the death flag so this function won't be called again.
		isDead = true;

		// Turn off any remaining shooting effects.
		//playerShooting.DisableEffects ();

		// Tell the animator that the player is dead.
		//anim.SetFloat ("Speed", 0);

		// Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
		//playerAudio.clip = deathClip;
		//playerAudio.Play ();

		GameObject.Destroy (gameObject);
	}       
}