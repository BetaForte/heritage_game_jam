﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSmoke : MonoBehaviour
{
    public GameObject particle;
    AIController aiCon;
    PlayerMovement playerCon;
    // Start is called before the first frame update
    void Start()
    {
        aiCon = GetComponent<AIController>();
        playerCon = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(aiCon)
        {
            if (aiCon.fired)
                particle.SetActive(true);
            else
                particle.SetActive(false);
            //particle.SetActive(Vector3.SqrMagnitude(GetComponent<Rigidbody>().velocity) > 0 && aiCon.isGrounded);
        }
        else if(playerCon)
        {
            if (playerCon.fired)
                particle.SetActive(true);
            else
                particle.SetActive(false);
            //particle.SetActive(Vector3.SqrMagnitude(GetComponent<Rigidbody>().velocity) > 0 && playerCon.isGrounded);
        }
    }
}
