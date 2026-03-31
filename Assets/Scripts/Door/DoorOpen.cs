using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator doorAnimator;

    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetBool("open", true);
            audioSource.PlayOneShot(openSound);
        }
    }

    public void doorOpen()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().SetBool("open", true);
        GetComponent<Animator>().SetBool("close", false);

        audioSource.PlayOneShot(openSound);
    }

    public void doorClose()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Animator>().SetBool("close", true);
        GetComponent<Animator>().SetBool("open", false);

        audioSource.PlayOneShot(closeSound);
    }
}