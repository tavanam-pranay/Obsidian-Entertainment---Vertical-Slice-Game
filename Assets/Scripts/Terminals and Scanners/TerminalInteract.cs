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

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!terminalCanvas.activeSelf)
            {
                terminalCanvas.SetActive(true);
                if (panels.Count != 0) ShowPanel(0);

                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<AttachmentBehavior>().enabled = false;
                player.GetComponentInChildren<followMouse>().enabled = false;

                GetComponent<TerminalAudio>().StartTerminalSound();
            }
            else
            {
                terminalCanvas.SetActive(false);
                
                player.GetComponent<PlayerMovement>().enabled = true;
                player.GetComponent<AttachmentBehavior>().enabled = true;
                player.GetComponentInChildren<followMouse>().enabled = true;

                GetComponent<TerminalAudio>().StopTerminalSound();
            }
        }
    }

    public void ShowPanel(int panelIndex)
    {
        foreach (GameObject panel in panels) panel.SetActive(false);
        panels[panelIndex].SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            isPlayerInRange = false;

            terminalCanvas.SetActive(false);

            GetComponent<TerminalAudio>().StopTerminalSound();
        }
    }
}