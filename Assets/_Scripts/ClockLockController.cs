using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;

public class ClockLockController : MonoBehaviour
{
    MailboxController mailbox = null;

    public Canvas lock_canvas;
    public TMPro.TextMeshProUGUI lock_button_text;
    public string lock_answer;
    private bool unlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox();
        if (!unlocked && CheckLock())
        {
            unlocked = true;
            MessageObject new_message = new MessageObject(this.name);
            new_message.Add_Message_Tag("Status", "Unlock");
            new_message.Close_Tags();
            new_message.Date_Time = DateTime.Now.ToString();
            mailbox.Send_To_Sequence(new_message);
        }
    }

    private bool CheckLock()
    {
        bool checkLock = false;

        if (lock_button_text.text == lock_answer) checkLock = true;

        return checkLock;
    }

    private void EnableLock()
    {
        lock_canvas.gameObject.SetActive(true);
    }

    private void DisableLock()
    {
        lock_canvas.gameObject.SetActive(false);
    }

    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            switch (message.Get_Message_Tag("Action"))
            {
                case "Enable":
                    EnableLock();
                    break;
                case "Disable":
                    DisableLock();
                    break;
                default:
                    break;
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }
}
