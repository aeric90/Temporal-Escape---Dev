using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomController : MonoBehaviour
{
    public static EndRoomController instance;

    public GameObject winObjects;
    public GameObject loseObjects;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetObjects(bool win)
    {
        winObjects.SetActive(win);
        loseObjects.SetActive(!win);
    }
}
