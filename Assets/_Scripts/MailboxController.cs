using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;

public class MailboxController : MonoBehaviour
{
    private List<MessageObject> mailbox; // Holds the list of incomming messages

    // CONSTRUCTOR
    public MailboxController()
    {
        mailbox = new List<MessageObject>(); // Initialize the mailbox
    }

    // This function sends a message object to all object that have a mailbox.
    public void Send_To_Recipient(MessageObject outgoing_message, string recipient_name)
    {
        GameObject recipient = GameObject.Find(recipient_name);

        if (recipient != null)
        {
            MailboxController mailbox = recipient.GetComponent<MailboxController>();

            if(mailbox != null)
            {
                mailbox.Deliver_Message(outgoing_message); // Add the message to the mailbox
            } else
            {
                Debug.Log(recipient_name + " has no mailbox");
            }
        } 
    }

    public void Send_To_Sequence(MessageObject outgoing_message)
    {
        SequenceManager.instance.GetMailbox().Deliver_Message(outgoing_message); // Add the message to the mailbox
    }

    // Recieve an incomming message and add it to the mailbox
    public void Deliver_Message(MessageObject incomming_message)
    {
        mailbox.Add(incomming_message);
    }

    // Get the next message in the mailbox, if any
    public MessageObject Get_Message()
    {
        // For every message in the mailbox
        foreach (MessageObject message in mailbox)
        {
            return message;
        }

        return null; // Otherwise return null
    }

    // This function removes a specified message from the mailbox
    public void Remove_Message(MessageObject message)
    {
        if (message != null) mailbox.Remove(message);
    }

    // This function clears the mailbox of all messages
    public void Clear_Mailbox()
    {
        mailbox.Clear();
    }

    // This function creates and returns a new message object of the type requested
    public MessageObject New_Message(int type_id)
    {
        MessageObject new_message = new MessageObject(this.gameObject.name);

        return new_message;
    }
}
