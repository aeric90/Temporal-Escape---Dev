using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDoorController : MonoBehaviour
{
    MailboxController mailbox = null;  // Holds the current object's mailbox object
    Animator animator = null;
    bool animation_trigger = false;

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox();
    }


    private void AnimateObject()
    {
        Debug.Log("Recieve Animate Command " + gameObject.name);
        animation_trigger = !animation_trigger;
        animator.SetBool("trigger", animation_trigger);
    }

    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            switch (message.Get_Message_Tag("Action"))
            {
                case "Animate":
                    AnimateObject();
                    break;
                default:
                    break;
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }
}
