using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;

public class TemporalController : MonoBehaviour
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

    public void TemporalSpoof()
    {
        MessageObject new_message = new MessageObject(this.name);
        new_message.Add_Message_Tag("Temporal Event", "Wall Smash");
        new_message.Close_Tags();
        new_message.Date_Time = DateTime.Now.ToString();
        mailbox.Send_To_Sequence(new_message);
    }
}
