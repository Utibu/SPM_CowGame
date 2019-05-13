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

    private float originalFOV;
    public float maxFOV;
    
    public bool isDashing = false;
    [HideInInspector] public float elapsedDashTime;
    public bool allowedToDash = true;
    public float dashCooldown = 5f;

    public string[] CameraIgnoreTags;

    public float dashStateAcceleration;
    public float dashStateGravity;
    public float dashAirResistance;
    public float terminalVelocity;

    public GameObject meshParent;

    [HideInInspector] public Animator anim;
    public float animationSpeed;

    public float mouseSensitivity;
    float rotationX;
    float rotationY;
    public float minAngle;
    public float maxAngle;
    public Vector3 cameraPositionRelativeToPlayer;
    public CameraType cameraType;
    
    [HideInInspector] public bool hasFreeDash = false;

    override protected void Awake()
    {
        base.Awake();
        //objectCollider = transform.parent.GetComponent<BoxCollider>();
        playerValues = GetComponent<PlayerValues>();
        countdown = dashCooldown;

    }

    private void Start()
    {
        EventSystem.Current.RegisterListener<HayEatingFinishedEvent>(OnInteractionFinished);
        originalFOV = Camera.main.fieldOfView;
        hasFreeDash = false;
        anim = GetComponent<Animator>();
        cameraCollider = GameManager.instance.cam.GetComponent<SphereCollider>();
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

    public override void Update()
    {
        base.Update();

        terminalVelocity = (dashStateAcceleration * Time.deltaTime) / (1 - Mathf.Pow(dashAirResistance, Time.deltaTime));

        //0 = 10
        //1 = maxSpeed
        if (!(GetCurrentState().GetType() == typeof(AirState)))
        {
            float normalizedFOV = velocity.magnitude / maxSpeed;
            Camera.main.fieldOfView = Mathf.Lerp(originalFOV, maxFOV, normalizedFOV);
        } else
        {

        }


        if(!allowedToDash)
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
        bool okHit = Physics.SphereCast(transform.position + objectCollider.center, cameraCollider.radius, goalVector.normalized, out hit, goalVector.magnitude, collisionMask);
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
        Camera.main.transform.position = transform.position + objectCollider.center + okToMove;

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
}
