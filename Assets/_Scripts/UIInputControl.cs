using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UIInputControl : MonoBehaviour
{
    public XRController leftInteractionRay;
    public XRController rightInteractionRay;

    public XRController leftUIRay;
    public XRController righUIRay;

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(leftInteractionRay.inputDevice, InputHelpers.Button.Primary2DAxisClick, out bool isActivatedLeft);
        InputHelpers.IsPressed(rightInteractionRay.inputDevice, InputHelpers.Button.Primary2DAxisClick, out bool isActivatedRight);

        if (isActivatedLeft && isActivatedRight)
        {
            // Return to menu
                // Disable interaction, enable UI
                // Fade Out
                // Hide current room
                // Show Menu room
                // Fade In
        }
    }
}
