using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PasswordDoor : DoorOpen
{
    public GameObject terminal;
    public string password = "1234";
    public string exitPassword = "fin";

    void Update()
    {
        // special case for final exit door
        if (CompareTag("Finish"))
        {
            if (terminal.GetComponent<PasswordTerminal>().getPassword() == exitPassword)
            {
                Debug.Log("final door opened");
                doorOpen();
            }
        }
        // check for CORRECT password
        else if (terminal.GetComponent<PasswordTerminal>().getPassword() == password)
        {
            doorOpen();
        }
    }
}
