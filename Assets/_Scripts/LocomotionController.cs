using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class LocomotionController : MonoBehaviour
{
    MailboxController mailbox = null;  // Holds the current object's mailbox object

    public XRController leftTeleportRay;
    public XRController rightTeleportRay;

    public XRController leftInteractionRay;
    public XRController rightInteractionRay;

    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;

    private TeleportationProvider teleportationProvider;

    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
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
        InputHelpers.IsPressed(leftInteractionRay.inputDevice, InputHelpers.Button.PrimaryButton, out bool openFireplace, activationThreshold);
        InputHelpers.IsPressed(leftInteractionRay.inputDevice, InputHelpers.Button.SecondaryButton, out bool openSecretDoor, activationThreshold);

        if (temporalSpoofA)
        {
            switch (i)
            {
                case 0:
                    TemporalController.instance.Network_Event("Key Pick Up");
                    break;
                case 1:
                    TemporalController.instance.Network_Event("Wall Smash");
                    break;
                case 2:
                    TemporalController.instance.Network_Event("Eyeball Send");
                    break;
                case 3:
                    TemporalController.instance.Network_Event("Past Escape");
                    break;
                case 4:
                    TemporalController.instance.Network_Event("Desk Drawer Open");
                    break;
                default:
                    break;
            }

            i++;
            temporalSpoofA = false;
        }

        if(openFireplace)
        {
            MessageObject new_message = new MessageObject("Book Lock");
            new_message.Add_Message_Tag("Status", "Unlock");
            new_message.Close_Tags();
            new_message.Date_Time = DateTime.Now.ToString();
            mailbox.Send_To_Sequence(new_message);

            new_message = new MessageObject("Laptop Lock");
            new_message.Add_Message_Tag("Status", "Unlock");
            new_message.Close_Tags();
            new_message.Date_Time = DateTime.Now.ToString();
            mailbox.Send_To_Sequence(new_message);

            openFireplace = false;
        }

        if(openSecretDoor)
        {
            MessageObject new_message = new MessageObject("Strange Lock");
            new_message.Add_Message_Tag("Status", "Unlock");
            new_message.Close_Tags();
            new_message.Date_Time = DateTime.Now.ToString();
            mailbox.Send_To_Sequence(new_message);

            new_message = new MessageObject("Retinal Scanner");
            new_message.Add_Message_Tag("Grab Object", "Eyeball In A Jar");
            new_message.Close_Tags();
            new_message.Date_Time = DateTime.Now.ToString();
            mailbox.Send_To_Sequence(new_message);

            new_message = new MessageObject("Retinal Scanner");
            new_message.Add_Message_Tag("Grab Object", "Retinal Scanner ID Card");
            new_message.Close_Tags();
            new_message.Date_Time = DateTime.Now.ToString();
            mailbox.Send_To_Sequence(new_message);

            openSecretDoor = false;
        }
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
