using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator doorAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetBool("open", true);
        }
    }

    public void doorOpen()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().SetBool("open", true);
        GetComponent<Animator>().SetBool("close", false);
    }

    public void doorClose()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Animator>().SetBool("close", true);
        GetComponent<Animator>().SetBool("open", false);
    }
}
