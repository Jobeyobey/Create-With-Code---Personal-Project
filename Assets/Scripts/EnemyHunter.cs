using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHunter : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;
    private Animator enemyAnim;
    private Vector3 castlePosition;
    [SerializeField] private GameObject sword;

    // Status
    private bool isAlive = true;

    // Movement
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private int damping = 50;
    private Vector3 hunterPos;
    private Vector3 playerPos;
    private Vector3 moveDir;
    private float distanceToPlayer;

    // Attack
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackCooldown = 1.5f;
    private float attackCountdown = 0;
    private float attackSpeed = 0.5f;
    private bool isAttacking = false;

    // Audio
    private Footsteps footsteps;
    [SerializeField] private AudioSource deathSource;
    [SerializeField] private AudioClip[] deathSounds;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        enemyAnim = GetComponentInChildren<Animator>();
        footsteps = GetComponentInChildren<Footsteps>();
        castlePosition = GameObject.Find("Castle").GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking && gameManager.gameStatus != "Castle Destroyed" && isAlive)
        {
            hunterPos = transform.position;
            playerPos = player.transform.position;
            moveDir = playerPos - hunterPos;

            distanceToPlayer = Vector3.Distance(hunterPos, playerPos);

            // Check if outside of attacking distance of player
            if (distanceToPlayer > attackDistance)
            {
                enemyAnim.SetBool("isRunning", true);
                footsteps.WalkSound();

                // Move towards player
                transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

                // Look towards player
                var rotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
            else
            {
                enemyAnim.SetBool("isRunning", false);

                // Look towards player
                var rotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }

            // Attacking
            if (distanceToPlayer < attackDistance && (Time.time - attackCountdown) > attackCooldown)
            {
                Attack();

                // Track time of attack for attack cooldown
                attackCountdown = Time.time;
            }
        }

        // If castle is destroyed, run into castle
        else if (gameManager.gameStatus == "Castle Destroyed" && isAlive)
        {
            enemyAnim.SetBool("isRunning", true);
            footsteps.WalkSound();

            // Move into center of bridge if not in line with doors
            if (transform.position.z < -2 || transform.position.z > 2)
            {
                moveDir = new Vector3(15, 0.5f, 0) - transform.position;
                transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

                var rotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
            else // Run into castle if in line with doors
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
        // Set isAttacking to true, so unit will not move or rotate while attacking
        isAttacking = true;

        // Play attack animation
        enemyAnim.SetTrigger("doAttack");

        // Run attack routine with delay to match with animation. Unfreeze unit by setting isAttacking to false
        StartCoroutine(AttackDelay());
        StartCoroutine(Unfreeze());
    }

    IEnumerator AttackDelay()
    {
        // Delay attack for animation
        yield return new WaitForSeconds(attackSpeed);
        sword.GetComponent<Sword>().DamageTarget();
    }

    IEnumerator Unfreeze()
    {
        // Delay unfreezing of unit movement for animation
        yield return new WaitForSeconds(attackSpeed + 0.3f);
        isAttacking = false;
    }

    public void Death()
    {
        // Play random death sound
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
