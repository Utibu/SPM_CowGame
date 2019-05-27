using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : Triggable
{
    private Collider colli;
    public LayerMask masken;
    public float fallTime = 1.0f;
    private bool broke = false;
    public bool Broke { get { return broke; } set { broke = value; } }
    private bool falling = false;
    private float speed = 0.1f;
    public float countdown;
    Vector3 toGround;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        colli = GetComponent<BoxCollider>();
        GameManager.instance.SaveManager.TrapObjects.Add(GetComponent<Saveable>().Id, this);
        //countdown = fallTime;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //if (!broke)
        //{
            //DetectCow();
        //}
        if (falling && !broke)
        {
            if (countdown <= 0)
            {
                transform.position += Vector3.down * Time.deltaTime * speed;
                RaycastHit hitinfo;
                bool hit = Physics.Raycast(colli.transform.position, Vector3.down, out hitinfo, 0.2f, masken);
                if (hit)
                {
                    Debug.Log("hit " + hitinfo);
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

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
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
            //broke = true;

            /*
            hit = Physics.Raycast(colli.transform.position, Vector3.down, out hitinfo, 10.0f, masken);

            toGround = Vector3.down * hitinfo.distance;

            //transform.position += toGround;
            */
            
        }

    }

}
