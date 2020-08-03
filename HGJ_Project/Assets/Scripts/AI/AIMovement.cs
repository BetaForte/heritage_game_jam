using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    //time to move in max speed (in seconds)
    public float chargeTime;
    //max speed of the vehicle
    public float vehicleMaxSpeed;
    public float rotationSpeed = 10.0f;
    public float changeDirectionSpeed;

    Vector3 releasedDirection;
    Vector3 currentDirection;

    public bool fired;
    public bool isGrounded;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MovementUpdate();
        FixPlayerRotation();

        if (Input.GetKeyDown(KeyCode.L))
        {
            Charge(1f);
        }
    }

    private void MovementUpdate()
    {
        if (fired)
        {
            chargeTime -= Time.deltaTime;
            releasedDirection = transform.forward;
            rb.velocity = releasedDirection * vehicleMaxSpeed;

            if (chargeTime <= 0)
                fired = false;
        }
    }

    private void Charge(float m_chargeValue)
    {
        if (!isGrounded)
        {
            fired = false;
            return;
        }

        chargeTime = m_chargeValue;

        fired = true;
    }

    private void Steer()
    {

    }


    private void FixPlayerRotation()
    {
        rb.AddForce(0, -5, 0);

        if (!fired)
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


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            isGrounded = false;
        }
    }

}
