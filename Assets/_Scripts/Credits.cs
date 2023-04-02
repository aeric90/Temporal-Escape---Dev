using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public GameObject creditsObject;
    
    public void CreditsToggle()
    {
        creditsObject.SetActive(!creditsObject.activeSelf);
    }

}
