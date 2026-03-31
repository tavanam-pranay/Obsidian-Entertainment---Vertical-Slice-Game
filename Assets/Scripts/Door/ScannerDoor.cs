using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockedDoorScanner : DoorOpen
{
    public GameObject scanner;

    private bool isOpen = false;

    void Update()
    {
        bool shouldOpen = scanner.GetComponent<Scanner>().open;

        if (shouldOpen && !isOpen)
        {
            doorOpen();
            isOpen = true;
        }
        else if (!shouldOpen && isOpen)
        {
            doorClose();
            isOpen = false;
        }
    }
}
