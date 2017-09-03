using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public float startingHealth = 100;                          // The amount of health the player starts the game with.
	public float currentHealth;                                 // The current health the player has.
	public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
//	public AudioClip deathClip;                                 // The audio clip to play when the player dies.
	public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
	public Color flashColor = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.


	Animator anim;                                              // Reference to the Animator component.
	Animator healthAnim;                                        // Reference to the Health Bar Animator
//	AudioSource playerAudio;                                    // Reference to the AudioSource component.
	PlayerController playerController;                          // Reference to the player controller.
//	PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
	bool isDead;                                                // Whether the player is dead.
	bool damaged;                                               // True when the player gets damaged.


	void Awake ()
	{
		// Setting up the references.
		anim = GetComponent <Animator> ();
		//healthAnim = GameObject.Find ("HealthBar").GetComponent <Animator> ();
//		playerAudio = GetComponent <AudioSource> ();
		playerController = GetComponent <PlayerController> ();
//		playerShooting = GetComponentInChildren <PlayerShooting> ();

		// Set the initial health of the player.
		currentHealth = startingHealth;

		// Set the health bar's value to the current health.
		//healthAnim.SetFloat ("Health", currentHealth);
	}


	void Update ()
	{
		// If the player has just been damaged...
		if (damaged && !isDead) {
			// ... set the colour of the damageImage to the flash colour.
			damageImage.color = flashColor;
		} else {
			// ... transition the colour back to clear.
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		// Reset the damaged flag.
		damaged = false;
	}


	public void TakeDamage (float amount)
	{
		// Set the damaged flag so the screen will flash.
		damaged = true;

		// Reduce the current health by the damage amount.
		currentHealth -= amount;

		// Set the health bar's value to the current health.
		//healthAnim.SetFloat ("Health", currentHealth);

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
		//anim.SetFloat ("LastMoveX", 0);

		// Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
		//playerAudio.clip = deathClip;
		//playerAudio.Play ();

		// Turn off the movement and shooting scripts.
		playerController.enabled = false;
		//playerShooting.enabled = false;
	}       
}