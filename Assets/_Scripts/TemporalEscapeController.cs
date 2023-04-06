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

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null) instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchRoom(string room_name) 
    {
        int room_id = 0;

        switch(room_name) {
            case "Past Room":
                //UIInputControl.instance.SetControllersToGame();
                room_id = 1;
                break;
            case "Future Room":
                //UIInputControl.instance.SetControllersToGame();
                room_id = 2;
                break;
            default:
                UIInputControl.instance.SetControllersToUI();
                room_id = 0;
                break;
        }
        
        StartCoroutine(SwitchRoomRoutine(room_id));
    }

    IEnumerator SwitchRoomRoutine(int room_id)
    {
        FadeController.instance.SetFade(true);
        menuAudio.FadeAudio();
        yield return new WaitForSeconds(3);
        active_room.SetActive(false);
        active_room = rooms[room_id];
        active_room.SetActive(true);
        FadeController.instance.SetFade(false);
    }
}
