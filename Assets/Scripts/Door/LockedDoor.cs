using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public List<GameObject> plates;
    private bool canOpen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        canOpen = true;
        foreach (GameObject plate in plates)
        {
            if (plate.GetComponent<PlateFunction>().pressed == false)
                canOpen = false;
        }

        if (canOpen) doorOpen();
        else doorClose();
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
