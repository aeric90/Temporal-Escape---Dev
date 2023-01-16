using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInteractableController : MonoBehaviour
{
    MailboxController mailbox = null;  // Holds the current object's mailbox object

    public Transform spawn_position;
    public Collider object_collider;
    public Rigidbody object_body;

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox();
    }

    private void SpawnObject()
    {
        this.gameObject.transform.position = spawn_position.transform.position;
        object_collider.enabled = true;
        object_body.useGravity = true;
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
            switch (message.Get_Message_Tag("Action"))
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

    public void OnGrab()
    {
        object_collider.isTrigger = true;
    }
    public void OnDrop()
    {
        object_collider.isTrigger = false;
    }

}
