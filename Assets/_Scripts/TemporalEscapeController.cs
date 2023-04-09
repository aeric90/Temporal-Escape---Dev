using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class TemporalEscapeController : MonoBehaviour
{
    public static TemporalEscapeController instance;
    public GameObject active_room;
    public List<GameObject> rooms = new List<GameObject>();
    public MenuAudioController menuAudio;

    private string statusText = "Select an option";
    private int room_id = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null) instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetStatusText()
    {
        return statusText;
    }

    public void SwitchRoom(string room_name) 
    {
        switch(room_name) {
            case "Past Room":
                room_id = 1;
                break;
            case "Future Room":
                room_id = 2;
                break;
            default:
                room_id = 0;
                break;
        }
        
        StartCoroutine(WaitForPlayers());
    }

    public void CancelConnect()
    {
        NetworkController.instance.LeaveRoom();
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
                StartCoroutine(SwitchRoomRoutine(room_id));
                break;
            }
        }

        if(!NetworkController.instance.InRoom())
        {
            statusText = "Cancelling connection...";
            yield return new WaitForSeconds(1.0f);
        }

        statusText = "Select an option";
    }

    IEnumerator SwitchRoomRoutine(int room_id)
    {
        statusText = "Enjoy your escape!";
        FadeController.instance.SetFade(true);
        menuAudio.FadeAudio();
        yield return new WaitForSeconds(3);
        Destroy(active_room);
        yield return null;
        active_room = rooms[room_id];
        Instantiate(active_room);
        yield return null;
        FadeController.instance.SetFade(false);
    }
}
