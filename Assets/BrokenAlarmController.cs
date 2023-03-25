using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenAlarmController : MonoBehaviour
{
    public GameObject alarmText;
    public float flashingDelay = 5.0f;
    private float timeElapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= flashingDelay)
        {
            alarmText.SetActive(!alarmText.activeSelf);
            timeElapsed= 0.0f;
        }
    }
}
