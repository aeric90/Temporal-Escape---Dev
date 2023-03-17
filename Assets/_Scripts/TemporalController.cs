using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using Photon.Pun;

public class TemporalController : MonoBehaviourPun
{
    public static TemporalController instance;
    MailboxController mailbox = null;  // Holds the current object's mailbox object

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox();
    }

    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            string messageTag = message.Get_Message_Tag("Temporal Action");
            Debug.Log(gameObject.name + " received message " + messageTag);
            switch (messageTag)
            {
                case "Wall Smash":
                    Network_Event(0);
                    break;
                case "Eyeball Send":
                    Network_Event(1);
                    break;
                default:
                    break;
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }

    [PunRPC]
    public void Network_Event(int code)
    {
        MessageObject new_message = new MessageObject(this.name);
        switch (code)
        {
            case 0:
                new_message.Add_Message_Tag("Temporal Event", "Wall Smash");
                break;
            case 1:
                new_message.Add_Message_Tag("Temporal Event", "Eyeball Send");
                break;
        }
        new_message.Close_Tags();
        new_message.Date_Time = DateTime.Now.ToString();
        mailbox.Send_To_Sequence(new_message);
    }
}
