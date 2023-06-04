using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 15.0f;

    private Rigidbody arrowRb;

    // Start is called before the first frame update
    void Start()
    {
        arrowRb = GetComponent<Rigidbody>();

        // Fire arrow in direction of castle
        Vector3 arrowDir = new Vector3(4.25f, 1.5f, 0);
        arrowRb.AddForce(arrowDir, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy arrow if collides with non-enemy object
        if (!collision.gameObject.CompareTag("enemy"))
        {
            Destroy(gameObject);
        }
    }
}
