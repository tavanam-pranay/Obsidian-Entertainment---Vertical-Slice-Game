using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockedDoorScanner : DoorOpen
{
    public GameObject scanner;

    // Update is called once per frame
    void Update()
    {
        if (scanner.GetComponent<Scanner>().open == true)
        {
            doorOpen();
        }
    }
}
