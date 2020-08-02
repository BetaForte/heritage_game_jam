using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public CinemachineFreeLook cinemachine;
    public Rigidbody rb;

    public float chargeValue;
    public float chargeTime;
    public float vehicleMaxSpeed;
    public float rotationSpeed = 10.0f;
    public float changeDirectionSpeed;

    Vector3 releasedDirection;
    Vector3 currentDirection;

    public bool fired;
    public bool charging;
    public bool isGrounded;


    private void Start()
    {
        cinemachine.m_XAxis.m_MaxSpeed = 0;
    }

    private void Update()
    {
        LookAround();
        Charging();
        Fire();
        FixPlayerRotation();

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

    [PunRPC]
    private void FixPlayerRotation()
    {

        if (!charging && !fired && chargeValue <= 0)
        {
            if (isGrounded)
            {
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeRotationX;
                rb.constraints = RigidbodyConstraints.FreezeRotationY;
                rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            }

            Quaternion finalRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            transform.localRotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * 5);
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotationY;
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void Charging()
    {
        if (!isGrounded) return;
        if(Input.GetKey(KeyCode.Space))
        {
            if (!fired)
            {
                chargeValue += Time.deltaTime;
                charging = true;
            }

            if (chargeValue >= chargeTime)
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

            chargeValue -= Time.deltaTime / chargeTime;

            if(isGrounded)
            {
                releasedDirection = Vector3.MoveTowards(releasedDirection, currentDirection, Time.deltaTime * changeDirectionSpeed);
            }

            if (chargeValue <= 0)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                if(!isGrounded)
                {
                    Vector3 direction = new Vector3(releasedDirection.x, releasedDirection.y -= Time.deltaTime * 1.5f, releasedDirection.z);
                    rb.velocity = direction * vehicleMaxSpeed;
                }
                rb.velocity = releasedDirection * vehicleMaxSpeed;

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
