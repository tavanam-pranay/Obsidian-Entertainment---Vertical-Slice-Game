using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentBehavior : MonoBehaviour
{

    //Equipment Variables
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile to be fired
    [SerializeField] private Transform firingPoint; // Point from which the projectile will be fired
    
    //TMP variable for editing text.
    public TextMeshProUGUI currentAbility; // Text object to display current ability

    //Magnet Variables
    [SerializeField] private float magnetStrength = 5f; // Strength of the magnet pull
    [SerializeField] private float heldRange = 1f; // Range at which an item is considered "held".
    [SerializeField] private Collider2D beamCollider; // Collider used to detect magnetic objects
    public PlayerMovement playerRoot; // Used to tell if player is flipped, for beam direction

    //Grabber Variables
    [SerializeField] private float grabRange = 1f; // Range at which the grabber can grab objects
    [SerializeField] private Vector2 grabOffset; // Offset from the firingpoint where grabbed objects will be held
    public bool isGrabbing = false; // Whether the grabber is currently grabbing an object
    private Rigidbody2D grabbedObject = null; // Reference to the currently grabbed object

    // List to track objects currently inside the beam
    private List<Rigidbody2D> objectsInBeam = new List<Rigidbody2D>();
    private ContactFilter2D contactFilter; // Empty contact filter for OverlapCollider

    // List of bools to recognize which arms are available to the player; add bools as more arms are added
    public bool hasGrabber;
    public bool hasCannon;
    public bool hasMagnet;

    void Start()
    {
        if (beamCollider == null)
        {
            Debug.LogError("Beam collider is not assigned!");
            enabled = false;
            return;
        }

        // Ensure the beam collider is a trigger
        beamCollider.isTrigger = true;

        contactFilter = new ContactFilter2D();// Initialize contact filter
        contactFilter.useTriggers = true; // We want to detect trigger colliders
        contactFilter.useLayerMask = false; 
    }

    void Update()
    {
        switch (playerRoot.armIndex)
        {
            case 0:
                // Basic grabber, no shooting
                if(hasGrabber) currentAbility.SetText("Grabber");
                else currentAbility.SetText("(Arm Unavailable)");

                if (Input.GetMouseButtonDown(0) && !isGrabbing && hasGrabber)
                {
                    GrabberGrab();
                }
                else if (Input.GetMouseButtonDown(0) && isGrabbing && hasGrabber)
                {
                    GrabberRelease();
                }

                break;
            case 1:
                if (hasCannon) currentAbility.SetText("Cannon");
                else currentAbility.SetText("(Arm Unavailable)");

                if (Input.GetMouseButtonDown(0) && hasCannon)
                {
                    Shoot();
                }
                break;
            case 2:
                if (hasMagnet) currentAbility.SetText("Magnet");
                else currentAbility.SetText("(Arm Unavailable)");

                if (Input.GetMouseButton(0) && hasMagnet)
                {
                    Magnet();
                }
                break;
            default:
                Debug.LogWarning("Invalid arm index: " + playerRoot.armIndex);
                break;
        }

        if (isGrabbing && grabbedObject != null) // If we're currently grabbing an object, we need to keep it at the grabbed position
        {
            Vector2 directionToPlayer = (Vector2)firingPoint.position - grabbedObject.position;
            grabbedObject.MovePosition(firingPoint.position + (Vector3)grabOffset);

            Collider2D grabbedCollider = grabbedObject.GetComponent<Collider2D>();

            if (grabbedCollider != null)
            {
                    Physics2D.IgnoreCollision(grabbedCollider, playerRoot.GetComponent<Collider2D>(), true);//Exclude the grabbed object from the player's collider to prevent physics issues
            }

        }


    }

    private void Shoot()
    {
        Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
    }

    private void Magnet()
    {
        
        objectsInBeam.RemoveAll(rb => rb == null);// Clean up any destroyed objects from the list
        List<Collider2D> collidersInBeam = new List<Collider2D>(); //Make a new list to store the colliders currently in the beam
        beamCollider.OverlapCollider(contactFilter, collidersInBeam); // Get all colliders currently in the beam

        foreach (Collider2D col in collidersInBeam)
        {
            if (col.CompareTag("magnetic")){ // If object is magnetic

                Rigidbody2D metalRb = col.GetComponent<Rigidbody2D>();

                if (metalRb != null)
                {
                    Vector2 directionToPlayer = (Vector2)firingPoint.position - metalRb.position;
                    metalRb.AddForce(directionToPlayer.normalized * magnetStrength, ForceMode2D.Impulse);
                }
                else
                {
                    Debug.LogWarning("Object tagged as Magnetic does not have a Rigidbody2D: " + col.gameObject.name);
                }
            }
        }
    }

    private void GrabberGrab()
    {
        List<Collider2D> collidersInBeam = new List<Collider2D>(); //Make a new list to store the colliders currently in the beam
        beamCollider.OverlapCollider(contactFilter, collidersInBeam); // Get all colliders currently in the beam

        foreach (Collider2D col in collidersInBeam)
        {
            
            if (Vector2.Distance(firingPoint.position, col.transform.position) > grabRange)//Check if in range
            {
                continue; // Skip this object if it's out of range
            }

            if (col.GetComponent<GrabbableTag>() != null && !isGrabbing) // If object is grabbable and we're not already grabbing something
            { 
                grabbedObject = col.GetComponent<Rigidbody2D>(); 

                if (grabbedObject != null) // If object is grabbable
                {
                    Vector2 directionToPlayer = (Vector2)firingPoint.position - grabbedObject.position;

                    if (directionToPlayer.magnitude <= grabRange)
                    {
                        // Move the object to the grab offset position
                        grabbedObject.MovePosition(firingPoint.position + (Vector3)grabOffset);
                        isGrabbing = true;
                    }
                }
                else
                {
                    Debug.LogWarning("Object with Grabbable does not have a Rigidbody2D: " + col.gameObject.name);
                }
            }
        }
    }

    private void GrabberRelease()
    {
        if (grabbedObject != null)
        {
            grabbedObject = null; // Release the object
            isGrabbing = false;
        }
    }



    public void addArm(ArmSO arm)
    {
        switch (arm.armIndexForPlayer)
        {
            case 0:
                hasGrabber = true; break;
            case 1:
                hasCannon = true; break;
            case 2:
                hasMagnet = true; break;

        }
    }
}


