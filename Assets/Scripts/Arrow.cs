using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody arrowRb;

    [SerializeField] private int attackDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

        // Damage castle if collides with castle
        if (collision.gameObject.CompareTag("Castle"))
        {
            gameManager.AdjustCastleHP(-attackDamage);
        }
    }
}
