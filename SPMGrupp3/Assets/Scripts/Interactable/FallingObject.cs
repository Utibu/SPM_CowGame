﻿using System.Collections;
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
    public GameObject pivot;
    private Vector3 size;
    public bool freeFall = true;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        size = meshRenderer.bounds.size;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 initialRotation = transform.eulerAngles;
        initialRotation.y = 0f;
        transform.eulerAngles = initialRotation;
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
            GameManager.instance.player.ShakeCamera();
            //direction = matchDirection;
            isFalling = true;
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
            //Debug.Log(hit.distance);
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
        //collisionCheck.transform.forward
        //new Vector3(direction.z * -1, direction.y, direction.x)
        Vector3 tempDir = transform.rotation * direction;
        bool ray = Physics.Raycast(collisionCheck.transform.position, tempDir, out hit, float.MaxValue, layerMask);
        
        if(ray)
        {
            if(hit.distance < rotThisFrame.magnitude)
            {
                isFalling = false;
                hasFallen = true;
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
        //collisionCheck.transform.forward
        //new Vector3(direction.z * -1, direction.y, direction.x)
        //Debug.Log("ROTTHISFRAME: " + rotThisFrame.magnitude);
        Vector3 tempDir = transform.rotation * direction;
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, meshRenderer.bounds.size / 2, tempDir, Quaternion.identity, float.MaxValue, layerMask);
        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.tag.Equals("Enemy"))
            {
                Debug.Log("HIT DISTANCE: " + hit.distance);
            }
            if (hit.distance < 0.1f && hit.collider.tag.Equals("Enemy"))
            {
                Debug.Log("DISTANCE: " + hit.distance);
                Destroy(hit.collider.gameObject);
            }
        }
    }

    // Update is called once per frame
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
            //Debug.Log(rotationDirection.normalized);
        }
    }
}
