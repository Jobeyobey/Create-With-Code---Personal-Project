using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Game Status
    public string gameStatus = "Menu";
    public bool isGameActive = false;

    // Start and Game Over menu
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI gameOverTitle;
    [SerializeField] private Button restartButton;

    // Game Objects
    [SerializeField] private GameObject castleDoors;
    private PlayerController playerController;

    // Castle and Player health
    [SerializeField] private TextMeshProUGUI castleText;
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private int castleMaxHP = 100;
    [SerializeField] private int playerHP = 5;
    private int currentCastleHP;

    // Prefabs
    [SerializeField] private GameObject[] enemyBasic;
    [SerializeField] private GameObject enemyHunter;
    [SerializeField] private GameObject armourUp;

    // Enemy spawn and movement restrictions
    [SerializeField] private float basicEnemySpawnRate = 3;
    [SerializeField] private float hunterSpawnRate = 15;
    [SerializeField] private float armourSpawnRate = 20;
    private float enemySpawnX = -25f;
    private float spawnY = 0.5f;
    private float boundZ = 4f;
    private float armourBoundX = 14f;

    // Audio
    [SerializeField] private AudioSource gateSound;
    [SerializeField] private AudioSource startHorn;

    void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void StartGame()
    {
        // Set player and castle HP
        playerController.playerHealth = playerHP;
        currentCastleHP = castleMaxHP;

        // Start game
        titleScreen.gameObject.SetActive(false);
        isGameActive = true;
        startHorn.Play();

        // Set enemy spawn invokes
        InvokeRepeating("SpawnBasicEnemy", basicEnemySpawnRate, basicEnemySpawnRate);
        InvokeRepeating("SpawnHunter", hunterSpawnRate, hunterSpawnRate);
        InvokeRepeating("SpawnArmour", armourSpawnRate, armourSpawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        // User UI
        castleText.text = "Castle: " + currentCastleHP;
        playerText.text = "Player: " + playerController.playerHealth;
    }

    // Prefab Spawning
    void SpawnBasicEnemy()
    {
        // Pick Swordsman(0) or Archer(1) and spawn at random Z location
        int enemyIndex = Random.Range(0, enemyBasic.Length);
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
        currentCastleHP += adjustment;

        // Cap castle HP between 0 and 100
        if (currentCastleHP < 0)
        {
            currentCastleHP = 0;
        }
        else if (currentCastleHP > castleMaxHP)
        {
            currentCastleHP = castleMaxHP;
        }

        // If castleHP falls to 0, game over
        if (currentCastleHP == 0)
        {
            gateSound.Play();
            castleDoors.gameObject.SetActive(false);
            GameOver("Castle Destroyed");
        }
    }

    public void GameOver(string status)
    {
        gameStatus = status;
        isGameActive = false;
        CancelInvoke();

        gameOverScreen.gameObject.SetActive(true);

        if (gameStatus == "Castle Destroyed")
        {
            gameOverTitle.text = "The castle has fallen";
        }
        else
        {
            gameOverTitle.text = "You have fallen";
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
