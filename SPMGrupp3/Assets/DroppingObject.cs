using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingObject : MonoBehaviour
{
    public DroppableObject drop;

    public void OnEnter(PlayerValues player)
    {
        GameObject go = Instantiate(drop.gameObject, this.transform.position, Quaternion.identity);
        go.GetComponent<DroppableObject>().OnCreate(player);
    }
}
