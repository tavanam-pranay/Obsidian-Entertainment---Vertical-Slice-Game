using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PasswordDoor : DoorOpen
{
    public GameObject terminal;

    void Update()
    {
        // check for CORRECT password
        if (terminal.GetComponent<PasswordTerminal>().getPassword() == "1234")                          // (change this pls)
        {
            doorOpen();
        }
    }
}
