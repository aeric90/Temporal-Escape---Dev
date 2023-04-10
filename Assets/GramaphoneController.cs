using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GramaphoneController : MonoBehaviour
{
    // Start is called before the first frame update
    
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMusic()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        } else
        {
            audioSource.Play();
        }
    }

}
