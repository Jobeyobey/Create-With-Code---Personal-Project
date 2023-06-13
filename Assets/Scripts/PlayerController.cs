using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    private float moveSpeed = 7f;
    private float xBound = 15f;
    private float zBound = 4f;
    private Vector3 oldPosition;
    private Vector3 newPosition;
    private Vector3 lookDir;
    private int damping = 50;

    // Other variables
    public GameObject sword;
    private Rigidbody playerRb;
    private Animator playerAnim;
    public float attackCooldown = 1.0f;
    public float attackCountdown = 0;
    public float attackSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
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
        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        // Save new position for rotation
        newPosition = transform.position;

        // If moving, face direction of travel
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            lookDir = (newPosition - oldPosition).normalized;
            var rotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            playerAnim.SetBool("isRunning", true);
        }
        else
        {
            playerAnim.SetBool("isRunning", false);
        }
        
        // Attack
        if (Input.GetKey(KeyCode.Space) && (Time.time - attackCountdown) > 1.0f)
        {
            playerAnim.SetTrigger("doAttack");
            attackCountdown = Time.time;
            StartCoroutine(SwordDelay());
        }
    }

    private IEnumerator SwordDelay()
    {
        yield return new WaitForSeconds(attackSpeed);
        sword.GetComponent<Sword>().DestroyTarget();
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
