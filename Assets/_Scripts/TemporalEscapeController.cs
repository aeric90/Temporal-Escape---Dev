using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum GAME_TYPE
{
    FUTURE,
    PAST
}

public class TemporalEscapeController : MonoBehaviour
{
    MailboxController mailbox = null;  // Holds the current object's mailbox object
    public static TemporalEscapeController instance;
    public GameObject active_room;
    public List<GameObject> rooms = new List<GameObject>();
    public GameObject xrOrigin;
    public GAME_TYPE startingRoom = GAME_TYPE.PAST;

    private string statusText = "Select an option";
    private int room_id = 0;

    public bool testing = false;

    // Start is called before the first frame update
    void Start()
    {
        mailbox = this.GetComponent<MailboxController>(); // Initialize the mailbox object.
        if (instance == null) instance = this;
        active_room = Instantiate(rooms[0]);
        // Changing setup so that there is only one menu room. The program will decide which 
        // room to chose between the two, instead of the build.
        /*
        switch (startingRoom)
        {
            case GAME_TYPE.PAST:
                MenuRoomController.instance.SetPastMenu();
                break;
            case GAME_TYPE.FUTURE:
                MenuRoomController.instance.SetFutureMenu();
                break;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (mailbox != null) CheckMailbox();
    }

    public string GetStatusText()
    {
        return statusText;
    }

    public void SwitchRoom(string room_name) 
    {
        switch(room_name) {
            case "Menu Room":
                room_id = 0;
                break;
            case "Past Room":
                room_id = 1;
                break;
            case "Future Room":
                room_id = 2;
                break;
            case "End Game Room":
                room_id = 3;
                break;
            default:
                room_id = 0;
                break;
        }
        
        if(!testing) StartCoroutine(WaitForPlayers());
        if(testing) StartCoroutine(SwitchRoomRoutine(room_id));
    }

    public void SwitchRoom()
    {
        if (!testing) StartCoroutine(WaitForPlayers());
        if (testing) StartCoroutine(SwitchRoomRoutine(room_id));
    }

    public void CancelConnect()
    {
        NetworkController.instance.LeaveRoom();
    }

    public void GameOverProcess(bool win)
    {
        StartCoroutine(SwitchGameOverRoomRoutine(win));
    }

    IEnumerator WaitForPlayers()
    {
        int loopCount = 0;

        do
        {
            NetworkController.instance.JoinRoom();
            statusText = "Joining room";
            for (int i = 0; i < loopCount; i++)
            {
                statusText += ".";
            }

            loopCount++;
            if (loopCount > 3) loopCount = 0;

            yield return new WaitForSeconds(1.0f);
        } while (!NetworkController.instance.InRoom());

        while (NetworkController.instance.InRoom())
        {
            if (NetworkController.instance.GetPlayerCount() != 2)
            {
                if (room_id == 0) room_id = 1;
                statusText = "Waiting for player 2";
                for (int i = 0; i < loopCount; i++)
                {
                    statusText += ".";
                }

                loopCount++;
                if (loopCount > 3) loopCount = 0;
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                if (room_id == 0) room_id = 2;
                statusText = "Enjoy your escape!";
                StartCoroutine(SwitchRoomRoutine(room_id));
                break;
            }
        }
        yield return new WaitForSeconds(1.0f);

        if (!NetworkController.instance.InRoom())
        {
            statusText = "Cancelling connection...";
            room_id = 0;
            yield return new WaitForSeconds(1.0f);
            statusText = "Select an option";
        }
    }

    IEnumerator SwitchRoomRoutine(int room_id)
    {
        MenuAudioController.instance.FadeAudio();
        FadeController.instance.SetFade(true);
        yield return new WaitForSeconds(3);
        Destroy(active_room);
        yield return null;
        xrOrigin.transform.position = new Vector3(0.0f, xrOrigin.transform.position.y, 0.0f);
        active_room = Instantiate(rooms[room_id]);
        switch(room_id)
        {
            case 0:
                /*
                switch (startingRoom)
                {
                    case GAME_TYPE.PAST:
                        MenuRoomController.instance.SetPastMenu();
                        break;
                    case GAME_TYPE.FUTURE:
                        MenuRoomController.instance.SetFutureMenu();
                        break;
                }
                */
                break;
            case 1:
                active_room.gameObject.name = "Past Room";
                break;
            case 2:
                active_room.gameObject.name = "Future Room";
                break;
        }

        yield return null;
        FadeController.instance.SetFade(false);
    }

    IEnumerator SwitchGameOverRoomRoutine(bool win)
    {
        FadeController.instance.SetFade(true);
        yield return new WaitForSeconds(3);
        Destroy(active_room);
        yield return null;
        xrOrigin.transform.position = new Vector3(0.0f, xrOrigin.transform.position.y, 0.0f);
        active_room = Instantiate(rooms[3]);
        EndRoomController.instance.SetObjects(win);
        yield return null;
        FadeController.instance.SetFade(false);
    }

    private void CheckMailbox()
    {
        MessageObject message = mailbox.Get_Message(); // Get the first message, if any

        // Do until there are no messages in the mailbox
        while (message != null)
        {
            Debug.Log("Recieved Message " + gameObject.name);
            switch (message.Get_Message_Tag("Action"))
            {
                case "Game Over":
                    GameOverProcess(true);
                    break;
                default:
                    break;
            }

            mailbox.Remove_Message(message); // Remove the processed message
            message = mailbox.Get_Message(); // Get the next message, if any
        }
    }
}
