using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHunter : MonoBehaviour
{
    private GameObject player;
    private Vector3 hunterPos;
    private Vector3 playerPos;
    private Vector3 moveDir;
    private bool atPlayer = false;

    public float speed = 5.0f;
    public int damping = 50;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards player
        Vector3 hunterPos = transform.position;
        Vector3 playerPos = player.transform.position;
        Vector3 moveDir = playerPos - hunterPos;
        transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

        // Look towards player
        var rotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Stop when collide with player
        if (collision.gameObject.CompareTag("Player"))
        {
            atPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Follow player if collisions stops
        if (collision.gameObject.CompareTag("Player"))
        {
            atPlayer = false;
        }
    }
}
