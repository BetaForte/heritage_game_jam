using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public enum Characters
    {
        None,
        Player1,
        Player2,
        AI1
    }


    public GameObject lastVehicleInContact;
    public int score;

    public float delay;

    private void Update()
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
        }
    }

    public void AddScore()
    {
        if(delay <= 0)
        {
            Debug.Log(gameObject.name + " score added!");
            score++;
            delay = 0.5f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            lastVehicleInContact = other.gameObject;

        }
        

    }

}
