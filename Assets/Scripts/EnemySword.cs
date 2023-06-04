using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    private GameObject target;
    private Rigidbody enemyRb;
    private Vector3 moveDir;

    public int damping = 50;
    public float speed = 1f;
    public bool atCastle = false;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Castle");
        enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!atCastle)
        {
            // Move towards castle
            moveDir = (target.transform.position - transform.position);
            transform.Translate(moveDir.normalized * Time.deltaTime * speed, Space.World);

            // Keep unit on floor
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }

        // Look towards castle
        var rotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detect arrival at castle
        if (collision.gameObject.CompareTag("Castle"))
        {
            atCastle = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Detect push away from castle
        if (collision.gameObject.CompareTag("Castle"))
        {
            atCastle = false;
        }
    }
}
