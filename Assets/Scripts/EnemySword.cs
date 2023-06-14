using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject target;
    private Rigidbody enemyRb;
    private Animator enemyAnim;
    private Vector3 moveDir;

    private bool isAlive = true;
    public int damping = 50;
    public float speed = 1f;
    public bool atCastle = false;
    public float attackCooldown = 1.5f;
    public float attackCountdown = 0;
    public float attackSpeed = 0.3f;
    public int attackDamage = 2;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        target = GameObject.Find("Castle");
        enemyRb = GetComponent<Rigidbody>();
        enemyAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameStatus != "Castle Destroyed" && isAlive)
        {
            // If far from castle, move towards castle
            if (transform.position.x < 15)
            {
                // Move towards castle
                moveDir = Vector3.right;
                transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

                // Keep unit on floor
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

                // Set run
                enemyAnim.SetBool("isRunning", true);

                atCastle = false;
            }
            // Else if close to castle, stop moving
            else if (isAlive)
            {
                enemyAnim.SetBool("isRunning", false);
                atCastle = true;
            }

            // If at castle, attack
            if (atCastle && (Time.time - attackCountdown) > attackCooldown && isAlive)
            {
                attackCountdown = Time.time;
                Attack();
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
        enemyAnim.SetTrigger("doAttack");
        gameManager.AdjustCastleHP(-attackDamage);
    }

    public void Death()
    {
        isAlive = false;
        enemyAnim.SetBool("isDead", true);
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<BoxCollider>());
    }
}
