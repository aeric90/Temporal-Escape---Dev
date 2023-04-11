using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRoomController : MonoBehaviour
{
    public static MenuRoomController instance;

    public GameObject pastMenu;
    public GameObject futureMenu;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPastMenu()
    {
        Debug.Log("Setting Past Menu");
        pastMenu.SetActive(true);
        futureMenu.SetActive(false);
    }

    public void SetFutureMenu()
    {
        pastMenu.SetActive(false);
        futureMenu.SetActive(true);
    }
}
