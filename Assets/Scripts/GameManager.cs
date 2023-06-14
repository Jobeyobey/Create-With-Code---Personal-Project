using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Core Variables
    public string gameStatus = "Menu";
    public bool isGameActive = false;
    public GameObject titleScreen;
    public Button startButton;

    // Castle
    public GameObject castleDoors;
    public int castleHP = 100;

    // Player
    private PlayerController playerController;

    // Prefabs
    public GameObject[] enemyBasic;
    public GameObject enemyHunter;
    public GameObject armourUp;

    // Enemy spawn and movement restrictions
    private float enemySpawnX = -25f;
    private float spawnY = 0.5f;
    private float boundZ = 4f;

    // Armour spawn restrictions
    private float armourBoundX = 14f;

    // Start is called before the first frame update
    public void StartGame()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.playerHP = 5;

        titleScreen.gameObject.SetActive(false);
        isGameActive = true;
        castleHP = 100;

        InvokeRepeating("SpawnBasicEnemy", 2, 3);
        InvokeRepeating("SpawnHunter", 15, 15);
        InvokeRepeating("SpawnArmour", 10, 20);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Prefab Spawning
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
        Vector3 randomSpawnPos = new Vector3(randomSpawnX, spawnY -0.2f, randomSpawnZ);
        Instantiate(armourUp, randomSpawnPos, armourUp.transform.rotation);
    }

    // Health Management
    public void AdjustCastleHP(int adjustment)
    {
        // Make health adjustment
        castleHP += adjustment;

        // Cap castle HP between 0 and 100
        if (castleHP < 0)
        {
            castleHP = 0;
        }
        else if (castleHP > 100)
        {
            castleHP = 100;
        }

        // If castleHP falls to 0, game over
        if (castleHP == 0)
        {
            castleDoors.gameObject.SetActive(false);
            GameOver("Castle Destroyed");
        }
    }

    public void GameOver(string status)
    {
        gameStatus = status;
        isGameActive = false;
        titleScreen.gameObject.SetActive(true);
        CancelInvoke();
    }
}
