using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{
    public Player player;
    public CinemachineFreeLook cinemachine;
    public Rigidbody rb;
    public PostProcessVolume postProcessing;
    public BoxCollider[] bcArray;

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
    public bool isDead;

    [HideInInspector] public float spinTime;
    public bool isHit;
    public BoxCollider hitCollider;

    public bool isPlayer2;
    public float valueToLerp = 40;

    public HitCollider hc;
    Score scoreScript;
    
    public ArenaGameManager2Player arenaGM;
    public ArenaGameManager1Player arenaGM1;

    WhiteboardKiller whiteboard;
    GenerateSmoke smoke;

    public GameObject uiInputs;

    bool isSoundPlayed = false;

    bool rightHeld = false;
    bool leftHeld = false;
    bool mobileCharge = false;
    bool isMobile = false;

    private void Awake()
    {
        smoke = GetComponent<GenerateSmoke>();
        scoreScript = GetComponent<Score>();
        arenaGM = FindObjectOfType<ArenaGameManager2Player>();
        if(arenaGM == null)
        {
            arenaGM1 = FindObjectOfType<ArenaGameManager1Player>();
        }

        whiteboard = FindObjectOfType<WhiteboardKiller>();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        uiInputs.SetActive(false);
        isMobile = false;
        postProcessing.enabled = false;
#elif UNITY_ANDROID || UNITY_IOS
        uiInputs.SetActive(true);
        isMobile = true;
#endif

    }

    private void Start()
    {
        for (int i = 0; i < ArenaSettings.instance.vehicleTypes.Count; i++)
        {
            switch (player)
            {
                case Player.Player1:
                    if (ArenaSettings.instance.vehicleTypes[i].vehicleTypeName == ArenaSettings.instance.player1Vehicle)
                    {
                        chargeTime = ArenaSettings.instance.vehicleTypes[i].chargeTime;
                        vehicleMaxSpeed = ArenaSettings.instance.vehicleTypes[i].vehicleMaxSpeed;
                        hc.knockbackStrength = ArenaSettings.instance.vehicleTypes[i].knockbackStrength;
                        GetComponent<MeshRenderer>().material = ArenaSettings.instance.vehicleTypes[i].vehicleMaterial;
                        GetComponent<MeshFilter>().mesh = ArenaSettings.instance.vehicleTypes[i].meshRender;
                    }
                    break;

                case Player.Player2:
                    if (ArenaSettings.instance.vehicleTypes[i].vehicleTypeName == ArenaSettings.instance.player2Vehicle)
                    {
                        chargeTime = ArenaSettings.instance.vehicleTypes[i].chargeTime;
                        vehicleMaxSpeed = ArenaSettings.instance.vehicleTypes[i].vehicleMaxSpeed;
                        hc.knockbackStrength = ArenaSettings.instance.vehicleTypes[i].knockbackStrength;
                        GetComponent<MeshRenderer>().material = ArenaSettings.instance.vehicleTypes[i].vehicleMaterial;
                        GetComponent<MeshFilter>().mesh = ArenaSettings.instance.vehicleTypes[i].meshRender;
                    }
                    break;
            }
        }
    }

    private void Update()
    {
        MobileControls();

        if(arenaGM == null)
        {
            if (!arenaGM1.hasRoundStart || arenaGM1.isRoundOver) return;
        }
        if(arenaGM != null)
        {
            if (!arenaGM.hasRoundStart || arenaGM.isRoundOver) return;
        }

        if (transform.position.y > 230)
        {
            isGrounded = false;
        }

        if(!isHit)
        {
            Charging();
            LookAround();
            Fire();
            FixPlayerRotation();
            RotationInput();
        }

        if (isHit)
        {
            smoke.PlayHit();
            StartCoroutine(DamageSpin(spinTime));
        }

        rb.AddForce(0, -5, 0);

    }

    private IEnumerator DamageSpin(float duration)
    {
        int playRandomHitFX = Random.Range(5, 11);
        if(!isSoundPlayed)
        {
            SoundManager.instance.PlaySFX(playRandomHitFX);
            isSoundPlayed = true;
        }


        transform.Rotate(0, 1000f * Time.deltaTime, 0);
        yield return new WaitForSeconds(duration);
        isHit = false;
        SoundManager.instance.StopSFX(playRandomHitFX);
        isSoundPlayed = false;
    }

    private void RotationInput()
    {
#if UNITY_EDITOR ||UNITY_STANDALONE_WIN
        if (!isPlayer2)
        {
            if (Input.GetKey(KeyCode.D))
            {
                TurnRight();
            }

            if (Input.GetKey(KeyCode.A))
            {
                TurnLeft();
            }
        }
        else if(isPlayer2)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                TurnRight();
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                TurnLeft();
            }
        }



#endif
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
                    SoundManager.instance.StopSFX(2);
                    SoundManager.instance.PlaySFX(4);
                    charging = false;
                    fired = true;
                    releasedDirection = transform.forward;
                    rb.velocity = transform.forward * chargeValue;
                }

            }


            if (Input.GetKeyDown(KeyCode.M))
                SoundManager.instance.PlaySFX(2);

            if (Input.GetKeyUp(KeyCode.M))
            {
                SoundManager.instance.StopSFX(2);
                SoundManager.instance.PlaySFX(4);
                releasedDirection = transform.forward;
                charging = false;
                fired = true;
            }
        }

        else if(!isPlayer2)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                OnChargeKeyHeld();
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                OnChargeKeyDown();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                OnChargeKeyUp();
            }
        }
    }

    private void Fire()
    {
        if (fired)
        {
            if(isPlayer2)
                SoundManager.instance.StopSFX(2);
            else
                SoundManager.instance.StopSFX(1);

            if(!isMobile)
            {
                AddFiringEffects();
            }

            hitCollider.enabled = true;
            currentDirection = transform.forward;


            releasedDirection = Vector3.MoveTowards(releasedDirection, currentDirection, Time.deltaTime * changeDirectionSpeed);

            chargeValue -= Time.deltaTime / chargeTime;

            if (isGrounded)
            {
                flyDirection = transform.forward;
            }

            if (chargeValue <= 0 && isGrounded)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                if (!isGrounded)
                {
                    Vector3 direction = new Vector3(flyDirection.x, flyDirection.y -= Time.deltaTime * 1.5f, flyDirection.z);
                    rb.velocity = direction * vehicleMaxSpeed;
                }
                if (isGrounded)
                {
                    rb.velocity = new Vector3(releasedDirection.x * vehicleMaxSpeed, releasedDirection.y*vehicleMaxSpeed, releasedDirection.z * vehicleMaxSpeed);
                }

            }

        }

        if (chargeValue <= 0)
        {
            RemoveFiringEffects();
            fired = false;
            hitCollider.enabled = false;
        }
    }

    private void AddFiringEffects()
    {
        if (postProcessing == null) return;
        if (cinemachine.m_Lens.FieldOfView <= 45)
            cinemachine.m_Lens.FieldOfView += Time.deltaTime * 2f;

        LensDistortion lensDistortion;
        postProcessing.profile.TryGetSettings(out lensDistortion);
        lensDistortion.intensity.value = 50f;

        ChromaticAberration chromaticAberration;
        postProcessing.profile.TryGetSettings(out chromaticAberration);
        chromaticAberration.intensity.value = 1f;

    }

    private void RemoveFiringEffects()
    {
        if (cinemachine.m_Lens.FieldOfView >= 40)
            cinemachine.m_Lens.FieldOfView -= Time.deltaTime * 5;

        LensDistortion lensDistortion;
        postProcessing.profile.TryGetSettings(out lensDistortion);
        lensDistortion.intensity.value = 20f;

        ChromaticAberration chromaticAberration;
        postProcessing.profile.TryGetSettings(out chromaticAberration);
        chromaticAberration.intensity.value = 0f;
    }

    private void LookAround()
    {
        //if(!fired && Input.GetMouseButton(1))
        //    cinemachine.m_XAxis.m_MaxSpeed = 300;
        //else if(Input.GetMouseButtonUp(1))
        //    cinemachine.m_XAxis.m_MaxSpeed = 0;
    }

    private void Die()
    {
        for(int i = 0; i < bcArray.Length; i++)
        {
            bcArray[i].enabled = false;
        }
        GetComponent<MeshRenderer>().enabled = false;
        cinemachine.enabled = false;
        isDead = true;

        if(scoreScript.lastVehicleInContact != null)
        {
            if (scoreScript.lastVehicleInContact.name == "Player1")
            {
                SoundManager.instance.PlaySFX(12);
            }


            if (scoreScript.lastVehicleInContact.name == "Player2")
            {
                SoundManager.instance.PlaySFX(13);
            }
        }
    }

    public void Respawn()
    {
        for (int i = 0; i < bcArray.Length; i++)
        {
            bcArray[i].enabled = true;
        }
        GetComponent<MeshRenderer>().enabled = true;
        cinemachine.enabled = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        chargeValue = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (arenaGM == null)
        {
            if (arenaGM1.isRoundOver) return;
        }
        if (arenaGM != null)
        {
            if (arenaGM.isRoundOver) return;
        }

        if (other.tag == "Deadzone")
        {
            SoundManager.instance.PlaySFX(15);
            if(scoreScript.lastVehicleInContact != null)
            {
                GameObject killer = scoreScript.lastVehicleInContact;
                Score killerScore = killer.GetComponent<Score>();

                if (killerScore.lastVehicleInContact != null && killerScore != null)
                {
                    whiteboard.Killer(killerScore, killerScore.lastVehicleInContact);
                    killerScore.AddScore();
                }

                killerScore.lastVehicleInContact = null;
            }

            Die();

        }
    }

    public void TurnRight()
    {
        transform.rotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
    }

    public void TurnLeft()
    {
        transform.rotation = Quaternion.Euler(0f, -rotationSpeed * Time.deltaTime, 0f) * transform.rotation;
    }

    public void OnChargeKeyUp()
    {
        if (!fired)
        {
            SoundManager.instance.StopSFX(1);
            SoundManager.instance.PlaySFX(3);
        }
        releasedDirection = transform.forward;
        charging = false;
        fired = true;
    }

    public void OnChargeKeyDown()
    {
        SoundManager.instance.PlaySFX(1);
    }

    public void OnChargeKeyHeld()
    {
        if (!fired)
        {
            chargeValue += Time.deltaTime;
            charging = true;
        }

        if (chargeValue >= chargeTime)
        {
            SoundManager.instance.StopSFX(1);
            SoundManager.instance.PlaySFX(3);
            charging = false;
            fired = true;
            releasedDirection = transform.forward;
            rb.velocity = transform.forward * chargeValue;
        }
    }

    private void MobileControls()
    {
        if (rightHeld)
        {
            TurnRight();
        }
        if (leftHeld)
        {
            TurnLeft();
        }

        if(mobileCharge)
        {
            OnChargeKeyHeld();
        }
    }

    public void RightButtonClicked()
    {
        rightHeld = true;
    }

    public void RightButtonReleased()
    {
        rightHeld = false;
    }

    public void LeftButtonClicked()
    {
        leftHeld = true;
    }

    public void LeftButtonReleased()
    {
        leftHeld = false;
    }

    public void ChargeButtonClicked()
    {
        mobileCharge = true;
    }

    public void ChargeButtonReleased()
    {
        mobileCharge = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Ground")
        {
            isGrounded = true;
        }

        if (other.tag == "Player")
        {
            isGrounded = true;
        }

        if (other.tag != "Ground")
        {
            isGrounded = false;
        }

        if (other.tag == "Deadzone")
        {
            if (scoreScript.lastVehicleInContact != null)
            {
                GameObject killer = scoreScript.lastVehicleInContact;
                Score killerScore = killer.GetComponent<Score>();

                if (killerScore.lastVehicleInContact != null && killerScore != null)
                {
                    whiteboard.Killer(killerScore, killerScore.lastVehicleInContact);
                    killerScore.AddScore();
                }

                killerScore.lastVehicleInContact = null;
            }

            Die();

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
