using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TerminalInteract : MonoBehaviour
{

    public GameObject terminalCanvas;
    public List<GameObject> panels = new List<GameObject>();
    private bool isPlayerInRange = false;
    public GameObject player;

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
            if (!terminalCanvas.activeSelf)
            {
                // turn on and show homescreen
                terminalCanvas.SetActive(true);
                if (panels.Count != 0) ShowPanel(0);

                // Disable player movement and arms
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<AttachmentBehavior>().enabled = false;
            }
            else
            {
                // turn off
                terminalCanvas.SetActive(false);

                // Enable player movement and arms
                player.GetComponent<PlayerMovement>().enabled = true;
                player.GetComponent<AttachmentBehavior>().enabled = true;
            }
        }
    }

    public void ShowPanel(int panelIndex)
    {
        // Hide all panels first, then show the selected panel
        foreach (GameObject panel in panels) panel.SetActive(false);
        panels[panelIndex].SetActive(true);
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
            terminalCanvas.SetActive(false);
        }
    }
}
