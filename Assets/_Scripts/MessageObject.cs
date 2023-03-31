using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

[Serializable]
[XmlRoot("message_object")]
public class MessageObject
{
    // ATTRIBUTES
    private string message_date_time; // This is the message date and time stamp
    private string message_sender; // Stores the name of the sender game object
    private string message_tags; // Stores a string of message tags in JSON format
    private bool message_process_flag; // A boolean flag which indicates if the message has been processed

    // CONSTRUCTORS
    // XML serialization requires an empty constructor
    public MessageObject() { }
    // Main constructor which accepts a type id and a sender name
    public MessageObject(string sender)
    {
        Sender = sender;
        Tags = ""; // Make sure the tag field is empty
    }

    // GET / SET Statements
    public string Date_Time
    {
        get { return message_date_time; }
        set { message_date_time = value; }
    }
    public string Sender
    {
        get { return message_sender; }
        set { message_sender = value; }
    }
    public string Tags
    {
        get { return message_tags; }
        set { message_tags = value; }
    }
    public bool Process_Flag
    {
        get { return message_process_flag; }
        set { message_process_flag = value; }
    }

    // METHODS

    // This function adds a string tag to the tag list
    public void Add_Message_Tag(string tag_name, string tag_content)
    {
        if (Tags == "")
        {
            Tags += "{'" + tag_name + "':'" + tag_content + "'";
        }
        else
        {
            Tags += ", '" + tag_name + "':'" + tag_content + "'";
        }
    }

    // This function adds an integer tag to the tag list
    public void Add_Message_Tag(string tag_name, int tag_content)
    {
        if (Tags == "")
        {
            Tags += "{'" + tag_name + "':" + tag_content + "";
        }
        else
        {
            Tags += ", '" + tag_name + "':" + tag_content + "";
        }
    }

    // This function sets the tags to a particular predefinded JSON string
    public void Set_Message_Tags(string tags)
    {
        Tags = tags;
    }

    // This function adds the final terminator bracket to the tags if it does not exist
    public void Close_Tags()
    {
        if (Tags != "" && Tags.Substring(Tags.Length - 1) != "}") Tags += "}";
    }

    // This function parses the JSON tag list for a particular tag name and returns the value associated
    public string Get_Message_Tag(string tag_name)
    {
        string tag = "";
        JObject test = JObject.Parse(Tags);
        if(test.ContainsKey(tag_name)) tag = test[tag_name].ToString();

        return tag;
    }
}
