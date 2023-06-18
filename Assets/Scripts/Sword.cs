using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject target;
    [SerializeField] private AudioSource hitEnemy;

    private void Update()
    {
        
    }

    // Track GameObject infront of unit
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject;
        }
    }

    // If GameObject leaves area infront of unit, set target to null
    private void OnTriggerExit(Collider other)
    {
        target = null;
    }


    public void DamageTarget()
    {
        // If a GameObject is infront of target, damage it. Reset target to null incase GameObject is destroyed and OnTriggerExit doesn't run
        if (target != null)
        {
            if (target.CompareTag("Enemy"))
            {
                target.GetComponent<EnemyHP>().TakeDamage();
                hitEnemy.Play();
                target = null;
            }
            else if (target.CompareTag("Player"))
            {
                target.GetComponent<PlayerController>().TakeDamage();
                hitEnemy.Play();
                target = null;
            }
        }
    }
}
