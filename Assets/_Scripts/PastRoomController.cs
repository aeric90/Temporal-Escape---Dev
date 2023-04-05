using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PastRoomController : MonoBehaviour
{
    MailboxController mailbox = null;  // Holds the current object's mailbox object
    public GameObject teleport_area_2;

    public GameObject regularWall;
    public GameObject holeWall;


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

    void ExpandRoom()
    {
        teleport_area_2.SetActive(true);
    }

    void SwapWalls()
    {
        regularWall.SetActive(false);
        holeWall.SetActive(true);
    }

    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            switch (message.Get_Message_Tag("Action"))
            {
                case "Expand":
                    ExpandRoom();
                    break;
                case "Wall":
                    SwapWalls();
                    break;
                default:
                    break;
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }
}
