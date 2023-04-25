using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class TemporalController : MonoBehaviourPun
{
    public static TemporalController instance;
    MailboxController mailbox = null;  // Holds the current object's mailbox object

    public int player_1_room = 0;
    public int player_2_room = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox();
    }

    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            string messageTag = message.Get_Message_Tag("Temporal Action");

            if (messageTag != null)
            {
                Debug.Log(gameObject.name + " received message " + messageTag);
                photonView.RPC("Network_Event", RpcTarget.AllBuffered, messageTag);
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }

    [PunRPC]
    public void Network_Event(string name)
    {
        MessageObject new_message = new MessageObject(this.name);
        new_message.Add_Message_Tag("Temporal Event", name);
        new_message.Close_Tags();
        new_message.Date_Time = DateTime.Now.ToString();
        mailbox.Send_To_Sequence(new_message);
    }

    [PunRPC]
    public void SetPlayerRooms()
    {

    }

    public int GetPlayer1RoomID() 
    {
        Debug.Log("Setting Player 1 Room");
        if (player_1_room == 0) {
            System.Random rnd = new System.Random();

            player_1_room = rnd.Next(1, 3);
        }
        return player_1_room;
    }

    public int GetPlayer2RoomID()
    {
        Debug.Log("Setting Player 2 Room");
        if (player_1_room == 0) GetPlayer1RoomID();
        if (player_1_room == 1) player_2_room = 2;
        if (player_1_room == 2) player_2_room = 1;

        return player_2_room;
    }

    public void ClearRoomID(int roomID)
    {
        if(player_1_room == roomID) player_1_room = 0;
        if(player_2_room == roomID) player_2_room = 0;
    }
}
