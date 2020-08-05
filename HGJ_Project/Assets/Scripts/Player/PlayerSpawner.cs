using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    public bool isPlayer;
    PlayerMovement playerMovement;
    AIController aiController;

    private void Start()
    {
        if(isPlayer)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }
        else
        {
            aiController = GetComponent<AIController>();
        }
    }

    private void Update()
    {
        if(isPlayer)
        {
            if (playerMovement.isDead)
            {
                int randomSpawnPoint = Random.Range(0, spawnPoints.Length - 1);
                transform.position = spawnPoints[randomSpawnPoint].position;
                playerMovement.isDead = false;
                playerMovement.Respawn();
            }
        }
        else
        {
            if(aiController.isDead)
            {
                int randomSpawnPoint = Random.Range(0, spawnPoints.Length - 1);
                transform.position = spawnPoints[randomSpawnPoint].position;
                aiController.isDead = false;
                aiController.Respawn();
            }
        }


    }

}
