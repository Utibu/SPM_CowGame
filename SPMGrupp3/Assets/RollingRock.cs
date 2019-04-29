using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingRock : MonoBehaviour
{

    private Rigidbody rb;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if((time % 60 > 10 && rb.velocity.magnitude < 1f) || time % 60 > 120) {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision col) {
        if(col.collider.tag.Equals("Player") && rb.velocity.magnitude > 10f) {
            GameManager.instance.player.playerValues.health -= 25;
            Destroy(this.gameObject);
        }
    }
}
