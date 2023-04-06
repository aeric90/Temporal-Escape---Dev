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

    private TeleportationProvider teleportationProvider;

    // Start is called before the first frame update
    void Start()
    {
        teleportationProvider = GetComponentInParent<TeleportationProvider>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject rightReticle = GameObject.FindWithTag("RightTeleportReticle");
        GameObject leftReticle = GameObject.FindWithTag("LeftTeleportReticle");

        if (teleportationProvider.enabled)
        {
            if (CheckIfActivated(rightTeleportRay))
            {
                rightTeleportRay.gameObject.SetActive(true);
                rightInteractionRay.gameObject.SetActive(false);
            }
            else
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
        }

        if (test()) TemporalEscapeController.instance.SwitchRoom("Menu Room");

        InputHelpers.IsPressed(rightInteractionRay.inputDevice, InputHelpers.Button.PrimaryButton, out bool temporalSpoofA, activationThreshold);
        InputHelpers.IsPressed(rightInteractionRay.inputDevice, InputHelpers.Button.SecondaryButton, out bool temporalSpoofB, activationThreshold);
        InputHelpers.IsPressed(leftInteractionRay.inputDevice, InputHelpers.Button.PrimaryButton, out bool temporalSpoofC, activationThreshold);
        InputHelpers.IsPressed(leftInteractionRay.inputDevice, InputHelpers.Button.SecondaryButton, out bool temporalSpoofD, activationThreshold);

        if (temporalSpoofA) TemporalController.instance.Network_Event("Key Pick Up");
        if (temporalSpoofB) TemporalController.instance.Network_Event("Wall Smash");
        if (temporalSpoofC) TemporalController.instance.Network_Event("Eyeball Send");
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
