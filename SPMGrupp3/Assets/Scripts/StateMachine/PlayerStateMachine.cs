using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CameraType
{
    First, Third
};

public class PlayerStateMachine : PhysicsStateMachine
{
    [HideInInspector] public SphereCollider cameraCollider;
    [HideInInspector] public PlayerValues playerValues;
    public float maxSpeed;
    
    public float velocityToDash;
    public float toSuperDash = 30f;
    
    [HideInInspector] public float countdown;
    [HideInInspector] public float waitWhenInteracting;
    [HideInInspector] public float lastAcceleration;
    [HideInInspector] public float lastGravity;

    
    
    public bool IsDashing = false;
    [HideInInspector] public float elapsedDashTime;
    public bool allowedToDash = true;
    public float dashCooldown = 5f;
    public float dashDuration = 2f;
    public int DashLevel { get; set; }

    

    public float dashStateAcceleration;
    public float dashStateGravity;
    public float dashAirResistance;
    public float terminalVelocity;

    public GameObject meshParent;

    [HideInInspector] public Animator anim;
    public float animationSpeed;


    // KAMERA VARIABLER
    public float mouseSensitivity;
    float rotationX;
    float rotationY;
    public float minAngle;
    public float maxAngle;
    public Vector3 cameraPositionRelativeToPlayer;
    public CameraType cameraType;
    public bool IsWithinTriggerRange { get; set; }
    public string[] CameraIgnoreTags;
    private float originalFOV;
    public float maxFOV;
    private float cameraShakeIntensity = 0f;
    [SerializeField] private float cameraShakeMagnitude = 1f;
    [SerializeField] private float cameraShakeFalloff = 3f;

    private float shakeX = 0f;
    private float shakeY = 0f;
    private float shakeZ = 0f;
    public Quaternion OriginalCameraRotation { get; private set; }
    public Quaternion CameraPlayerMovement { get; private set; }



    [SerializeField] private Vector3 cameraRotationOffset;
    [SerializeField] private ParticleSystem speedLinesParticleSystem;
    [SerializeField] private ParticleSystem groundParticleSystem;
    public PlayerSounds PlayerSounds;
    
    public bool hasFreeDash = false;
    public BasicTimer DashCooldownTimer { get; private set; }
    public BasicTimer DashDurationTimer { get; private set; }
    public bool IsRotating { get; set; }
    public float CameraRotationSpeed { get; set; }

    private bool isPaused = false;

    public bool UsingFreeCamera = false;
    public BasicTimer VelocityTimer { get; set; }
    public Vector3 Direction;
    public bool IsChangingDirection { get; private set; }

    public float sensitiveRotationX;
    public float sensitiveRotationY;
    public bool canTakeInput = true;
    private Vector3 colliderCenter;


    override protected void Awake()
    {
        base.Awake();
        //objectCollider = transform.parent.GetComponent<BoxCollider>();
        playerValues = GetComponent<PlayerValues>();
        countdown = dashCooldown;
        DashCooldownTimer = new BasicTimer(dashCooldown);
        DashDurationTimer = new BasicTimer(dashDuration);
        IsWithinTriggerRange = false;
        DashLevel = 1;
    }


    public override void Start()
    {
        EventSystem.Current.RegisterListener<HayEatingFinishedEvent>(OnInteractionFinished);
        originalFOV = Camera.main.fieldOfView;
        hasFreeDash = false;
        UsingFreeCamera = true;
        anim = GetComponent<Animator>();
        cameraCollider = GameManager.instance.cam.GetComponent<SphereCollider>();
        EventSystem.Current.RegisterListener<PauseEvent>(Pause);
        EventSystem.Current.RegisterListener<ResumeEvent>(Resume);
        colliderCenter = objectCollider.center;
        colliderCenter = Vector3.zero;
    }

    private void Pause(PauseEvent eventInfo)
    {
        isPaused = true;
    }

    private void Resume(ResumeEvent eventInfo)
    {
        isPaused = false;
    }

    public void SetMouseCameraRotation(float x, float y, float cowX, float cowY)
    {
        rotationX = x;
        rotationY = y;
        sensitiveRotationX = x;
        sensitiveRotationY = y;
        meshParent.transform.eulerAngles = new Vector3(cowX, cowY, 0f);
    }

    //Code from:
    //https://answers.unity.com/questions/1203266/how-can-i-slowly-rotate-a-sprite-to-face-its-movem.html
    private IEnumerator Rotator(Vector3 direction)
    {
        IsRotating = true;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion currentRotation = meshParent.transform.rotation;
        for (float i = 0; i < 1.0f; i += Time.deltaTime / 0.1f)
        {
            if(IsRotating == false)
            {
                break;
            }
            meshParent.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, i);
            yield return null;
        }

        IsRotating = false;
    }

    private IEnumerator VelocityConstrainer(Vector3 newDirection)
    {
        IsChangingDirection = true;
        Vector3 targetDirection = newDirection;
        Vector3 currentDirection = (OriginalCameraRotation * Direction).normalized;
        for (float i = 0; i < 1.0f; i += Time.deltaTime / 6f)
        {
            Direction = Vector3.Lerp(currentDirection, newDirection, i);
            yield return null;
        }

        IsChangingDirection = false;
    }

    public void ShowParticles()
    {
        if(speedLinesParticleSystem.isStopped && groundParticleSystem.isStopped)
        {
            speedLinesParticleSystem.Play();
            groundParticleSystem.Play();
        }
        
    }

    public void HideParticles()
    {
        if (speedLinesParticleSystem.isStopped == false && groundParticleSystem.isStopped == false)
        {
            speedLinesParticleSystem.Stop();
            groundParticleSystem.Stop();
        }
    }

    public void RotatePlayer(Vector3 direction)
    {
        IEnumerator rotationRoutine = Rotator(direction);
        StartCoroutine(rotationRoutine);
    }

    public void ChangeDirection(Vector3 direction)
    {
        IEnumerator routine = VelocityConstrainer(direction);
        StartCoroutine(routine);
    }


    private void OnInteractionFinished(HayEatingFinishedEvent eventInfo)
    {
        Transition<WalkState>();
    }

    public void ResetDash()
    {
        elapsedDashTime = 0f;
        allowedToDash = false;
    }


    public void ShakeCamera()
    {
        cameraShakeIntensity = 2f;
    }


    public override void Update()
    {

        if(isPaused)
        {
            return;
        }


        OriginalCameraRotation = GameManager.instance.cam.transform.rotation;

        
        /*sensitiveRotationX = Mathf.Clamp(sensitiveRotationX, minAngle, maxAngle);
        CameraPlayerMovement = Quaternion.Euler(sensitiveRotationX, sensitiveRotationY, 0f);
        sensitiveRotationX = rotationX * CameraRotationSpeed;
        sensitiveRotationY = rotationY * CameraRotationSpeed;*/
        /*if(GetCurrentState().GetType() == typeof(DashState))
        {
            CameraPlayerMovement = 
        } else
        {

        }*/

        speedLinesParticleSystem.transform.rotation = OriginalCameraRotation;

        if(Input.GetKeyDown(KeyCode.C))
        {
            UsingFreeCamera = !UsingFreeCamera;

        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            ShakeCamera();
        }

        // if camShake:
        if(cameraShakeIntensity > 1f)
        {
            Camera cam = GameManager.instance.cam;
            cameraShakeIntensity -= 1 * cameraShakeFalloff * cameraShakeIntensity * Time.deltaTime;
            float cameraShakeValue = cameraShakeMagnitude * cameraShakeIntensity;

            shakeX = UnityEngine.Random.Range(-cameraShakeValue, cameraShakeValue);
            shakeY = UnityEngine.Random.Range(-cameraShakeValue, cameraShakeValue);
            shakeZ = UnityEngine.Random.Range(-cameraShakeValue, cameraShakeValue);
        } else
        {
            shakeX = 0f;
            shakeY = 0f;
            shakeZ = 0f;
        }


        IsWithinTriggerRange = false;
        base.Update();
        terminalVelocity = (dashStateAcceleration * Time.deltaTime) / (1 - Mathf.Pow(dashAirResistance, Time.deltaTime));
        //0 = 10
        //1 = maxSpeed
        /*if (!(GetCurrentState().GetType() == typeof(AirState)))
        {
            float normalizedFOV = velocity.magnitude / maxSpeed;
            Camera.main.fieldOfView = Mathf.Lerp(originalFOV, maxFOV, normalizedFOV);
        } else
        {
        }*/
        /*if (GetCurrentState().GetType() != typeof(DashState))
        {
            DashCooldownTimer.Update(Time.deltaTime);
            UIManager.instance.SetDashFillAmount(DashCooldownTimer.GetPercentage());
        }*/

        DashCooldownTimer.Update(Time.deltaTime);
        //UIManager.instance.SetDashFillAmount(DashCooldownTimer.GetPercentage());
        UIManager.instance.UpdateRamCooldown(DashCooldownTimer.GetPercentage());

        if (!allowedToDash)
        {
            if(elapsedDashTime >= dashCooldown)
            {
                allowedToDash = true;
                UIManager.instance.SetRamReady(true);
                //elapsedDashTime = 0f;
            } else
            {
                elapsedDashTime += Time.deltaTime;
            }
        }
        
        if (countdown > 0)
            countdown -= Time.deltaTime;

        if(cameraType == CameraType.First)
        {
            HandleCameraFirstPerson();
        } else
        {
            HandleCamera();
        }
    }

    Vector3 GetAllowedCameraMovement(Vector3 goalVector)
    {
        //RaycastHit hit;
        //bool okHit = Physics.SphereCast(transform.position + objectCollider.center + cameraRotationOffset, cameraCollider.radius, goalVector.normalized, out hit, goalVector.magnitude + cameraCollider.radius, collisionMask);
        Debug.DrawLine(transform.position + objectCollider.center + cameraRotationOffset, transform.position + objectCollider.center + cameraRotationOffset + goalVector, Color.red);
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + objectCollider.center + cameraRotationOffset, cameraCollider.radius, goalVector.normalized, goalVector.magnitude - cameraCollider.radius + 2f, collisionMask);

        /*if (okHit)
        {
            if (hit.collider != null && !isTagged(hit.collider))
            {
                Vector3 allowedMovement = goalVector.normalized * (hit.distance);
                return allowedMovement;
            }
        }
        return goalVector;*/

        foreach(RaycastHit hit in hits)
        {
            if (hit.collider != null && !isTagged(hit.collider))
            {
                Vector3 allowedMovement = goalVector.normalized * (hit.distance);

                if(goalVector.normalized.y <= 0f)
                {
                    //allowedMovement += cameraRotationOffset;
                }

                /*RaycastHit cameraHit;
                bool camHitOk = Physics.Raycast(Camera.main.transform.position - Vector3.up, Vector3.up, out cameraHit, cameraRotationOffset.y * 2, collisionMask);
                if(camHitOk && cameraHit.collider != null)
                {
                    return allowedMovement;
                } else
                {
                }*/

                return allowedMovement;
                
            }
        }
        return goalVector + cameraRotationOffset;
    }



    private bool isTagged(Collider col)
    {
        foreach(string tag in CameraIgnoreTags)
        {
            if (col.tag.Equals(tag))
            {
                return true;
            }
        }
        return false;
    }

    /*Vector3 GetAllowedCameraMovement(Vector3 goalVector)
    {*/
        /*RaycastHit[] hits = Physics.SphereCastAll(transform.position, cameraCollider.radius, goalVector.normalized, goalVector.magnitude, collisionMask);
        foreach(RaycastHit hit in hits)
        {
            Vector3 allowedMovement = goalVector.normalized * (hit.distance - cameraCollider.radius);
            return allowedMovement;
        }*/
        //Physics.CheckSphere(cameraCollider.transform.position, cameraCollider.radius, collisionMask);
  /*      Collider[] hits = Physics.OverlapSphere(cameraCollider.transform.position, cameraCollider.radius, collisionMask);
        foreach(Collider hit in hits)
        {
            
        }

        return goalVector;
    }
    */
    void HandleCameraFirstPerson()
    {
        float horizontal = Input.GetAxisRaw("Mouse X");
        float vertical = Input.GetAxisRaw("Mouse Y");

        rotationX -= vertical * mouseSensitivity;
        rotationY += horizontal * mouseSensitivity;

        rotationX = Mathf.Clamp(rotationX, minAngle, maxAngle);

        Camera.main.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        Camera.main.transform.position = transform.position;

    }

    protected void HandleCamera()
    {
        float horizontal = Input.GetAxisRaw("Mouse X");
        float vertical = Input.GetAxisRaw("Mouse Y");

        UIManager.instance.mouseDebug.text = "MouseX: " + horizontal + " \nMouseY: " + vertical;

        float localMouseSensitivity = mouseSensitivity;

        if(UsingFreeCamera)
        {
            localMouseSensitivity = 1;
        }

        rotationX -= vertical * localMouseSensitivity;
        rotationY += horizontal * localMouseSensitivity;

        

        rotationX = Mathf.Clamp(rotationX, minAngle, maxAngle);

        Vector3 rotationVector = new Vector3(rotationX + shakeX, rotationY + shakeY, 0f);
        Camera.main.transform.rotation = Quaternion.Euler(rotationVector);
        //Camera.main.transform.RotateAround(transform.position + objectCollider.center + cameraRotationOffset, rotationVector.normalized, rotationVector.magnitude);



        Vector3 cameraPlayerRelationship = Camera.main.transform.rotation * (cameraPositionRelativeToPlayer + cameraRotationOffset);
        Vector3 okToMove = GetAllowedCameraMovement(cameraPlayerRelationship);
        //Camera.main.transform.position = transform.position + (Vector3.up / 2) + objectCollider.center + okToMove;
        Camera.main.transform.position = transform.position + objectCollider.center + cameraRotationOffset + okToMove;
        //Vector3.Lerp(Camera.main.transform.position, transform.position + cameraRotationOffset + okToMove, Time.deltaTime * 30f);

    }

    public void ResetCooldown()
    {
        countdown = dashCooldown;
    }

    public float getCooldown()
    {
        return dashCooldown;
    }

    public void Respawn(Vector3 position)
    {
        Debug.Log("Finns");
        transform.position = position;
    }

    private void LateUpdate()
    {
        if (!IsWithinTriggerRange)
        {
            UIManager.instance.HideInteractionIndicator();
        }
    }
}
