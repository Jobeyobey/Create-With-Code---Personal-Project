using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int enemyHP;

    public void TakeDamage()
    {
        enemyHP -= 1;

        if (enemyHP == 0)
        {
            if (gameObject.name == "EnemySwordsman(Clone)")
            {
                gameObject.GetComponent<EnemySwordsman>().Death();
            }
            else if (gameObject.name == "EnemyArcher(Clone)")
            {
                gameObject.GetComponent<EnemyArcher>().Death();
            }
            else // Hunter
            {
                gameObject.GetComponent<EnemyHunter>().Death();
            }
        }
    }
}
