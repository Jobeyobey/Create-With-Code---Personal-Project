using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject target;
    public AudioSource hitEnemy;

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
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        target = null;
    }


    public void DamageTarget()
    {
        if (target.CompareTag("Enemy"))
        {
            target.GetComponent<EnemyHP>().TakeDamage();
            hitEnemy.Play();
        }
        else if (target.CompareTag("Player"))
        {
            target.GetComponent<PlayerController>().TakeDamage();
            hitEnemy.Play();
        }
    }
}
