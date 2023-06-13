using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHunter : MonoBehaviour
{
    private GameObject player;
    private Vector3 hunterPos;
    private Vector3 playerPos;
    private Vector3 moveDir;
    private Animator enemyAnim;
    private bool atPlayer = false;
    private float distanceToPlayer;

    public float speed = 5.0f;
    public int damping = 50;
    public float attackCooldown = 1.5f;
    public float attackCountdown = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemyAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
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

    void Attack()
    {
        enemyAnim.SetTrigger("doAttack");
    }
}
