using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialToggle : MonoBehaviour
{
    public GameObject tutorial;

    public void Toggle()
    {
        tutorial.SetActive(!tutorial.activeSelf);
    }

}
