using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioController : MonoBehaviour
{
    private AudioSource audioSource;
    private float startVolume;
    private float fadeTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startVolume = audioSource.volume;
    }

    public void FadeAudio()
    {
        StartCoroutine(AudioFadeOut());
    }

    IEnumerator AudioFadeOut() {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
