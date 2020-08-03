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
    Vector3 flyDirection;

    public bool fired;
    public bool charging;
    public bool isGrounded;

    [HideInInspector] public float spinTime;
    public bool isHit;
    public BoxCollider hitCollider;

    public bool isPlayer2;

    public float valueToLerp = 40;


    private void Start()
    {
        //cinemachine.m_XAxis.m_MaxSpeed = 0;
    }

    private void Update()
    {

        if(!isHit)
        {
            LookAround();
            Charging();
            Fire();
            FixPlayerRotation();
            RotationInput();
        }

        if (isHit)
        {
            StartCoroutine(DamageSpin(spinTime));
        }

        rb.AddForce(0, -5, 0);

    }

    private IEnumerator DamageSpin(float duration)
    {
        transform.Rotate(0, 1000f * Time.deltaTime, 0);
        yield return new WaitForSeconds(duration);
        isHit = false;
    }

    private void RotationInput()
    {
        if (!isPlayer2)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.rotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.rotation = Quaternion.Euler(0f, -rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
            }
        }
        else if(isPlayer2)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.rotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation = Quaternion.Euler(0f, -rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
            }
        }
    }

    private void FixPlayerRotation()
    {
        if (!charging && !fired && chargeValue <= 0)
        {
            if (isGrounded)
            {
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

        if(isPlayer2)
        {
            if (Input.GetKey(KeyCode.M))
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

            if (Input.GetKeyUp(KeyCode.M))
            {
                releasedDirection = transform.forward;
                charging = false;
                fired = true;
            }
        }

        else if(!isPlayer2)
        {
            if (Input.GetKey(KeyCode.Space))
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

            if (Input.GetKeyUp(KeyCode.Space))
            {
                releasedDirection = transform.forward;
                charging = false;
                fired = true;
            }
        }
    }

    private void Fire()
    {
        if (fired)
        {
            if(valueToLerp <= 55)
            {
                valueToLerp += Time.deltaTime * 5;
            }
            cinemachine.m_Lens.FieldOfView = valueToLerp;
            hitCollider.enabled = true;
            currentDirection = transform.forward;

            chargeValue -= Time.deltaTime / chargeTime;

            releasedDirection = Vector3.MoveTowards(releasedDirection, currentDirection, Time.deltaTime * changeDirectionSpeed);

            if(isGrounded)
            {
                flyDirection = transform.forward;
            }

            if (chargeValue <= 0)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                if(!isGrounded)
                {
                    Vector3 direction = new Vector3(flyDirection.x, flyDirection.y -= Time.deltaTime * 1.5f, flyDirection.z);
                    rb.velocity = direction * vehicleMaxSpeed;
                }
                if (isGrounded)
                {
                    rb.velocity = releasedDirection * vehicleMaxSpeed;
                    Quaternion finalRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                    transform.localRotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * 5);
                }

            }

        }

        if(chargeValue <= 0)
        {

            if (valueToLerp >= 40)
            {
                valueToLerp -= Time.deltaTime * 5;
            }
            cinemachine.m_Lens.FieldOfView = valueToLerp;
            fired = false;
            hitCollider.enabled = false;
        }
    }

    private void LookAround()
    {
        //if(!fired && Input.GetMouseButton(1))
        //    cinemachine.m_XAxis.m_MaxSpeed = 300;
        //else if(Input.GetMouseButtonUp(1))
        //    cinemachine.m_XAxis.m_MaxSpeed = 0;
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
