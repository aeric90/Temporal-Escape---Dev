using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Timer_Object
{
    // ATTRIBUTES
    private string timer_name; // The timer's name
    private float timer_duration; // The timer's specified duration
    private float timer_time; // The time of the current timer

    // CONSTRUCTORS
    public Timer_Object(string name, float duration)
    {
        timer_name = name;
        timer_duration = duration;
        timer_time = 0.0f;
    }

    // GET / SET STATEMETNS
    public string Name
    {
        get { return timer_name; }
        set { timer_name = value; }
    }

    // METHODS

    // This function updates the timer's current time
    public void Update_Timer()
    {
        timer_time += Time.deltaTime;
    }

    // This function returns the difference between the current time of the timer and it's duration
    public float Get_Timer_Difference()
    {
        return timer_duration - timer_time;
    }
}

public class TimerController : MonoBehaviour
{
    private MailboxController mailbox = null; // Holds the current object's mailbox object
    public List<Timer_Object> timer_objects = new List<Timer_Object>(); // List of timer objects

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox(); // Check the mailbox if it exists.
        Check_Timers(); // Check all update all the timers
    }

    // This function checks all the timer's in the timer list and deletes them
    // when they're done
    private void Check_Timers()
    {
        List<Timer_Object> delete_timers = new List<Timer_Object>(); // Running list of timers to be deleted

        foreach (Timer_Object timer in timer_objects)
        {
            timer.Update_Timer();

            if (timer.Get_Timer_Difference() <= 0.0f)
            {
                End_Timer(timer); // Run the end timer function
                delete_timers.Add(timer); // Add the timer to the deleted list
            }
        }

        // Delete all the timers on the delete timers list
        foreach (Timer_Object timer in delete_timers)
        {
            timer_objects.Remove(timer);
        }

        delete_timers.Clear(); // Clear the deleted timers list
    }

    // This function adds a new timer to the timer list with a name and duration specified
    private void Add_Timer(string timer_name, float timer_duration)
    {
        timer_objects.Add(new Timer_Object(timer_name, timer_duration));
    }

    // This function checks the current object's mailbox for messages and processes them
    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            switch (message.Get_Message_Tag("Action"))
            {
                case "Timer Start":
                    Add_Timer(message.Get_Message_Tag("Name"), float.Parse(message.Get_Message_Tag("Duration")));
                    break;
                default:
                    break;
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }

    // This function sends a message when a timer is over
    private void End_Timer(Timer_Object timer)
    {
        MessageObject new_message = new MessageObject(this.name);
        new_message.Add_Message_Tag("Timer End", timer.Name);
        new_message.Close_Tags();
        new_message.Date_Time = DateTime.Now.ToString();
        Debug.Log("Sending message to Sequence Manager");
        mailbox.Send_To_Recipient(new_message, "Sequence Manager");
    }
}
