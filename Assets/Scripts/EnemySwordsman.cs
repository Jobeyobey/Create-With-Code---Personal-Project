using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordsman : MonoBehaviour
{
    private GameManager gameManager;
    private Animator enemyAnim;
    private Vector3 castlePosition;

    // Status
    private bool isAlive = true;
    private bool atCastle = false;

    // Movement
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damping = 2;
    private Vector3 moveDir;

    // Attack
    [SerializeField] private float attackPosition = 15;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int attackDamage = 2;
    private float attackCountdown = 0;

    // Audio
    private Footsteps footsteps;
    [SerializeField] private AudioSource stoneSound;
    [SerializeField] private AudioSource deathSource;
    [SerializeField] private AudioClip[] deathSounds;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        footsteps = GetComponentInChildren<Footsteps>();
        enemyAnim = GetComponentInChildren<Animator>();
        castlePosition = GameObject.Find("Castle").GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStatus != "Castle Destroyed" && isAlive)
        {
            // If far from castle, move towards castle
            if (transform.position.x < attackPosition)
            {
                atCastle = false;

                // Move towards castle
                moveDir = Vector3.right;
                transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

                // Keep unit on floor
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

                // Set run animation
                enemyAnim.SetBool("isRunning", true);
                footsteps.WalkSound();
            }
            // Else if close to castle, stop moving
            else if (isAlive)
            {
                atCastle = true;
                enemyAnim.SetBool("isRunning", false);
            }

            // If at castle, attack
            if (atCastle && (Time.time - attackCountdown) > attackCooldown && isAlive)
            {
                Attack();

                // Track time of attack for attack cooldown
                attackCountdown = Time.time;
            }

            // Always look towards castle
            var rotation = Quaternion.LookRotation(new Vector3(90, 0.001f, 0.001f));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }

        // If castle is destroyed
        else if (gameManager.gameStatus == "Castle Destroyed" && isAlive)
        {
            atCastle = false;
            enemyAnim.SetBool("isRunning", true);
            footsteps.WalkSound();

            // If not in line with doors, move closer to center of bridge
            if (transform.position.z < -2 || transform.position.z > 2)
            {
                moveDir = new Vector3(15, 0.5f, 0) - transform.position;
                transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

                var rotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
            else // If inline with doors, run into castle
            {
                moveDir = Vector3.right;
                transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

                var rotation = Quaternion.LookRotation(new Vector3(90, 0.001f, 0.001f));
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
        }

        // Destroy unit upon entering castle
        if (transform.position.x > castlePosition.x + 2)
        {
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        enemyAnim.SetTrigger("doAttack");
        gameManager.AdjustCastleHP(-attackDamage);
        stoneSound.Play();
    }

    public void Death()
    {
        // Play a random death sound
        var randomSound = Random.Range(0, deathSounds.Length);
        deathSource.clip = deathSounds[randomSound];
        deathSource.Play();

        // Run death animation and remove colliders to stop unit interacting with others
        isAlive = false;
        enemyAnim.SetBool("isDead", true);
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<BoxCollider>());
    }
}
