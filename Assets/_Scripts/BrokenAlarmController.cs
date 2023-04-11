using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenAlarmController : MonoBehaviour
{
    public GameObject alarmText;
    public float flashingDelay = 1.0f;
    private float timeElapsed = 0.0f;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

    public void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Play();
        }
    }
}
