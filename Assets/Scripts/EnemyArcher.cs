using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : MonoBehaviour
{
    private GameObject target;
    private Rigidbody enemyRb;
    public GameObject arrowPrefab;

    public int damping = 50;
    public float speed = 1f;
    public bool inPosition = false;
    public bool isFiring = false;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Castle");
        enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards castle
        if (!inPosition)
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed);

            // Keep unit on floor
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

            // If archer was firing, cancel
            if (isFiring)
            {
                CancelInvoke();
                isFiring = false;
            }
        }

        // Check if in position to fire
        if (transform.position.x > -4)
        {
            // Begin firing
            if (!isFiring)
            {
                InvokeRepeating("FireArrow", 1, 3);
                isFiring = true;
            }

            inPosition = true;
        }

        // Look towards castle
        Vector3 lookDir = new Vector3(0, 90, 0);
        lookDir.y = 0;
        var rotation = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    void FireArrow()
    {
        // Arrow spawn position
        Vector3 spawnPos = new Vector3(transform.position.x + 1, 0.5f, transform.position.z);

        Instantiate(arrowPrefab, spawnPos, arrowPrefab.transform.rotation);
    }
}
