//Main Author: Niklas Almqvist
//Secondary Author: Joakim Ljung
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : Dashable
{
    public Vector3 direction;
    public Vector3 rotationDirection;
    public bool isFalling = false;
    public float acceleration;
    public LayerMask layerMask;
    public float checkDistance = 0.2f;
    public GameObject collisionCheck;
    public MeshRenderer meshRenderer;
    private float smallestValue = -1; //To not overshoot
    private bool hasFallen = false;
    public bool HasFallen { get { return hasFallen; } set { hasFallen = value; } }
    public GameObject pivot;
    private Vector3 size;
    public bool freeFall = true;
    [SerializeField] private ParticleSystem particles;
    private AudioSource audioSource;
    [SerializeField] private AudioClip fallingSound;
    [SerializeField] private AudioClip groundImpactSound;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        size = meshRenderer.bounds.size;
    }

    public override void Start()
    {
        base.Start();
        Vector3 initialRotation = transform.eulerAngles;
        initialRotation.y = 0f;
        transform.eulerAngles = initialRotation;
        GameManager.instance.SaveManager.FallingObjects.Add(GetComponent<Saveable>().Id, this);
        if(isFalling)
        {
            isFalling = false;
            SetFalling(direction);
        }
    }

    public override void OnPlayerCollideEnter(Collider hitCollider, out bool skipCollision, int dashLevel)
    {
        skipCollision = false;
        SetFalling(GameManager.instance.player.velocity.normalized);
        
        
    }

    public void SetFalling(Vector3 matchDirection)
    {
        if(!isFalling && !hasFallen)
        {
            if (particles != null && particles.isPlaying == false)
            {
                particles.Play();
            }
            GameManager.instance.player.ShakeCamera();
            isFalling = true;
            EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, GetClip(), 0.5f, 1f, 1f));
            if (audioSource != null)
            {
                audioSource.PlayOneShot(fallingSound);
            }
            matchDirection.y = 0f;
            if(freeFall)
            {
                direction = matchDirection;
            }
            
            Debug.Log("DIR: " + direction.normalized);
            rotationDirection = new Vector3(direction.z, 0f, direction.x * -1);
            Vector3 tempSize = size;
            tempSize.y = 0f;
            pivot.transform.position += new Vector3(tempSize.x / 2 * rotationDirection.z * -1, 0f, tempSize.z / 2 * rotationDirection.x);
            
        }
        
    }

    public bool HitGround()
    {
        Vector3 size = new Vector3(meshRenderer.bounds.size.x / 2, 4f, meshRenderer.bounds.size.x / 2);
        RaycastHit hit;
        bool ray = Physics.Raycast(collisionCheck.transform.position, collisionCheck.transform.forward, out hit, float.MaxValue, layerMask);
        
        Debug.DrawLine(collisionCheck.transform.position, collisionCheck.transform.position + collisionCheck.transform.forward * 10f, Color.blue);
        if(ray)
        {
            if(smallestValue < 0)
            {
                smallestValue = hit.distance;
            }
            if(hit.distance <= checkDistance || hit.distance > smallestValue)
            {
                return true;
            }

            smallestValue = hit.distance;
            
        }

        return false;
    }

    public Vector3 DistanceToGround(Vector3 rotThisFrame)
    {
        RaycastHit hit;
        Vector3 tempDir = transform.rotation * direction;
        bool ray = Physics.Raycast(collisionCheck.transform.position, tempDir, out hit, float.MaxValue, layerMask);
        
        if(ray)
        {
            if(hit.distance < rotThisFrame.magnitude)
            {
                isFalling = false;
                hasFallen = true;
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, groundImpactSound, 1f, 1f, 1f));
                return rotThisFrame.normalized * (rotThisFrame.magnitude - hit.distance);
            } else
            {
                return rotThisFrame;
            }
        }

        return rotThisFrame;
    }

    public void CheckEnemyHits(Vector3 rotThisFrame)
    {
        Vector3 tempDir = transform.rotation * direction;
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, transform.localScale / 2, tempDir, transform.rotation, float.MaxValue, layerMask);
        foreach (RaycastHit hit in hits)
        {
            if (hit.distance < 0.1f && hit.collider.tag.Equals("Enemy"))
            {
                if(hit.collider.GetComponent<Peasant>().DoingKnockback == false)
                {
                    Vector3 newDirection = Vector3.Cross(direction, Vector3.up);
                    hit.collider.GetComponent<Peasant>().PlayerDash(Vector3.zero, false);
                }
                
            }
        }
    }

    void Update()
    {
        Vector3 tempDir = transform.rotation * direction;
        Debug.DrawLine(collisionCheck.transform.position, collisionCheck.transform.position + tempDir * 10f, Color.blue);
        if (isFalling)
        {
            Vector3 rot = direction.normalized * acceleration * Time.deltaTime;
            Vector3 toRot = DistanceToGround(rot);
            transform.RotateAround(pivot.transform.position, rotationDirection.normalized, rot.magnitude);
            CheckEnemyHits(rot);
        }
    }
}
