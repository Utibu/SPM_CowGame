using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{

    public GameObject gateKey;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Open()
    {
        // do a nice gate open rotation or start anim or whatever
        // but for now: just move it aside so player can pass
        transform.position += new Vector3(0.0f, 50.0f, 0.0f);
        Destroy(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
