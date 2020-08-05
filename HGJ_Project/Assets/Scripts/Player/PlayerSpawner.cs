using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;


    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(playerMovement.isDead)
        {
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length - 1);
            transform.position = spawnPoints[randomSpawnPoint].position;
            playerMovement.isDead = false;
            playerMovement.Respawn();
        }


    }

}
