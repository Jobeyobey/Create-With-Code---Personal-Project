using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    private GameObject target;
    private Rigidbody enemyRb;
    private Animator enemyAnim;
    private Vector3 moveDir;

    public int damping = 50;
    public float speed = 1f;
    public bool atCastle = false;
    public float attackCooldown = 1.5f;
    public float attackCountdown = 0;
    public float attackSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Castle");
        enemyRb = GetComponent<Rigidbody>();
        enemyAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < 15)
        {
            // Move towards castle
            moveDir = (Vector3.right);
            transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

            // Keep unit on floor
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

            // Set run
            enemyAnim.SetBool("isRunning", true);

            atCastle = false;
        }
        else
        {
            enemyAnim.SetBool("isRunning", false);
            atCastle = true;
        }

        if (atCastle && (Time.time - attackCountdown) > attackCooldown)
        {
            attackCountdown = Time.time;
            Attack();
        }

        // Look towards castle
        var rotation = Quaternion.LookRotation(new Vector3(90, 0, 0));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    void Attack()
    {
        enemyAnim.SetTrigger("doAttack");
    }
}
