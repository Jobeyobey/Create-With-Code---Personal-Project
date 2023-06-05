using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyBasic;
    public GameObject enemyHunter;
    public GameObject armourUp;

    private float enemySpawnX = -25f;
    private float spawnY = 0.5f;
    private float boundZ = 4f;

    private float armourBoundX = 14f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBasicEnemy", 2, 5);
        InvokeRepeating("SpawnHunter", 15, 15);
        InvokeRepeating("SpawnArmour", 10, 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnBasicEnemy()
    {
        // Pick Swordsman(0) or Archer(1) and spawn at random Z location
        int enemyIndex = Random.Range(0, 2);
        float randomSpawnZ = Random.Range(-boundZ, boundZ);
        Vector3 randomSpawnPos = new Vector3(enemySpawnX, spawnY, randomSpawnZ);
        Instantiate(enemyBasic[enemyIndex], randomSpawnPos, enemyBasic[enemyIndex].transform.rotation);
    }

    void SpawnHunter()
    {
        // Spawn hunter at random Z location
        float randomSpawnZ = Random.Range(-boundZ, boundZ);
        Vector3 randomSpawnPos = new Vector3(enemySpawnX, spawnY, randomSpawnZ);
        Instantiate(enemyHunter, randomSpawnPos, enemyHunter.transform.rotation);
    }

    void SpawnArmour()
    {
        float randomSpawnZ = Random.Range(-boundZ, boundZ);
        float randomSpawnX = Random.Range(-armourBoundX, armourBoundX);
        Vector3 randomSpawnPos = new Vector3(randomSpawnX, spawnY, randomSpawnZ);
        Instantiate(armourUp, randomSpawnPos, armourUp.transform.rotation);
    }
}
