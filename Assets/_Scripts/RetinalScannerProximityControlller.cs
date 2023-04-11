using HighlightPlus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetinalScannerProximityControlller : MonoBehaviour
{
    MailboxController mailbox = null;  // Holds the current object's mailbox object

    public Collider mainCollider;
    private bool trigger = false;

    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
        mainCollider = GetComponent<Collider>();
    }
    private void Update()
    {
        if (mailbox != null) CheckMailbox();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Main Camera" && !trigger)
        {
            MessageObject new_message = new MessageObject(this.name);
            new_message.Add_Message_Tag("Zone Enter", other.gameObject.name);
            new_message.Close_Tags();
            new_message.Date_Time = DateTime.Now.ToString();
            mailbox.Send_To_Sequence(new_message);
            trigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Main Camera") trigger = false;
    }

    private void DisableObject()
    {
        mainCollider.enabled = false;
    }

    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            string messageTag = message.Get_Message_Tag("Action");
            Debug.Log(gameObject.name + " received message " + messageTag);
            switch (messageTag)
            {
                case "Disable":
                    DisableObject();
                    break;
                default:
                    break;
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }
}
