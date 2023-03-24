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

    public GameObject accessibilityControls;
    public GameObject soundControls;
    public GameObject exitControls;

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

    public void AccessibilityMenuButton()
    {
        accessibilityControls.SetActive(true);
        soundControls.SetActive(false);
        exitControls.SetActive(false);
    }

    public void SoundMenuButton()
    {
        accessibilityControls.SetActive(false);
        soundControls.SetActive(true);
        exitControls.SetActive(false);
    }

    public void ExitMenuButton()
    {
        accessibilityControls.SetActive(false);
        soundControls.SetActive(false);
        exitControls.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TeleportToggle()
    {
       teleportationProvider.enabled = !teleportationProvider.enabled;
    }

    public void RotateToggle()
    {
        snapTurnProvider.enabled = !snapTurnProvider.enabled;
    }

    public void RayGrabToggle()
    {
        leftRayController.SetActive(!leftRayController.activeSelf);
        rightRayController.SetActive(!rightRayController.activeSelf);
    }
}
