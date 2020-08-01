using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public CinemachineFreeLook cinemachine;

    public float chargeValue;
    public float maxChargeValue;
    public float rotationSpeed = 10.0f;
    public float changeDirectionSpeed;

    Vector3 releasedDirection;
    Vector3 currentDirection;

    public bool fired;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cinemachine.m_XAxis.m_MaxSpeed = 0;
    }

    private void Update()
    {
        LookAround();
        Charging();
        Fire();
    }

    private void Charging()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if(!fired)
            chargeValue += Time.deltaTime * 25;

            if (chargeValue >= maxChargeValue)
            {
                fired = true;
                releasedDirection = transform.forward;
                rb.velocity = transform.forward * chargeValue;
            }

        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            releasedDirection = transform.forward;
            fired = true;
        }
    }

    private void Fire()
    {
        if (fired)
        {
            currentDirection = transform.forward;

            chargeValue -= Time.deltaTime * 15;

            if (Input.GetKey(KeyCode.D))
            {
                transform.rotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.rotation = Quaternion.Euler(0f, -rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
            }

            releasedDirection = Vector3.MoveTowards(releasedDirection, currentDirection, Time.deltaTime * changeDirectionSpeed);

            rb.velocity = releasedDirection* chargeValue;
        }

        if(chargeValue <= 0)
        {
            fired = false;
        }
    }

    private void LookAround()
    {
        if(!fired && Input.GetMouseButton(1))
            cinemachine.m_XAxis.m_MaxSpeed = 300;
        else if(Input.GetMouseButtonUp(1))
            cinemachine.m_XAxis.m_MaxSpeed = 0;
    }


}
