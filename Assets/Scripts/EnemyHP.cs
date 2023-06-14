using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int enemyHP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage()
    {
        enemyHP -= 1;

        if (enemyHP == 0)
        {
            Debug.Log("First Set");
            if (gameObject.name == "EnemySwordsman(Clone)")
            {
                Debug.Log("Second Set");
                gameObject.GetComponent<EnemySword>().Death();
            }
            else if (gameObject.name == "EnemyArcher(Clone)")
            {
                gameObject.GetComponent<EnemyArcher>().Death();
            }
            else
            {
                gameObject.GetComponent<EnemyHunter>().Death();
            }
        }
    }
}
