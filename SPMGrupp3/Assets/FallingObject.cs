using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public Vector3 direction;
    public bool isFalling = false;
    public float acceleration;
    public LayerMask layerMask;
    public float checkDistance = 0.2f;
    public GameObject collisionCheck;
    public MeshRenderer meshRenderer;
    private float smallestValue = -1; //To not overshoot
    private bool hasFallen = false;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetFalling(Vector3 matchDirection)
    {
        if(!isFalling && !hasFallen)
        {
            //direction = matchDirection;
            isFalling = true;
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
        bool ray = Physics.Raycast(collisionCheck.transform.position, collisionCheck.transform.forward, out hit, float.MaxValue, layerMask);
        
        if(ray)
        {
            if(hit.distance < rotThisFrame.magnitude)
            {
                isFalling = false;
                hasFallen = true;
                Debug.LogWarning("NHHH");
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
        if(isFalling)
        {
            Vector3 rot = direction.normalized * acceleration * Time.deltaTime;
            Vector3 toRot = DistanceToGround(rot);
            /*if(HitGround())
            {
                isFalling = false;
                return;
            }*/

           
            transform.parent.Rotate(toRot.z, 0f, toRot.x);
            
            //transform.parent.Rotate(10f, 10f, 10f);
        }
    }
}
