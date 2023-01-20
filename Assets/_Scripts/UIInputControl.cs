using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UIInputControl : MonoBehaviour
{
    public static UIInputControl instance;

    public GameObject leftInteractionRay;
    public GameObject rightInteractionRay;
    public GameObject leftUIRay;
    public GameObject righUIRay;

    private void Start()
    {
        if(instance == null) instance = this;
    }

    public void SetControllersToUI()
    {
        leftInteractionRay.SetActive(false);
        rightInteractionRay.SetActive(false);
        leftUIRay.SetActive(true);
        righUIRay.SetActive(true);
    }

    public void SetControllersToGame()
    {
        leftInteractionRay.SetActive(true);
        rightInteractionRay.SetActive(true);
        leftUIRay.SetActive(false);
        righUIRay.SetActive(false);
    }
}
