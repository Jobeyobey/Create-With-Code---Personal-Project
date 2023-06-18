using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody playerRb;
    private Animator playerAnim;

    // Player Status
    public int playerHealth;
    private bool isAlive = true;

    // Movement
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private int damping = 50;
    private float xBound = 15f;
    private float zBound = 4f;
    private Vector3 oldPosition;
    private Vector3 newPosition;
    private Vector3 lookDir;

    // Attacking
    [SerializeField] private GameObject sword;
    [SerializeField] private float attackCooldown = 1.0f;
    private float attackCountdown = 0;
    private float attackSpeed = 0.3f;

    // Audio
    [SerializeField] private Footsteps footsteps;
    [SerializeField] private AudioSource deathSource;
    [SerializeField] private AudioClip[] deathSounds;
    private int randomDeathSound;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        footsteps = GetComponent<Footsteps>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            // Only allow player to move or attack if they are not blocking
            CheckBlock();
            if (!playerAnim.GetBool("isBlocking"))
            {
                CheckAttack();
                MovePlayer();
            }
        } 
        else if (gameManager.gameStatus == "Castle Destroyed")
        {
            playerAnim.SetBool("isRunning", false);
        }
        ConstrainPlayerPosition();
    }

    // Move player according to player input
    void MovePlayer()
    {
        // Save current position for rotation later
        oldPosition = transform.position;

        // Reset movement values to 0
        float moveX = 0f;
        float moveZ = 0f;

        // Adjust movement values according to WASD input
        if (Input.GetKey(KeyCode.W))
        {
            moveZ += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveZ += -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX += -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX += 1f;
        }

        // Move player using normalized movements values
        Vector3 moveDir = new Vector3(moveX, 0.0000001f, moveZ).normalized;
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        // Save new position for rotation later
        newPosition = transform.position;

        // If moving, face direction of travel
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            lookDir = (newPosition - oldPosition).normalized;

            if (lookDir != Vector3.zero)
            {
                var rotation = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }

            // Play running animation and sound
            playerAnim.SetBool("isRunning", true);
            footsteps.WalkSound();
        }
        else
        {
            playerAnim.SetBool("isRunning", false);
        }
    }

    private void CheckAttack()
    {
        if (Input.GetKey(KeyCode.Space) && (Time.time - attackCountdown) > attackCooldown)
        {
            // Play attack animation
            playerAnim.SetTrigger("doAttack");

            // Reset attack cooldown
            attackCountdown = Time.time;

            // Run sword hit routine (Delays damage to line up with animation)
            StartCoroutine(SwordDelay());
        }
    }

    private IEnumerator SwordDelay()
    {
        // Delay sword hit to time with animation
        yield return new WaitForSeconds(attackSpeed);
        sword.GetComponent<Sword>().DamageTarget();
    }

    private void CheckBlock()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerAnim.SetBool("isBlocking", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            playerAnim.SetBool("isBlocking", false);
        }
    }

    private void PlayerDeath()
    {
        if (isAlive)
        {
            // Play a random death sound
            randomDeathSound = Random.Range(0, deathSounds.Length);
            deathSource.clip = deathSounds[randomDeathSound];
            deathSource.Play();

            // Play death animation
            playerAnim.SetBool("isDead", true);

            // Game over
            gameManager.GameOver("Player Dead");
            isAlive = false;
        }
    }

    public void TakeDamage()
    {
        // Only take damage if player isn't blocking
        if (!playerAnim.GetBool("isBlocking"))
        {
            playerHealth -= 1;

            // Do not let health drop below 0
            if (playerHealth < 0)
            {
                playerHealth = 0;
            }

            // Kill player if health is 0
            if (playerHealth == 0)
            {
                PlayerDeath();
            }
        }
    }

    public void HealPlayer(int healAmount)
    {
        playerHealth += healAmount;
    }

    // Keep player in bounds
    void ConstrainPlayerPosition()
    {
        // X boundary
        if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }

        // Z boundary
        if (transform.position.z < -zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }
        else if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }
    }
}
