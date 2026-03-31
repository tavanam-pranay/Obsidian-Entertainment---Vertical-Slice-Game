using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PasswordDoor : DoorOpen
{
    public GameObject terminal;
    public string password = "1234";
    public string exitPassword = "fin";

    private bool isOpen = false;

    void Update()
    {
        bool shouldOpen = false;

        if (CompareTag("Finish"))
        {
            if (terminal.GetComponent<PasswordTerminal>().getPassword() == exitPassword)
            {
                shouldOpen = true;
            }
        }
        else
        {
            if (terminal.GetComponent<PasswordTerminal>().getPassword() == password)
            {
                shouldOpen = true;
            }
        }

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