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

    
    
    public bool isDashing = false;
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
    private float camShakeIntensity = 0f;
    private float camShakeMagnitude = 5f;
    private float camShakeFalloff = 3f;


    [SerializeField] private Vector3 cameraRotationOffset;
    
    public bool hasFreeDash = false;
    public BasicTimer DashCooldownTimer { get; private set; }
    public BasicTimer DashDurationTimer { get; private set; }
    public bool IsRotating { get; private set; }

    private bool isPaused = false;

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
        anim = GetComponent<Animator>();
        cameraCollider = GameManager.instance.cam.GetComponent<SphereCollider>();
        EventSystem.Current.RegisterListener<PauseEvent>(Pause);
        EventSystem.Current.RegisterListener<ResumeEvent>(Resume);
    }

    private void Pause(PauseEvent eventInfo)
    {
        isPaused = true;
    }

    private void Resume(ResumeEvent eventInfo)
    {
        isPaused = false;
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
            meshParent.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, i);
            yield return null;
        }

        IsRotating = false;
    }

    public void RotatePlayer(Vector3 direction)
    {
        IEnumerator rotationRoutine = Rotator(direction);
        StartCoroutine(rotationRoutine);
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
        camShakeIntensity = 50f; 

        /*
             static var shakeInt = 50.0;
             var decrease = 5.0;
             var magnitude = 5.0;
             function Update () {
                 if (shakeInt < 0){
                 shakeInt = 0;    
                 }
                 if (shakeInt != 0){
                     shakeInt -= 1*decrease*shakeInt*Time.deltaTime;
                 }
                 var cam = camera.main.transform;
                 if (shakeInt > 0){
                 cam.rotation.x += Random.Range(-magnitude*shakeInt, magnitude*shakeInt);
                 cam.rotation.y += Random.Range(-magnitude*shakeInt, magnitude*shakeInt);
                 cam.rotation.z += Random.Range(-magnitude*shakeInt, magnitude*shakeInt);
                 }
             }
         */
    }


    public override void Update()
    {

        if(isPaused)
        {
            return;
        }

        // if camShake:
        if(camShakeIntensity > 1f)
        {
            var cam = Camera.main.transform;
            camShakeIntensity -= 1 * camShakeFalloff * camShakeIntensity * Time.deltaTime;

            float shakeX = UnityEngine.Random.Range(-camShakeMagnitude * camShakeIntensity, camShakeMagnitude * camShakeIntensity);
            float shakeY = UnityEngine.Random.Range(-camShakeMagnitude * camShakeIntensity, camShakeMagnitude * camShakeIntensity);
            float shakeZ = UnityEngine.Random.Range(-camShakeMagnitude * camShakeIntensity, camShakeMagnitude * camShakeIntensity);

            //cam.rotation = Quaternion.Euler(shakeX, shakeY, shakeZ);
            /*
            cam.rotation.x += System.Random.Range(-camShakeMagnitude * camShakeIntensity, camShakeMagnitude * camShakeIntensity);
            cam.rotation.y += System.Random.Range(-camShakeMagnitude * camShakeIntensity, camShakeMagnitude * camShakeIntensity);
            cam.rotation.z += System.Random.Range(-camShakeMagnitude * camShakeIntensity, camShakeMagnitude * camShakeIntensity);
            */
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
        if (GetCurrentState().GetType() != typeof(DashState))
        {
            DashCooldownTimer.Update(Time.deltaTime);
            UIManager.instance.SetDashFillAmount(DashCooldownTimer.GetPercentage());
        }

        if (!allowedToDash)
        {
            if(elapsedDashTime >= dashCooldown)
            {
                allowedToDash = true;
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
        RaycastHit hit;
        bool okHit = Physics.SphereCast(transform.position + objectCollider.center + cameraRotationOffset, cameraCollider.radius, goalVector.normalized, out hit, goalVector.magnitude, collisionMask);
        if (okHit)
        {
            if (hit.collider != null && !isTagged(hit.collider))
            {
                Vector3 allowedMovement = goalVector.normalized * (hit.distance - cameraCollider.radius);
                return allowedMovement;
            }
        }
        return goalVector;
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

        rotationX -= vertical * mouseSensitivity;
        rotationY += horizontal * mouseSensitivity;

        rotationX = Mathf.Clamp(rotationX, minAngle, maxAngle);

        Camera.main.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);

        Vector3 cameraPlayerRelationship = Camera.main.transform.rotation * cameraPositionRelativeToPlayer;
        Vector3 okToMove = GetAllowedCameraMovement(cameraPlayerRelationship);
        //Camera.main.transform.position = transform.position + (Vector3.up / 2) + objectCollider.center + okToMove;
        Camera.main.transform.position = transform.position + cameraRotationOffset + objectCollider.center + okToMove;

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
