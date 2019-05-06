using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingObject : MonoBehaviour
{
    public GameObject drop;

    public void OnEnter(PlayerValues player)
    {
        GameObject go = Instantiate(drop.gameObject, this.transform.position, Quaternion.identity);
    }
}
