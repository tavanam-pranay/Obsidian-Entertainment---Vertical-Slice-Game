using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateDoor : DoorOpen
{
    public List<GameObject> plates;
    public bool canOpen;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        // check to see if all pressure plates connected to this door are ON
        canOpen = true;
        foreach (GameObject plate in plates)
        {
            if (plate.GetComponent<PlateFunction>().pressed == false)
                canOpen = false;
        }

        if (canOpen) doorOpen();
        else doorClose();
    }
}
