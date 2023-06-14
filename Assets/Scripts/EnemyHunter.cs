using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHunter : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;
    private PlayerController playerController;
    private Vector3 hunterPos;
    private Vector3 playerPos;
    private Vector3 moveDir;
    private Animator enemyAnim;
    private bool atPlayer = false;
    private float distanceToPlayer;

    public GameObject sword;
    private bool isAlive = true;
    public float speed = 5.0f;
    public int damping = 50;
    public float attackCooldown = 1.5f;
    public float attackCountdown = 0;
    public float attackSpeed = 0.5f;
    public bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        enemyAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking && gameManager.gameStatus != "Castle Destroyed" && isAlive)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > 1)
            {
                // Move towards player
                Vector3 hunterPos = transform.position;
                Vector3 playerPos = player.transform.position;
                Vector3 moveDir = playerPos - hunterPos;
                transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

                // Look towards player
                var rotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

                atPlayer = false;
                enemyAnim.SetBool("isRunning", true);
            }
            else
            {
                atPlayer = true;
                enemyAnim.SetBool("isRunning", false);
            }

            // Attacking
            if (atPlayer && (Time.time - attackCountdown) > attackCooldown)
            {
                attackCountdown = Time.time;
                Attack();
            }
        }

        // If castle is destroyed
        else if (gameManager.gameStatus == "Castle Destroyed" && isAlive)
        {
            enemyAnim.SetBool("isRunning", true);

            // Move into center of bridge
            if (transform.position.z < -2 || transform.position.z > 2)
            {
                moveDir = new Vector3(15, 0.5f, 0) - transform.position;
                transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

                var rotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
            else
            {
                moveDir = Vector3.right;
                transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

                var rotation = Quaternion.LookRotation(new Vector3(90, 0.001f, 0.001f));
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
        }

        if (transform.position.x > 18)
        {
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        isAttacking = true;
        enemyAnim.SetTrigger("doAttack");
        StartCoroutine(AttackDelay());
        StartCoroutine(Unfreeze());
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackSpeed);
        sword.GetComponent<Sword>().DamageTarget();
    }

    IEnumerator Unfreeze()
    {
        yield return new WaitForSeconds(attackSpeed + 0.3f);
        isAttacking = false;
    }

    public void Death()
    {
        isAlive = false;
        enemyAnim.SetBool("isDead", true);
    }
}
