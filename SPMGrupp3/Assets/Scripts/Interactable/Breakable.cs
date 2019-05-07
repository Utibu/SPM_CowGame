using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private Collider colli;
    public LayerMask masken;
    public float fallTime = 1.0f;
    private bool broke = false;
    private bool falling = false;
    private float speed = 0.1f;
    public float countdown;
    Vector3 toGround;

    // Start is called before the first frame update
    void Start()
    {
        colli = GetComponent<BoxCollider>();
        //countdown = fallTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!broke)
        {
            //DetectCow();
        }
        if (falling && !broke)
        {
            if (countdown <= 0)
            {
                transform.position += Vector3.down * Time.deltaTime * speed;
                RaycastHit hitinfo;
                bool hit = Physics.Raycast(colli.transform.position, Vector3.down, out hitinfo, 0.2f, masken);
                if (hit)
                {
                    falling = false;
                    broke = true;
                }
                else
                {
                    speed += 0.5f;
                }
            }
            else
                countdown -= Time.deltaTime;
        }
    }

    public void SetFall()
    {
        falling = true;
            
    }


    private void DetectCow()
    {
        RaycastHit hitinfo;
        bool hit = Physics.Raycast(colli.transform.position, Vector3.up, out hitinfo, 1.0f, masken);

        if (hit)
        {
            Debug.Log("cow detected");
            falling = true;
            broke = true;

            /*
            hit = Physics.Raycast(colli.transform.position, Vector3.down, out hitinfo, 10.0f, masken);

            toGround = Vector3.down * hitinfo.distance;

            //transform.position += toGround;
            */
            
        }

    }

}
