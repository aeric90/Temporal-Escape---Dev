using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;

public class FixedInteractableController : MonoBehaviour
{

    MailboxController mailbox = null;  // Holds the current object's mailbox object
    
    Animator animator = null;
    bool animation_trigger = false;

    public GameObject attach_object;

    public Vector3 original_origin;
    public Transform spawn_position;

    public List<Outline> outlines = new List<Outline>();
    public Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
        animator = this.GetComponent<Animator>();
        original_origin = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox();
    }

    private void SpawnObject()
    {
        Debug.Log(gameObject.name + " spawning");
        this.gameObject.transform.position = spawn_position.transform.position;
        this.gameObject.transform.rotation = spawn_position.transform.rotation;
    }

    private void RemoveObject()
    {
        Destroy(this.gameObject);
    }

    private void AnimateObject()
    {
        animation_trigger = !animation_trigger;
        animator.SetBool("trigger", animation_trigger);
    }

    private void AttachObject()
    {
        attach_object.SetActive(true);
    }

    private void DisableObject()
    {
        foreach(Outline outline in outlines) { 
            outline.enabled = false;
        }
        collider.enabled = false;
    }

    private void AppearObject()
    {
        this.gameObject.SetActive(true);
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
                case "Animate":
                    AnimateObject();
                    break;
                case "Attach":
                    AttachObject();
                    break;
                case "Disable":
                    DisableObject();
                    break;
                case "Appear":
                    AppearObject();
                    break;
                default:
                    break;
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }
}
