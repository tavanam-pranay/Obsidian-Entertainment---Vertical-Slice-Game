using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PasswordDoor : DoorOpen
{
    public GameObject terminal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check for CORRECT password
        if (terminal.GetComponent<PasswordTerminal>().getPassword() == "1234")                          // (change this pls)
        {
            // stop checking for pressure plate logic anymore since we got the password
            GetComponent<PlateActivatedDoor>().canOpen = true;
            doorOpen();
        }
    }
}
