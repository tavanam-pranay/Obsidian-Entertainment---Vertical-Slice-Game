using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public bool open;

    private void OnTriggerEnter2D(Collider2D keycard)
    {
        if (keycard.CompareTag("Keycard"))
            open = true;
    }
}
