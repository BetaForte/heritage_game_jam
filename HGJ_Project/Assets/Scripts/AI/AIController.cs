using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    //time to move in max speed (in seconds)
    public float chargeTime;
    //max speed of the vehicle
    public float vehicleMaxSpeed;
    public float rotationSpeed = 10.0f;
    public float changeDirectionSpeed;

    public BoxCollider[] bcArray;

    [HideInInspector]
    public Vector3 releasedDirection;
    Vector3 currentDirection;

    public bool isDead;
    public bool fired;
    public bool isGrounded;
    public bool isHit;
    public bool rotate;
    [HideInInspector] public float spinTime;

    public BoxCollider hitCollider;

    Rigidbody rb;
    float angleToRotate;

    Score scoreScript;
    ArenaGameManager2Player arenaGM;
    ArenaGameManager1Player arenaGM1;

    private void Awake()
    {
        arenaGM = FindObjectOfType<ArenaGameManager2Player>();
        if (arenaGM == null)
        {
            arenaGM1 = FindObjectOfType<ArenaGameManager1Player>();
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        scoreScript = GetComponent<Score>();
    }

    private void Update()
    {
        if (arenaGM == null)
        {
            if (!arenaGM1.hasRoundStart || arenaGM1.isRoundOver) return;
        }
        if (arenaGM != null)
        {
            if (!arenaGM.hasRoundStart || arenaGM.isRoundOver) return;
        }
        MovementUpdate();
        FixPlayerRotation();
        LogicUpdate();

    }
    public void Charge(float m_chargeValue)
    {
        if (!isGrounded)
        {
            fired = false;
            return;
        }

        chargeTime = m_chargeValue;

        fired = true;
    }

    public void Steer(float angle)
    {
        float newAngle = Mathf.MoveTowards(transform.eulerAngles.y, angle, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, newAngle, 0);
        if (newAngle >= angle)
            rotate = false;
    }



    private void LogicUpdate()
    {
        if (isHit)
        {
            StartCoroutine(DamageSpin(spinTime));
        }

        hitCollider.enabled = fired;
    }

    private IEnumerator DamageSpin(float duration)
    {
        transform.Rotate(0, 1000f * Time.deltaTime, 0);
        yield return new WaitForSeconds(duration);
        isHit = false;
    }

    private void MovementUpdate()
    {
        if (fired)
        {
            chargeTime -= Time.deltaTime;

            currentDirection = transform.forward;

            releasedDirection = Vector3.MoveTowards(releasedDirection, currentDirection, Time.deltaTime * changeDirectionSpeed);

            rb.velocity = releasedDirection * vehicleMaxSpeed;

            if (chargeTime <= 0)
                fired = false;
        }
    }

    private void FixPlayerRotation()
    {
        rb.AddForce(0, -5, 0);

        if (!fired)
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

    private void Die()
    {
        for (int i = 0; i < bcArray.Length; i++)
        {
            bcArray[i].enabled = false;
        }
        GetComponent<MeshRenderer>().enabled = false;
        isDead = true;
    }

    public void Respawn()
    {
        for (int i = 0; i < bcArray.Length; i++)
        {
            bcArray[i].enabled = true;
        }
        GetComponent<MeshRenderer>().enabled = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Deadzone")
        {
            if (scoreScript.lastVehicleInContact != null)
            {
                GameObject killer = scoreScript.lastVehicleInContact;
                Score killerScore = killer.GetComponent<Score>();

                killerScore.lastVehicleInContact = null;
                killerScore.score++;
            }

            Die();

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
