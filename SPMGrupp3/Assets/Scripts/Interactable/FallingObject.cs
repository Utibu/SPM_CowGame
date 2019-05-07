﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
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
        if(isFalling)
        {
            isFalling = false;
            SetFalling(direction);
        }
    }

    public void SetFalling(Vector3 matchDirection)
    {
        if(!isFalling && !hasFallen)
        {
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
            //pivot.transform.position += (tempDir.normalized + (tempSize / 2));
            //collisionCheck.transform.rotation = Quaternion.Euler(tempDir);
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

    // Update is called once per frame
    void Update()
    {
        Vector3 tempDir = transform.rotation * direction;
        Debug.DrawLine(collisionCheck.transform.position, collisionCheck.transform.position + tempDir * 10f, Color.blue);
        if (isFalling)
        {
            Vector3 rot = direction.normalized * acceleration * Time.deltaTime;
            Vector3 toRot = DistanceToGround(rot);

            

            //Debug.Log("ROT " + rot);
            //Vector3 r = Helper.RotateAroundPivot(transform.position, pivot.transform.position, rot);
            //Debug.Log(r);
            //transform.Rotate(r.z, 0f, r.x);
            transform.RotateAround(pivot.transform.position, rotationDirection.normalized, rot.magnitude);
            //transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, 0f, transform.rotation.eulerAngles.z));
            Debug.Log(rotationDirection.normalized);
            //collisionCheck.transform.LookAt(transform.rotation * Vector3.forward);
            //Debug.Log(collisionCheck.transform.rotation);
            //transform.rotation = Quaternion.Euler(r);
            //transform.Rotate(r);

            //transform.parent.Rotate(10f, 10f, 10f);
        }
    }
}
