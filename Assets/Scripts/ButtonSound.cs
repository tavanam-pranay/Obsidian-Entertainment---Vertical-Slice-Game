using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip bgm;
    public AudioClip clickSound;
    public AudioClip hoverSound;
    public AudioClip cancelSound;
    public AudioClip upgradeSound;
    void Start()
    {
        audioSource.clip = bgm;
        audioSource.loop = true;
        audioSource.Play();

    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void PlayHover()
    {
        audioSource.PlayOneShot(hoverSound);
    }
    public void PauseBGM()
    {
        audioSource.Pause();
    }
    public void PlayCancel()
    {
        audioSource.PlayOneShot(cancelSound);
    }
    public void PlayUpgrade()
    {
        audioSource.PlayOneShot(upgradeSound);
    }
    public void ResumeBGM()
    {
        audioSource.UnPause();
    }
}

