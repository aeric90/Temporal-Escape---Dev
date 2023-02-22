using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIButtonController : MonoBehaviour
{
    public Canvas InGameMenuCanvas;
    private bool UITrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Right Hand Grab Controller" && UITrigger == false)
        {
            InGameMenuCanvas.enabled = !InGameMenuCanvas.enabled;   
            UITrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Right Hand Grab Controller")
        {
            UITrigger = false;
        }
    }
}
