using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIButtonController : MonoBehaviour
{
    public GameObject InGameMenu;
    private bool UITrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Right Hand Grab Controller" && UITrigger == false)
        {
            InGameMenu.SetActive(!InGameMenu.activeSelf);
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
