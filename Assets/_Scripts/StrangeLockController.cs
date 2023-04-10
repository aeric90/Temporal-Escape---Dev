using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StrangeLockController : MonoBehaviour
{
    MailboxController mailbox = null;  // Holds the current object's mailbox object

    public List<XRSocketInteractor> socketList = new List<XRSocketInteractor>();
    public string[] lockSolution = new string[3];
    private bool lockSolved = false;

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
    }

    // Update is called once per frame
    void Update()
    {
        bool lockStatus = true;

        if (!lockSolved)
        {
            for (int i = 0; i < socketList.Count; i++)
            {
                IXRSelectInteractable objName = socketList[i].GetOldestInteractableSelected();


                if (objName != null)
                {
                    if (objName.transform.name != lockSolution[i]) lockStatus = false;
                }
                else
                {
                    lockStatus = false;
                }
            }

            if (lockStatus)
            {
                StartCoroutine(solveProcess());
            }
        }
    }

    IEnumerator solveProcess()
    {
        yield return new WaitForSeconds(0.1f);
        MessageObject new_message = new MessageObject(this.name);
        new_message.Add_Message_Tag("Status", "Unlock");
        new_message.Close_Tags();
        new_message.Date_Time = DateTime.Now.ToString();
        mailbox.Send_To_Sequence(new_message);
        lockSolved = true;
    }
}
