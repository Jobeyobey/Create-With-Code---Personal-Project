using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : MonoBehaviour
{
    private GameManager gameManager;
    private Animator enemyAnim;
    private Vector3 castlePosition;
    [SerializeField] private GameObject arrowPrefab;

    // Status
    private bool isAlive = true;
    private bool isFiring = false;

    // Movement
    [SerializeField] private float attackPosition = -4;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damping = 2;

    // Audio
    [SerializeField] private AudioSource bowSound;
    [SerializeField] private Footsteps footsteps;
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
            // If not in position to fire
            if (transform.position.x < attackPosition)
            {
                // Move towards castle
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                enemyAnim.SetBool("isRunning", true);
                footsteps.WalkSound();

                // Keep unit on floor
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

                // If archer was firing, cancel (because not in position)
                if (isFiring)
                {
                    CancelInvoke();
                    isFiring = false;
                }
            }
            else // If in position to fire
            {
                enemyAnim.SetBool("isRunning", false);

                // Begin firing
                if (!isFiring)
                {
                    InvokeRepeating("Attack", 1, 3);
                    isFiring = true;
                }
            }

            // Look towards castle
            var rotation = Quaternion.LookRotation(Vector3.right);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }

        // If castle is destroyed
        else if (gameManager.gameStatus == "Castle Destroyed" && isAlive)
        {
            // Cancel firing, start running
            CancelInvoke();
            enemyAnim.SetBool("isRunning", true);
            footsteps.WalkSound();

            // Move towards castle centre
            var moveDir = new Vector3(castlePosition.x + 2, 0.5f, castlePosition.z) - transform.position;
            transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

            // Look in direction of movement
            var rotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }

        // Destroy unit upon entering castle
        if (transform.position.x > 18)
        {
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        // Play attack animation
        enemyAnim.SetTrigger("doAttack");

        // Spawn arrow with delay for animation
        StartCoroutine(FireArrow());
    }

    IEnumerator FireArrow()
    {
        // Delay arrow instantiation for animation
        yield return new WaitForSeconds(0.5f);

        Vector3 spawnPos = new Vector3(transform.position.x + 1, 1.5f, transform.position.z);
        Instantiate(arrowPrefab, spawnPos, arrowPrefab.transform.rotation);

        bowSound.Play();
    }

    public void Death()
    {
        // Play random death sound
        var randomSound = Random.Range(0, deathSounds.Length);
        deathSource.clip = deathSounds[randomSound];
        deathSource.Play();

        // Run death animation, stop arrow invocation and remove colliders to stop unit interacting with others
        isAlive = false;
        CancelInvoke();
        enemyAnim.SetBool("isDead", true);
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<BoxCollider>());
    }
}
