using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSettings : MonoBehaviour
{
    public static ArenaSettings instance;

    public List<VehicleType> vehicleTypes = new List<VehicleType>();

    public string player1Vehicle;
    public string player2Vehicle;
    public string player3Vehicle;
    public string player4Vehicle;


    public float roundTime;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


}

[System.Serializable]
public class VehicleType
{
    public string vehicleTypeName;
    public Material vehicleMaterial;
    public float chargeTime;
    public float vehicleMaxSpeed;
    public float knockbackStrength;

}
