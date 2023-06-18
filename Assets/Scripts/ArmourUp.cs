using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourUp : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;

    [SerializeField] private int repairStrength = 5;
    [SerializeField] private int healStrength = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Heal castle and player
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            gameManager.AdjustCastleHP(repairStrength);
            playerController.HealPlayer(healStrength);
        }
    }
}
