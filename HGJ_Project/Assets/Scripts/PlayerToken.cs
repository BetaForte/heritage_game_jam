using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToken : MonoBehaviour
{
    // Start is called before the first frame update

    public float maxHeight;
    public float minHeight;
    public float floatSpeed;
    public float rotateSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.y > maxHeight && floatSpeed > 0)
        {
            floatSpeed *= -1;
        }
        else if(transform.localPosition.y < minHeight && floatSpeed < 0)
        {
            floatSpeed *= -1;
        }

        transform.localPosition += new Vector3(0, floatSpeed * Time.deltaTime, 0);
        transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));

    }
}
