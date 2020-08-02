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

    public List<Ramps.RampAngle> previousRamp = new List<Ramps.RampAngle>();

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
        if(Input.GetKey(KeyCode.Space))
        {
            if(!fired)
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ramp")
        {
            Ramps ramp = other.GetComponent<Ramps>();
            switch(ramp.rampAngle)
            {
                case Ramps.RampAngle.Left:
                    Quaternion leftAngle = Quaternion.Euler(-28.14f, 0, 0);
                    transform.rotation = Quaternion.Slerp(transform.rotation, leftAngle, 10);
                    break;
                case Ramps.RampAngle.Right:
                    Quaternion rightAngle = Quaternion.Euler(28.14f, -180, 0);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rightAngle, 10);
                    break;
                case Ramps.RampAngle.Up:
                    if(previousRamp.Count == 0)
                    {
                        Quaternion upAngle = Quaternion.Euler(-28.14f, 90f, 0);
                        transform.rotation = Quaternion.Slerp(transform.rotation, upAngle, 10);
                        previousRamp.Add(Ramps.RampAngle.Up);
                    }

                    else if (previousRamp[previousRamp.Count - 1] != Ramps.RampAngle.Down)
                    {
                        Quaternion upAngle = Quaternion.Euler(-28.14f, 90f, 0);
                        transform.rotation = Quaternion.Slerp(transform.rotation, upAngle, 10);
                        previousRamp.Add(Ramps.RampAngle.Up);
                    }
                    break;
                case Ramps.RampAngle.Down:
                    if(previousRamp.Count == 0)
                    {
                        Quaternion downAngle = Quaternion.Euler(-28.14f, -90, 0);
                        transform.rotation = Quaternion.Slerp(transform.rotation, downAngle, 10);
                        previousRamp.Add(Ramps.RampAngle.Down);
                        return;
                    }
                    else if(previousRamp[previousRamp.Count - 1] != Ramps.RampAngle.Up)
                    {
                        Quaternion downAngle = Quaternion.Euler(-28.14f, -90, 0);
                        transform.rotation = Quaternion.Slerp(transform.rotation, downAngle, 10);
                        previousRamp.Add(Ramps.RampAngle.Down);
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ramp")
        {
            Ramps ramp = other.GetComponent<Ramps>();
            switch (ramp.rampAngle)
            {
                case Ramps.RampAngle.Left:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), 10);
                    break;
                case Ramps.RampAngle.Right:
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), 10);
                    break;
                case Ramps.RampAngle.Up:
                    if(previousRamp.Count == 0)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90f, 0), 10);
                    }
                    else if (previousRamp[previousRamp.Count - 1] != Ramps.RampAngle.Down || previousRamp.Count == 0)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90f, 0), 10);
                    }
                    break;
                case Ramps.RampAngle.Down:
                    if (previousRamp.Count == 0)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), 10);

                    }
                    else if(previousRamp[previousRamp.Count - 1] != Ramps.RampAngle.Up || previousRamp.Count == 0)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), 10);
                    }
                    break;
            }
        }

        if(other.tag == "Clear")
        {
            previousRamp.Clear();
        }
    }


}
