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
    public List<Material> deskLockButtonWrongMats = new List<Material>();

    public GameObject lockOpenButton;
    public Material lockOffmat;
    public Material lockOpenMat;
    public Material lockWrongMat;

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
        if(!codeEntered.Contains(i))
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
        } else
        {
            StartCoroutine(CodeWrong());
        }
    }

    private bool checkLock()
    {
        bool checkLock = true;

        if (codeEntered.Count == 4)
        {
            for (int i = 0; i < unlockCode.Count; i++)
            {
                if (unlockCode[i] != codeEntered[i]) checkLock = false;
            }
        } else
        {
            checkLock = false;
        }

        return checkLock;
    }

    private void EnableLock()
    {
        lock_canvas.enabled = true;
    }

    private void DisableLock()
    {
        lock_canvas.enabled= false;
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

    private void ResetLock()
    {
        for (int buttonID = 0; buttonID < deskLockButtons.Count; buttonID++)
        {
            deskLockButtons[buttonID].GetComponent<MeshRenderer>().material = deskLockButtonOffMats[buttonID];
        }
        lockOpenButton.GetComponent<MeshRenderer>().material = lockOffmat;
        EnableLock();
        codeEntered.Clear();
    }

    IEnumerator CodeWrong()
    {
        DisableLock();

        bool flash = true;

        for (int flashCount = 0; flashCount <= 5; flashCount++) {
            for(int buttonID = 0; buttonID < deskLockButtons.Count; buttonID++)
            {
                if(flash)
                {
                    deskLockButtons[buttonID].GetComponent<MeshRenderer>().material = deskLockButtonWrongMats[buttonID];
                } else
                {
                    deskLockButtons[buttonID].GetComponent<MeshRenderer>().material = deskLockButtonOffMats[buttonID];
                }
            }

            if (flash)
            {
                lockOpenButton.GetComponent<MeshRenderer>().material = lockWrongMat;
            }
            else
            {
                lockOpenButton.GetComponent<MeshRenderer>().material = lockOffmat;
            }

            yield return new WaitForSeconds(0.2f);
            flash = !flash;
        }

        ResetLock();
    }
}
