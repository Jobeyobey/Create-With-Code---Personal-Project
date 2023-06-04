using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        // Destroy enemy if player attacks with Space when in contact
        if (other.gameObject.CompareTag("Enemy") && Input.GetKey(KeyCode.Space))
        {
            Destroy(other.gameObject);
        }
    }
}
