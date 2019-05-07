using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    public float spinSpeed = 50f;
    void Update()
    {
        transform.Rotate(Vector3.left, spinSpeed * Time.deltaTime);
    }
}
