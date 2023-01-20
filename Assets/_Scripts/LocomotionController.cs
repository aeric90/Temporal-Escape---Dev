using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    public XRController leftTeleportRay;
    public XRController rightTeleportRay;

    public XRController leftInteractionRay;
    public XRController rightInteractionRay;

    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject rightReticle = GameObject.FindWithTag("RightTeleportReticle");
        GameObject leftReticle = GameObject.FindWithTag("LeftTeleportReticle");

        if (CheckIfActivated(rightTeleportRay))
        {
            rightTeleportRay.gameObject.SetActive(true);
            rightInteractionRay.gameObject.SetActive(false);
        } else
        {
            rightTeleportRay.gameObject.SetActive(false);
            rightInteractionRay.gameObject.SetActive(true);
            if (rightReticle != null) rightReticle.SetActive(false);
        }

        if (CheckIfActivated(leftTeleportRay))
        {
            leftTeleportRay.gameObject.SetActive(true);
            leftInteractionRay.gameObject.SetActive(false);
        }
        else
        {
            leftTeleportRay.gameObject.SetActive(false);
            leftInteractionRay.gameObject.SetActive(true);
            if (leftReticle != null) leftReticle.SetActive(false);
        }

        if (test()) TemporalEscapeController.instance.SwitchRoom("Menu Room");

        InputHelpers.IsPressed(rightInteractionRay.inputDevice, InputHelpers.Button.PrimaryButton, out bool temporalSpoof, activationThreshold);
        if (temporalSpoof) TemporalController.instance.TemporalSpoof();
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }

    public bool test()
    {
        InputHelpers.IsPressed(leftInteractionRay.inputDevice, InputHelpers.Button.Primary2DAxisClick, out bool isActivatedLeft, activationThreshold);
        InputHelpers.IsPressed(rightInteractionRay.inputDevice, InputHelpers.Button.Primary2DAxisClick, out bool isActivatedRight, activationThreshold);
        return isActivatedLeft && isActivatedRight;
    }
}