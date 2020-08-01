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
                rb.velocity = transform.forward * chargeValue;
            }

        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            fired = true;
        }
    }

    private void Fire()
    {
        if (fired)
        {
            chargeValue -= Time.deltaTime * 15;
            if(Input.GetKey(KeyCode.D))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y + 45, transform.rotation.z), Time.deltaTime * rotationSpeed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y - 45, transform.rotation.z), Time.deltaTime * rotationSpeed);
            }

            rb.velocity = transform.forward * chargeValue;
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
