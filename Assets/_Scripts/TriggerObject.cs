using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

[XmlRoot("TriggerObject")]
public class TriggerObject
{
    // ATTRIBUTES
    private MessageTrigger message_in; // The incomming message for the trigger
    private List<MessageTrigger> messages_out = new List<MessageTrigger>(); // The list of messages that will get sent out for this trigger
    private List<string> tags_required = new List<string>();
    private List<string> tags_excluded = new List<string>();
    private List<string> tags_on = new List<string>(); // The list of status tags that are turned on for this trigger
    private List<string> tags_off = new List<string>(); // The list of status tags that are turned off for this trigger

    // CONSTRUCTORS
    // XML serialization requires an empty constructor
    public TriggerObject() { }

    // GET / SET STATEMENTS
    public MessageTrigger Message_In
    {
        get { return message_in; }
        set { message_in = value; }
    }
    public List<MessageTrigger> Messages_Out
    {
        get { return messages_out; }
        set { messages_out = value; }
    }
    public List<string> Tags_Required
    {
        get { return tags_required; }
        set { tags_required = value; }
    }
    public List<string> Tags_Excluded
    {
        get { return tags_excluded; }
        set { tags_excluded = value; }
    }
    public List<string> Tags_On
    {
        get { return tags_on; }
        set { tags_on = value; }
    }
    public List<string> Tags_Off
    {
        get { return tags_off; }
        set { tags_off = value; }
    }
}

// This defines the message trigger
[XmlRoot("MessageTrigger")]
public class MessageTrigger
{
    // ATTRIBUTES
    private string message_mailbox;
    private List<MessageTag> message_tags = new List<MessageTag>(); // The list of tags for the trigger

    // CONSTRUCTORS
    // XML serialization requires an empty constructor
    public MessageTrigger() { }
    public MessageTrigger(string message_sender)
    {
        Message_Mailbox = message_sender;
    }

    public string Message_Mailbox
    {
        get { return message_mailbox; }
        set { message_mailbox = value; }
    }
    public List<MessageTag> Message_Tags
    {
        get { return message_tags; }
        set { message_tags = value; }
    }
}


// This defines the message tag object for the message trigger
[XmlRoot("MessageTag")]
public class MessageTag
{
    // ATTRIBUTES
    private string message_tag_name; // The name of the tag
    private string message_tag_content; // The message time content

    // CONSTRUCTORS
    // XML serialization requires an empty constructor
    public MessageTag() { }
    public MessageTag(string message_tag_name, string message_tag_content)
    {
        Message_Tag_Name = message_tag_name;
        Message_Tag_Content = message_tag_content;
    }

    // GET / SET STATEMETNS
    public string Message_Tag_Name
    {
        get { return message_tag_name; }
        set { message_tag_name = value; }
    }
    public string Message_Tag_Content
    {
        get { return message_tag_content; }
        set { message_tag_content = value; }
    }
}
