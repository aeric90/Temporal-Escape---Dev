using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;

public class FixedInteractableController : MonoBehaviour
{

    MailboxController mailbox = null;  // Holds the current object's mailbox object

    public Vector3 original_origin;
    public Transform spawn_position;

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
        original_origin = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox();
    }

    private void SpawnObject()
    {
        this.gameObject.transform.position = spawn_position.transform.position;
        this.gameObject.transform.rotation = spawn_position.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GrabInteractable")
        {
            MessageObject new_message = new MessageObject(this.name);
            new_message.Add_Message_Tag("Grab Object", other.gameObject.name);
            new_message.Close_Tags();
            new_message.Date_Time = DateTime.Now.ToString();
            mailbox.Send_To_Sequence(new_message);
        }
    }

    private void RemoveObject()
    {
        Destroy(this.gameObject);
    }

    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            switch(message.Get_Message_Tag("Action"))
                {
                case "Spawn":
                    SpawnObject();
                    break;
                case "Remove":
                    RemoveObject();
                    break;
                default:
                    break;
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }
}
