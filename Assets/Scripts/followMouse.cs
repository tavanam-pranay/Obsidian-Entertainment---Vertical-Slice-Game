using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script makes whatever it's atttached to (designed to be the arm's target) follow the mouse position when the E key is pressed. 
 * Whatever position you set in-engine before hitting play will be saved as the rest position. Written by Etienne
 */

public class followMouse : MonoBehaviour
{
    [Range(1f, 10f)]
    public float speed = 5f; // Speed of move to target
    public PlayerMovement playerRoot; //Used to tell if player is flipped

    public Transform shoulder; // Empty transform used as the center of the arm's reach
    public float maxDistanceFromShoulder = 1.5f; // Max distance from shoulder.

    Vector2 restPos;        // Position the target will lerp back to.
    Vector2 restOffset;     // Offset from the parent object to the target's rest position
    Vector2 flippedOffset;  // Offset when flipped


    void Start()
    {
        if (transform.parent == null)
        {
            Debug.LogError("The target requires a parent object to exist offset to.");
            enabled = false; // Disable the script if no parent is found
            return;
        }

        restOffset = (Vector2)(transform.position - transform.parent.position); // Set the rest offset based on the initial position of the target relative to its parent
        flippedOffset = new Vector2(-restOffset.x, restOffset.y); // Set the flipped offset to be the mirror of the rest offset on the x-axis
        restPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRoot.isFlipped) // If the player is flipped, rest position is the flipped offset from the parent.
        {
            restPos = (Vector2)transform.parent.position + flippedOffset;
        }
        else
        {
            restPos = (Vector2)transform.parent.position + restOffset;
        }

        //When E is pressed, the object will lerp to the mouse position.
        if (Input.GetKey(KeyCode.E))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

            
            Vector2 center = shoulder.position;
            Vector2 toTarget = targetPos - center; // Vector from shoulder to target position

            if (toTarget.magnitude > maxDistanceFromShoulder) //If said vector is longer than max, clamp it.
            {
                toTarget = toTarget.normalized * maxDistanceFromShoulder;
            }

            Vector2 clampedTargetPos = center + toTarget; // Final target position is the shoulder position plus the clamped vector

            transform.position = Vector2.Lerp(transform.position, clampedTargetPos, Time.deltaTime * speed); // Lerp to the target position

            if (mousePos.x < Screen.width / 2f)
            {
                playerRoot.isFlipped = true;
            }
            else
            {
                playerRoot.isFlipped = false;
            }
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, restPos, Time.deltaTime * speed);
        }
    }
}
