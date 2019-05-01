using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private GameObject[] invArr = {null, null, null};

    private GameObject key1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    /*
    public bool pickupObj(GameObject pickup)
    {
        for(int i = 0; i <= 2; i++)
        {
            if (invArr[i] == null)
            {
                invArr[i] = pickup;
                Destroy(pickup);
                
                return true;
            }
        }
        // if no space
        // do some error signifier
        Debug.Log("no space in inventory");
        return false;
    }

    public bool haveKey()
    {
        for (int i = 0; i <= invArr.Length; i++)
        {
            if (invArr[i].name == "Key")
            {
                return true;
            }
        }
        return false;
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }
}
