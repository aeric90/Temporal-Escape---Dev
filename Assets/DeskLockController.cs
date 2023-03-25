using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeskLockController : MonoBehaviour
{
    MailboxController mailbox = null;

    public Canvas lock_canvas;

    public List<GameObject> deskLockButtons = new List<GameObject>();
    public List<Material> deskLockButtonOffMats = new List<Material>();
    public List<Material> deskLockButtonOnMats = new List<Material>();

    public GameObject lockOpenButton;
    public Material lockOpenMat;

    private List<int> codeEntered = new List<int>();
    public List<int> unlockCode = new List<int>();

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

    public void pushLockButton(int i)
    {
        if(codeEntered.Contains(i))
        {
            codeEntered.Remove(i);
            deskLockButtons[i].GetComponent<MeshRenderer>().material = deskLockButtonOffMats[i];
        } else
        {
            codeEntered.Add(i);
            deskLockButtons[i].GetComponent<MeshRenderer>().material = deskLockButtonOnMats[i];
        }
    }

    public void pushUnlockButton()
    {
        if(checkLock())
        {
            lockOpenButton.GetComponent<MeshRenderer>().material = lockOpenMat;
            MessageObject new_message = new MessageObject(this.name);
            new_message.Add_Message_Tag("Status", "Unlock");
            new_message.Close_Tags();
            new_message.Date_Time = DateTime.Now.ToString();
            mailbox.Send_To_Sequence(new_message);
        }
    }

    private bool checkLock()
    {
        bool checkLock = true;

        foreach (int i in unlockCode)
        {
            if (!codeEntered.Contains(i)) checkLock = false;
        }

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
