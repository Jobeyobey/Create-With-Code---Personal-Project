using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;

    // Movement variables
    private Footsteps footsteps;
    private float moveSpeed = 6f;
    private float xBound = 15f;
    private float zBound = 4f;
    private Vector3 oldPosition;
    private Vector3 newPosition;
    private Vector3 lookDir;
    private int damping = 50;

    // Other variables
    public GameObject sword;
    public AudioSource deathSource;
    public AudioClip[] deathSounds;
    private int randomSound;
    private Rigidbody playerRb;
    private Animator playerAnim;
    public int playerHP = 5;
    public float attackCooldown = 1.0f;
    public float attackCountdown = 0;
    public float attackSpeed = 0.3f;
    private bool isAlive = true;

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

        // Reset movement to 0
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

        // Save new position for rotation
        newPosition = transform.position;

        // If moving, face direction of travel
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            footsteps.WalkSound();
            lookDir = (newPosition - oldPosition).normalized;
            var rotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            playerAnim.SetBool("isRunning", true);
        }
        else
        {
            playerAnim.SetBool("isRunning", false);
        }
    }

    private void CheckAttack()
    {
        // Attack
        if (Input.GetKey(KeyCode.Space) && (Time.time - attackCountdown) > attackCooldown)
        {
            // Run attack animation
            playerAnim.SetTrigger("doAttack");

            // Reset attack cooldown
            attackCountdown = Time.time;

            // Run sword hit routine (Delays damage by 0.3f to line up with animation)
            StartCoroutine(SwordDelay());
        }
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
            randomSound = Random.Range(0, deathSounds.Length);
            deathSource.clip = deathSounds[randomSound];
            deathSource.Play();

            playerAnim.SetBool("isDead", true);
            gameManager.GameOver("Player Dead");
            isAlive = false;
        }
    }

    private IEnumerator SwordDelay()
    {
        yield return new WaitForSeconds(attackSpeed);
        sword.GetComponent<Sword>().DamageTarget();
    }

    public void TakeDamage()
    {
        // Only take damage if player isn't blocking
        if (!playerAnim.GetBool("isBlocking"))
        {
            playerHP -= 1;

            if (playerHP < 0)
            {
                playerHP = 0;
            }

            if (playerHP == 0)
            {
                PlayerDeath();
            }
        }
    }

    public void HealPlayer()
    {
        playerHP += 1;
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
