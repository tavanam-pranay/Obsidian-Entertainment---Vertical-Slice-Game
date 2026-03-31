using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalAudio : MonoBehaviour
{
    public AudioSource terminalAudioSource;
    public AudioClip terminalBGM;

    public void StartTerminalSound()
    {
        terminalAudioSource.clip = terminalBGM;
        terminalAudioSource.Play();
    }

    public void StopTerminalSound()
    {
        terminalAudioSource.Stop();
    }
}