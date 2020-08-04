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

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Vehicle")
        {
            lastVehicleInContact = other.gameObject;

        }
        

    }

}
