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
    public float dashAirResistance;
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
        cameraCollider = Camera.main.GetComponent<SphereCollider>();
        playerValues = GetComponent<PlayerValues>();
        countdown = dashCooldown;
    }

    private void Start()
    {
        EventSystem.Current.RegisterListener<HayEatingFinishedEvent>(OnInteractionFinished);
        originalFOV = Camera.main.fieldOfView;
        hasFreeDash = false;
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

        //0 = 10
        //1 = maxSpeed
        if(!(GetCurrentState().GetType() == typeof(AirState)))
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
        bool okHit = Physics.SphereCast(transform.position, cameraCollider.radius, goalVector.normalized, out hit, goalVector.magnitude, collisionMask);
        if (okHit)
        {
            if (hit.collider != null)
            {
                Vector3 allowedMovement = goalVector.normalized * (hit.distance - cameraCollider.radius);
                return allowedMovement;
            }
        }

        return goalVector;
    }

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
        Camera.main.transform.position = transform.position + okToMove;

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
        this.transform.position = position;
    }
}
