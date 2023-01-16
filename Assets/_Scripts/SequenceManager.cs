using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;


// This is the root container object of the sequence trigger object model
[XmlRoot("SequenceContainer")]
public class SequenceContainer
{
    // ATTRIBUTES
    public List<TriggerObject> trigger_objects = new List<TriggerObject>(); // List of trigger objects

    // CONSTRUCTORS
    // XML serialization requires an empty constructor
    public SequenceContainer() { }
}

public class SequenceManager : MonoBehaviour
{
    // Public static instance of the sequence_controller allows other scripts to access the sequence_controller as needed
    public static SequenceManager instance;

    MailboxController mailbox = null;  // Holds the current object's mailbox object

    public string sequence_file; // The XML file containing the sequence information

    private SequenceContainer sequence_container = new SequenceContainer(); // The container for the sequence's trigger objects
    private List<string> task_flags = new List<string>(); // The list of flags that are currently active for the sequence

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this; // Initialize static object instance.
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
        Import_Sequence(sequence_file);
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox(); // Check the mailbox if it exists.
    }

    // This function checks the current object's mailbox for messages and processes them
    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            // Otherwise check the incomming message for any triggers that may be relevant
            foreach (TriggerObject trigger in sequence_container.trigger_objects)
            {
                Check_Triggers(trigger, message);
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }

    // This function checks the triggers for the information passed and sends the relevant messages if
    // the triggers match
    public void Check_Triggers(TriggerObject trigger, MessageObject message_in)
    {
        if (trigger.Message_In.Message_Mailbox == message_in.Sender)
        {
            Add_Flags(trigger.Tags_On); // Add the trigger's on flags
            Remove_Flags(trigger.Tags_Off); // Remove the trigger's off flags

            if (Check_Required_Flags(trigger.Tags_Required) && Check_Excluded_Flags(trigger.Tags_Excluded))
            {
                foreach (MessageTrigger message in trigger.Messages_Out)
                {
                    MessageObject new_message = new MessageObject(this.name);
                    foreach (MessageTag tag in message.Message_Tags)
                    {
                        new_message.Add_Message_Tag(tag.Message_Tag_Name, tag.Message_Tag_Content);
                    }
                    new_message.Close_Tags();
                    new_message.Date_Time = DateTime.Now.ToString();
                    mailbox.Send_To_Recipient(new_message, message.Message_Mailbox);
                }
            }
        }
    }

    // This function adds the list of flags to the flag list
    public void Add_Flags(List<string> in_flags)
    {
        foreach (string flag in in_flags)
        {
            task_flags.Add(flag);
        }
    }

    // This function removes the list of flags to the flag list
    public void Remove_Flags(List<string> in_flags)
    {
        foreach (string flag in in_flags)
        {
            task_flags.Remove(flag);
        }
    }

    public bool Check_Required_Flags(List<string> flags_in)
    {
        bool requiredFlags = true;

        foreach (string flag_in in flags_in)
        {
            if (!Check_Flag(flag_in)) requiredFlags = false;
        }

        return requiredFlags;
    }

    public bool Check_Excluded_Flags(List<string> flags_in)
    {
        bool excludedFlags = true;

        foreach (string flag_in in flags_in)
        {
            if (Check_Flag(flag_in)) excludedFlags = false;
        }

        return excludedFlags;
    }

    // This function searches for a provided flag and returns true if found
    public bool Check_Flag(string in_flag)
    {
        bool flagFound = false;

        foreach (string flag in task_flags)
        {
            if (flag == in_flag) flagFound = true;
        }

        return flagFound;
    }

    // This function clears the sequence container and all the current flags
    public void Clear_Sequence()
    {
        sequence_container.trigger_objects.Clear();
        task_flags.Clear();
    }

    // This function imports and deserializes the sequence file into the sequence container
    public void Import_Sequence(string file_name)
    {
        sequence_file = file_name;
        XmlSerializer serializer = new XmlSerializer(sequence_container.GetType());
        var myFileStream = new FileStream(Application.persistentDataPath + "/" + file_name, FileMode.Open);
        sequence_container = serializer.Deserialize(myFileStream) as SequenceContainer;
        myFileStream.Close();
    }

    public MailboxController GetMailbox()
    {
        return mailbox;
    }
}
