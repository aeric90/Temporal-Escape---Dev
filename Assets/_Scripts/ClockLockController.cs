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

    public int lock_hour_answer;
    public int lock_minute_answer;
    public int clock_hour = 12;
    public int clock_min = 0;
    private bool unlocked = false;

    private bool hands_moving = false;
    public GameObject minuteHand;
    public GameObject hourHand;
    private Vector3 minuteRotation = new Vector3(0.0f, 0.0f, 90.0f);
    private Vector3 hourRotation = new Vector3(0.0f, 0.0f, 7.5f);


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

    private bool CheckLock()
    {
        bool checkLock = false;
        if (lock_hour_answer == clock_hour && lock_minute_answer == clock_min) checkLock = true;

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

    public void Increment_Time()
    {
        clock_min += 15;
        if (clock_min >= 60)
        {
            clock_hour++;
            clock_min = 0;
        }
        if (clock_hour >= 13)
        {
            clock_hour = 1;
        }

        StartCoroutine(rotateHands());
    }

    IEnumerator rotateHands()
    {
        if(hands_moving)
        {
            yield break;
        }
        hands_moving = true;

        Vector3 currentMinRot = minuteHand.transform.eulerAngles;
        Vector3 newMinRot = minuteHand.transform.eulerAngles + minuteRotation;

        Vector3 currentHourRot = hourHand.transform.eulerAngles;
        Vector3 newHourRot = hourHand.transform.eulerAngles + hourRotation;

        float timer = 0.0f;

        while(timer < 0.1f)
        {
            timer += Time.deltaTime;
            minuteHand.transform.eulerAngles = Vector3.Lerp(currentMinRot, newMinRot, timer / 0.1f);
            hourHand.transform.eulerAngles = Vector3.Lerp(currentHourRot, newHourRot, timer / 0.1f);
            yield return null;
        }
        hands_moving = false;

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

}
