using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobController : MonoBehaviour
{

    private Animator anim;
    private float speed;
    private float direction;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("wow");
        speed = Input.GetAxis("Vertical");
        direction = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", speed);
        anim.SetFloat("Direction", direction);

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Taunt");
        }
    }
}
