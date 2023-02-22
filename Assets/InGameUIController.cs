using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InGameUIController : MonoBehaviour
{
    public TeleportationProvider teleportationProvider;
    public SnapTurnProviderBase snapTurnProvider;

    public GameObject leftRayController;
    public GameObject rightRayController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TeleportButton()
    {
        teleportationProvider.enabled = !teleportationProvider.enabled;
    }

    public void RotateButton()
    {
        snapTurnProvider.enabled = !snapTurnProvider.enabled;
    }

    public void RayGrabButton()
    {
        leftRayController.SetActive(!leftRayController.activeSelf);
        rightRayController.SetActive(!rightRayController.activeSelf);
    }
}
