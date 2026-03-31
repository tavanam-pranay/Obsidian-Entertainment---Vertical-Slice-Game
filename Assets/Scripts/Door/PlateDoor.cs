using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateDoor : DoorOpen
{
    public List<GameObject> plates;
    public bool canOpen;

    private bool isOpen = false;

    void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    void Update()
    {
        canOpen = true;

        foreach (GameObject plate in plates)
        {
            if (plate.GetComponent<PlateFunction>().pressed == false)
                canOpen = false;
        }

        if (canOpen && !isOpen)
        {
            doorOpen();
            isOpen = true;
        }
        else if (!canOpen && isOpen)
        {
            doorClose();
            isOpen = false;
        }
    }
}