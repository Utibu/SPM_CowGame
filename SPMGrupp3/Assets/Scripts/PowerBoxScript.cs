using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBoxScript : MonoBehaviour
{
    public GameObject RampSwitch;
    ArrayList lamps = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {
        lamps.Add("Red");
        lamps.Add("Blue");
        lamps.Add("Green");
        lamps.Add("Orange");
        lamps.Add("Turquoise");
        lamps.Add("Pink");
        lamps.Add("Yellow");
        lamps.Add("Purple");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleLightButton(string color)
    {
        Debug.Log("light should be togglin'");
        
        bool lightIsOn = transform.Find(color).GetComponent<Light>().enabled;
        lightIsOn = !lightIsOn;
        transform.Find(color).GetComponent<Light>().enabled = lightIsOn;
        
        // Ifall man vill pröva ha emissive material och välxla mellan sånt ist för ljus
        /*
        Color finalValue =  * someValue; // someValue adjust the scale of emission
        DynamicGI.SetEmissive(GetComponent<Renderer>(), finalValue);
        */

        /*
        transform.Find(color).GetComponent<Material>().color *= 0f;
        transform.Find(color)
        */

        CheckAllLights();
    }

    private void CheckAllLights()
    {
        Debug.Log("Checking all lights");
       
        foreach (string color in lamps)
        {
            if(transform.Find(color).GetComponent<Light>().enabled == false)
            {
                RampSwitch.GetComponent<MakeRampButton>().SetUnactive();
                return;
            }
        }
        //Activate switch
        Debug.Log("activating switch activating switch activating switch");
        RampSwitch.GetComponent<MakeRampButton>().SetActive();
        //Disable powerbox
        // ÖVER MIN DÖDA KROPP. RÖR EJ
    }

}
