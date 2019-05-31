using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerInputMenu : MonoBehaviour
{

    [SerializeField] private List<Button> buttons;
    public List<Button> Buttons { get { return buttons; } private set { buttons = value; } }
    private InputManager inputManager = new InputManager();
    private BasicTimer controllerTimer;
    int currentlyChosenMenuItem;
    private Color originalColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private float moveCooldown;

    // Start is called before the first frame update
    void Start()
    {
        controllerTimer = new BasicTimer(moveCooldown);
        originalColor = buttons[currentlyChosenMenuItem].GetComponent<Image>().color;
        currentlyChosenMenuItem = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerTimer.IsCompleted(Time.deltaTime, false, true) && Input.GetAxisRaw("Vertical") != 0)
        {
            controllerTimer.Reset();
            float vertical = Input.GetAxisRaw("Vertical");
            if (currentlyChosenMenuItem >= 0)
            {
                buttons[currentlyChosenMenuItem].GetComponent<Image>().color = originalColor;
            }

            if (vertical > 0)
            {
                currentlyChosenMenuItem = (currentlyChosenMenuItem - 1 + buttons.Count) % buttons.Count;
            }
            else
            {
                currentlyChosenMenuItem = (currentlyChosenMenuItem + 1) % buttons.Count;
            }
            buttons[currentlyChosenMenuItem].GetComponent<Image>().color = selectedColor;
            //Debug.Log("CURRENT INDEX: " + currentlyChosenMenuItem);
        }

        if (inputManager.JumpKeyDown())
        {
            buttons[currentlyChosenMenuItem].onClick.Invoke();
        }
    }
}
