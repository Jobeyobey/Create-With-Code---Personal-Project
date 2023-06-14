using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject target;
    private Rigidbody enemyRb;
    private Animator enemyAnim;
    public GameObject arrowPrefab;

    private bool isAlive = true;
    public int damping = 50;
    public float speed = 1f;
    public bool inPosition = false;
    public bool isFiring = false;

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
            // Move towards castle
            if (!inPosition)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);

                // Keep unit on floor
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

                // If archer was firing, cancel
                if (isFiring)
                {
                    CancelInvoke();
                    isFiring = false;
                }

                enemyAnim.SetBool("isRunning", true);
            }

            // Check if in position to fire
            if (transform.position.x > -4)
            {
                // Begin firing
                if (!isFiring)
                {
                    InvokeRepeating("Attack", 1, 3);
                    isFiring = true;
                }

                inPosition = true;
                enemyAnim.SetBool("isRunning", false);
            }

            // Look towards castle
            var rotation = Quaternion.LookRotation(Vector3.right);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }

        // If castle is destroyed
        else if (gameManager.gameStatus == "Castle Destroyed" && isAlive)
        {
            CancelInvoke();
            enemyAnim.SetBool("isRunning", true);

            var moveDir = new Vector3(20, 0.5f, 0) - transform.position;
            transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

            var rotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }

        if (transform.position.x > 18)
        {
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        // Attack animation
        enemyAnim.SetTrigger("doAttack");

        // Arrow spawn
        StartCoroutine(FireArrow());
    }

    IEnumerator FireArrow()
    {
        yield return new WaitForSeconds(0.5f);

        // Arrow spawn position
        Vector3 spawnPos = new Vector3(transform.position.x + 1, 1.5f, transform.position.z);

        Instantiate(arrowPrefab, spawnPos, arrowPrefab.transform.rotation);
    }

    public void Death()
    {
        isAlive = false;
        enemyAnim.SetBool("isDead", true);
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<BoxCollider>());
    }
}
