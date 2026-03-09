using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public GameObject terminalUI;
    public GameObject homescreen;
    public GameObject screen1;
    public GameObject screen2;
    private bool isPlayerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is in range and E is pressed
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Check if UI is currently on or off
            bool isCurrentlyOpen = terminalUI.activeSelf;

            // Toggle it to opposite state
            terminalUI.SetActive(!isCurrentlyOpen);
            homescreen.SetActive(true);
            screen1.SetActive(false);
            screen2.SetActive(false);
        }
    }

    // Run when player steps into trigger zone
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    // Run when player steps out of trigger zone
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            // Turn off when player walks away
            terminalUI.SetActive(false);
        }
    }
}
