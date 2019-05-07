using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBoxScript : MonoBehaviour
{
    public GameObject RampSwitch;
    ArrayList lamps;
    private Dictionary<string , Color> colors; // sparar alla originella emission-färger för toggling
    private Dictionary<string, bool> lampStatuses;

    private Color  keeper;

    // Start is called before the first frame update
    void Start()
    {
        lamps = new ArrayList();
        colors = new Dictionary<string, Color>();
        lampStatuses = new Dictionary<string, bool>();

        lamps.Add("Red");
        lamps.Add("Blue");
        lamps.Add("Green");
        lamps.Add("Orange");
        lamps.Add("Turquoise");
        lamps.Add("Pink");
        lamps.Add("Yellow");
        lamps.Add("Purple");

        //Debug.Log(transform.Find("Turquoise").GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));

        foreach (string color in lamps)
        {
            // save each original color from editor
            //keeper = transform.Find(color).GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
            Debug.Log(keeper + " " + color);
            colors.Add(color, transform.Find(color).GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));

            // set light statuses to correct bool according to if lamp is emissive or not
            lampStatuses.Add(color, true);
        }

        // TEMPORARY: 
        //(these colored lights will be turned off at start)
        transform.Find("Green").GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
        lampStatuses["Green"] = false;
        transform.Find("Pink").GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
        lampStatuses["Pink"] = false;
        transform.Find("Red").GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.black);
        lampStatuses["Red"] = false;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleLightButton(string colorName)
    {
        Material mat = transform.Find(colorName).GetComponent<MeshRenderer>().material;
        bool isEmissive;
        lampStatuses.TryGetValue(colorName, out isEmissive);
        if (!isEmissive)
        {
            Color EmissionColor;
            colors.TryGetValue(colorName, out EmissionColor);
            mat.SetColor("_EmissionColor", EmissionColor);
            lampStatuses[colorName] = true;
        }
        else
        {
            mat.SetColor("_EmissionColor", Color.black);
            lampStatuses[colorName] = false;
        }

        
        CheckAllLights();
    }

    private void CheckAllLights()
    {
        Debug.Log("Checking all lights");
        foreach (string colorName in lamps)
        {
            Material mat = transform.Find(colorName).GetComponent<MeshRenderer>().material;
            if (mat.GetColor("_EmissionColor") == Color.black)
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
