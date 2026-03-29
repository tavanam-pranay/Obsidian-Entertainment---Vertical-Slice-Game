using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentBehavior : MonoBehaviour
{

    [Header("General Attachment Variables")]
    
    [SerializeField] private Transform firingPoint; // Point from which the projectile will be fired
    public TextMeshProUGUI currentAbility; // Text object to display current ability
    public PlayerMovement playerRoot; // Used to tell if player is flipped, for beam direction
    private GameObject armPanel;
    public GameObject cameraRoot; // Used for screen shake on cannon fire.

    // List of bools to recognize which arms are available to the player; add bools as more arms are added
    public bool hasGrabber;
    public bool hasCannon;
    public bool hasMagnet;

    //Magnet Variables
    [Header("Magnet Variables")]
    [SerializeField] private float magnetStrength = 5f; // Strength of the magnet pull
    [SerializeField] private float heldRange = 1f; // Range at which an item is considered "held".
    [SerializeField] private Collider2D beamCollider; // Collider used to detect magnetic objects
    [SerializeField] private GameObject magnetPanel; // HUD panel for when magnet is active. Parent to the lights.
    
    private GameObject magnetLights;
    private GameObject defaultLight;
    private GameObject lowLight;
    private GameObject medLight;
    private GameObject highLight;
    private GameObject grabberLights;

    //Grabber Variables
    [Header("Grabber Variables")]
    [SerializeField] private float grabRange = 1f; // Range at which the grabber can grab objects
    [SerializeField] private Vector2 grabOffset; // Offset from the firingpoint where grabbed objects will be held
    public bool isGrabbing = false; // Whether the grabber is currently grabbing an object
    private Rigidbody2D grabbedObject = null; // Reference to the currently grabbed object
    [SerializeField] private GameObject grabberPanel;

    //Cannon Variables
    [Header("Cannon Variables")]
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile to be fired
    [SerializeField] private GameObject cannonPanel;


    // List to track objects currently inside the beam
    private List<Rigidbody2D> objectsInBeam = new List<Rigidbody2D>();
    private ContactFilter2D contactFilter; // Empty contact filter for OverlapCollider



    void Start()
    {
        // Ensure the beam collider is a trigger
        beamCollider.isTrigger = true;

        contactFilter = new ContactFilter2D();// Initialize contact filter
        contactFilter.useTriggers = true; // We want to detect trigger colliders
        contactFilter.useLayerMask = false; //On any layer


        //This code is used to find the status lights for the panels once, to avoid searching every frame.
        lowLight = magnetPanel.transform.Find("MstatusLow").gameObject; // This is pretty fragile code and can break if lights are renamed. I used find() on the transform because that only searches for children.
        medLight = magnetPanel.transform.Find("MstatusMid").gameObject;
        highLight = magnetPanel.transform.Find("MstatusFull").gameObject;
        defaultLight = magnetPanel.transform.Find("MstatusReady").gameObject;

        grabberLights = grabberPanel.transform.Find("GrabberLights").gameObject;

        magnetLights = defaultLight;

    }

    void Update()
    {
        switch (playerRoot.armIndex)
        {
            case 0: // Grabber
                
                if(armPanel) armPanel.SetActive(false); // Disable the current arm panel if there is one, to prevent multiple panels from being active at once

                if (hasGrabber) currentAbility.SetText("Grabber");
                else currentAbility.SetText("(Arm Unavailable)");

                if (Input.GetMouseButtonDown(0) && !isGrabbing && hasGrabber)
                {
                    GrabberGrab();
                }
                else if (Input.GetMouseButtonDown(0) && isGrabbing && hasGrabber)
                {
                    GrabberRelease();
                    grabberLights.SetActive(false);
                }
                
                if (hasGrabber)
                {
                    armPanel = grabberPanel;
                    armPanel.SetActive(true);
                }

                break;

            case 1: // Cannon

                if (armPanel) armPanel.SetActive(false); // Disable the current arm panel if there is one, to prevent multiple panels from being active at once
                if (hasCannon) currentAbility.SetText("Cannon");
                else currentAbility.SetText("(Arm Unavailable)");

                if (Input.GetMouseButtonDown(0) && hasCannon)
                {
                    Shoot();
                }

                if (hasCannon)
                {
                    armPanel = cannonPanel;
                    armPanel.SetActive(true);
                }
                break;

            case 2: // Magnet

                if (armPanel) armPanel.SetActive(false); // Disable the current arm panel if there is one, to prevent multiple panels from being active at once
                if (hasMagnet) currentAbility.SetText("Magnet");
                else currentAbility.SetText("(Arm Unavailable)");

                if (hasMagnet) // If has but not necessarily clicking.
                {
                    armPanel = magnetPanel;
                    armPanel.SetActive(true);
                    
                }

                if (Input.GetMouseButton(0) && hasMagnet)
                {

                    Magnet();
                }
                else // Reset to default light when magnet is not active
                {
                    defaultLight.SetActive(true);
                    lowLight.SetActive(false);
                    medLight.SetActive(false);
                    highLight.SetActive(false);
                    magnetLights = defaultLight;
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
        cameraRoot.GetComponent<Shake>().gunShake(); // Call the gun shake effect
    }

    private void Magnet()
    {
        
        objectsInBeam.RemoveAll(rb => rb == null);// Clean up any destroyed objects from the list
        List<Collider2D> collidersInBeam = new List<Collider2D>(); //Make a new list to store the colliders currently in the beam
        beamCollider.OverlapCollider(contactFilter, collidersInBeam); // Get all colliders currently in the beam

        foreach (Collider2D col in collidersInBeam)
        {
            if (col.GetComponent<MagneticTag>() != null){ // If object is magnetic

                Rigidbody2D metalRb = col.GetComponent<Rigidbody2D>();

                if (metalRb != null)
                {
                    Vector2 directionToPlayer = (Vector2)firingPoint.position - metalRb.position; // Magnetic items igidbody must be set to dynamic.
                    metalRb.AddForce(directionToPlayer.normalized * magnetStrength, ForceMode2D.Impulse);
                    
                    float distance = Vector2.Distance(firingPoint.position, metalRb.position); //Get distance to object for status lights

                    //Debug.Log("Distance to object: " + distance); //Was used for determining thresholds.

                    if (distance < 9f && distance > 5f)
                    {
                        defaultLight.SetActive(false);
                        lowLight.SetActive(true);
                        medLight.SetActive(false);
                        highLight.SetActive(false);
                        magnetLights = lowLight;
                    }
                    else if (distance < 5f && distance > 2f)
                    {
                        defaultLight.SetActive(false);
                        lowLight.SetActive(false);
                        medLight.SetActive(true);
                        highLight.SetActive(false);
                        magnetLights = medLight;
                    }
                    else if (distance < 1.5f)
                    {
                        defaultLight.SetActive(false);
                        lowLight.SetActive(false);
                        medLight.SetActive(false);
                        highLight.SetActive(true);
                        magnetLights = highLight;
                    }



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


                    grabberLights.SetActive(true);

                    if (directionToPlayer.magnitude <= grabRange)
                    {
                        
                        grabOffset = grabbedObject.position - (Vector2)firingPoint.position; //Calculates the grab offset
                        
                        grabbedObject.MovePosition(firingPoint.position + (Vector3)grabOffset); // Moves the object to the grab offset position
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
            grabOffset = Vector2.zero; // Reset grab offset
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


