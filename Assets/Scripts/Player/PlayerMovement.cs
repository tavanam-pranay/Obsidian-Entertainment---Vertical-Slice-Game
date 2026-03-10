using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D player;
    public bool canFlip = true;
    public bool isFlipped = false;
    public float flipThreshold = 0.1f; // TODO: Actually implement this. Threshold for determining when to flip the player based on movement direction
    public int armIndex = 0; //Index of the current arm, used for which effect. 0 (default) is the basic grabber.
    [SerializeField] private GameObject pauseScriptParent; // Reference to the pause menu script, used to check if the game is paused before allowing movement and flipping


    [Range(0.01f, 0.1f)]
    public float flipTime = 0.02f; // Time it takes to complete the flip animation

    private Vector2 movement;

    //Script References
    private AttachmentBehavior attachmentBehavior;
    private AbilityController abilityController;

    void Start()
    {
        attachmentBehavior = GetComponent<AttachmentBehavior>();
        abilityController = GetComponent<AbilityController>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (canFlip)
        {
            if (movement.x > flipThreshold) // If the player is moving right, set isFlipped to false
            {
                isFlipped = false;
            }
            else if (movement.x < -flipThreshold) // If the player is moving left, set isFlipped to true
            {
                isFlipped = true;
            }

            if (isFlipped) // If the player is flipped, rotate the root (including rig and everything) -180 degrees on the y-axis to face left. This is done with Lerp for animation.
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, -180f, 0f), Time.deltaTime / flipTime);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime / flipTime);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) // If the player presses the space key, cycle through arms
        {
            armIndex = (armIndex + 1) % 3; // Cycle through 0, 1, and 2
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // If the player presses the escape key, quit the game
        {
            pauseMenu pauseMenuScript = pauseScriptParent.GetComponent<pauseMenu>();

            pauseMenuScript.openPauseMenu();

        }
    }

    void FixedUpdate()
    {
        player.MovePosition(player.position + movement * moveSpeed * Time.deltaTime);
    }

}
