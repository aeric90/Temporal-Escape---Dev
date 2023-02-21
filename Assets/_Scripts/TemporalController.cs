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

    }

    public void TemporalSpoof(int code)
    {
        MessageObject new_message = new MessageObject(this.name);
        switch(code)
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
