using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

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

            if (messageTag != null)
            {
                Debug.Log(gameObject.name + " received message " + messageTag);
                photonView.RPC("Network_Event", RpcTarget.AllBuffered, messageTag);
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }

    [PunRPC]
    public void Network_Event(string name)
    {
        MessageObject new_message = new MessageObject(this.name);
        new_message.Add_Message_Tag("Temporal Event", name);
        new_message.Close_Tags();
        new_message.Date_Time = DateTime.Now.ToString();
        mailbox.Send_To_Sequence(new_message);
    }
}
