using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    private float moveSpeed = 7f;
    private float xBound = 15f;
    private float zBound = 4f;

    // Other variables
    private Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
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
