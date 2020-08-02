using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public CinemachineFreeLook cinemachine;

    public float chargeValue;
    public float chargeTime;
    public float vehicleMaxSpeed;
    public float rotationSpeed = 10.0f;
    public float changeDirectionSpeed;

    public Vector3 releasedDirection;
    public Vector3 currentDirection;

    public bool fired;
    public bool charging;
    public bool isGrounded;

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


        transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);


        if (!charging && !fired && chargeValue <= 0 && isGrounded)
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
            rb.constraints = RigidbodyConstraints.FreezeRotationY;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotationY;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        }

        rb.AddForce(0, -5, 0);

        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0f, -rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
        }
    }

    private void Charging()
    {
        if (!isGrounded) return;
        if(Input.GetKey(KeyCode.Space))
        {
            if (!fired)
            {
                chargeValue += Time.deltaTime * 25;
                charging = true;
            }

            if (chargeValue >= vehicleMaxSpeed)
            {
                charging = false;
                fired = true;
                releasedDirection = transform.forward;
                rb.velocity = transform.forward * chargeValue;
            }

        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            releasedDirection = transform.forward;
            charging = false;
            fired = true;
        }
    }

    private void Fire()
    {
        if (fired)
        {
            currentDirection = transform.forward;

            chargeValue -= Time.deltaTime * chargeTime;

            releasedDirection = Vector3.MoveTowards(releasedDirection, currentDirection, Time.deltaTime * changeDirectionSpeed);

            if (chargeValue <= 0)
                rb.velocity = Vector3.zero;
            else
            {
                rb.velocity = releasedDirection * vehicleMaxSpeed;
                //rb.AddForce(releasedDirection * vehicleMaxSpeed / 100, ForceMode.VelocityChange);

            }

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

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Ground")
        {
            isGrounded = false;
        }
    }


}
