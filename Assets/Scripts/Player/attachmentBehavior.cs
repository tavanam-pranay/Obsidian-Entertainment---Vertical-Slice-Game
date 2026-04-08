using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AttachmentBehavior : MonoBehaviour
{

    [Header("General Attachment Variables")]
    
    [SerializeField] private Transform firingPoint; // Point from which the projectile will be fired
    public int health = 2;
    [SerializeField] private GameObject painPanel;
    public TextMeshProUGUI currentAbility; // Text object to display current ability
    public PlayerMovement playerRoot; // Used to tell if player is flipped, for beam direction
    private GameObject armPanel;
    public GameObject cameraRoot; // Used for screen shake on cannon fire.
    public CursorManager cursorManager; // Used to change cursor based on arm.


    // List of bools to recognize which arms are available to the player; add bools as more arms are added
    public bool hasGrabber;
    public bool hasCannon;
    public bool hasMagnet;

    //Magnet Variables
    [Header("Magnet Variables")]
    [SerializeField] private float magnetStrength = 5f; // Strength of the magnet pull
    [SerializeField] private float heldRange = 1f; // Range at which an item is considered "held".
    [SerializeField] private GameObject beamCollider; // Collider used to detect magnetic objects
    [SerializeField] private GameObject magnetPanel; // HUD panel for when magnet is active. Parent to the lights.
    //Visual GameObjects
    [SerializeField] private GameObject Magnet_Bicep;
    [SerializeField] private GameObject Magnet_Forearm;
    [SerializeField] private GameObject Magnet_Hand;

    private GameObject magnetLights;
    private GameObject defaultLight;
    private GameObject lowLight;
    private GameObject medLight;
    private GameObject highLight;
    private GameObject grabberLights;
    private GameObject cannonLights;

    //Grabber Variables
    [Header("Grabber Variables")]
    [SerializeField] private float grabRange = 1f; // Range at which the grabber can grab objects
    [SerializeField] private Vector2 grabOffset; // Offset from the firingpoint where grabbed objects will be held
    public bool isGrabbing = false; // Whether the grabber is currently grabbing an object
    private Rigidbody2D grabbedObject = null; // Reference to the currently grabbed object
    [SerializeField] private GameObject grabberPanel;
    //Visual GameObjects
    [SerializeField] private GameObject Grabber_Bicep;
    [SerializeField] private GameObject Grabber_Forearm;
    [SerializeField] private GameObject Grabber_Hand;

    //Cannon Variables
    [Header("Cannon Variables")]
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile to be fired
    [SerializeField] private GameObject cannonPanel;
    [SerializeField] private float cannonCooldown = 1f; // Cooldown time between shots
    private float lastShotTime = 0f; // Timestamp for when the last shot was fired, used to calculate time before next shot.
    //Visual GameObjects
    [SerializeField] private GameObject Cannon_Bicep;
    [SerializeField] private GameObject Cannon_Forearm;

    [Header("Arm Sprites/Objects")]
    

    // List to track objects currently inside the beam
    private List<Rigidbody2D> objectsInBeam = new List<Rigidbody2D>();
    private ContactFilter2D contactFilter; // Empty contact filter for OverlapCollider



    void Start()
    {
        // Ensure the beam collider is a trigger
        beamCollider.GetComponent<Collider2D>().isTrigger = true;

        contactFilter = new ContactFilter2D();// Initialize contact filter
        contactFilter.useTriggers = true; // We want to detect trigger colliders
        contactFilter.useLayerMask = false; //On any layer


        //This code is used to find the status lights for the panels once, to avoid searching every frame.
        lowLight = magnetPanel.transform.Find("MstatusLow").gameObject; // This is pretty fragile code and can break if lights are renamed. I used find() on the transform because that only searches for children.
        medLight = magnetPanel.transform.Find("MstatusMid").gameObject;
        highLight = magnetPanel.transform.Find("MstatusFull").gameObject;
        defaultLight = magnetPanel.transform.Find("MstatusReady").gameObject;

        grabberLights = grabberPanel.transform.Find("GrabberLights").gameObject;

        cannonLights = cannonPanel.transform.Find("CannonLights").gameObject;

        magnetLights = defaultLight;
        beamCollider.GetComponent<SpriteRenderer>().enabled = false; 

    }

    void Update()
    {
        switch (playerRoot.armIndex)
        {
            
            case 0: // Grabber

                if (hasGrabber)
                {
                    cursorManager.SetCircleCursor();
                    //Turn on the Grabber's visuals to make them visible
                    Grabber_Bicep.SetActive(true);
                    Grabber_Forearm.SetActive(true);
                    Grabber_Hand.SetActive(true);

                    //Turn off the other Arms to hide them
                    Magnet_Bicep.SetActive(false);
                    Magnet_Forearm.SetActive(false);
                    Magnet_Hand.SetActive(false);
                    Cannon_Bicep.SetActive(false);
                    Cannon_Forearm.SetActive(false);
                }

                beamCollider.GetComponent<SpriteRenderer>().enabled = false; // Disable beam collider if not in use

                if(armPanel) armPanel.SetActive(false); // Disable the current arm panel if there is one, to prevent multiple panels from being active at once

                if (hasGrabber) currentAbility.SetText("Grabber");
                else currentAbility.SetText("(Arm Unavailable)");
                beamCollider.GetComponent<SpriteRenderer>().enabled = false; // hide magnet beam collider

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

            //Cannon Arm
            case 1: // Cannon

                if (hasCannon)
                {
                    //Turn on the Cannon's visuals to make them visible
                    Cannon_Bicep.SetActive(true);
                    Cannon_Forearm.SetActive(true);
                    cursorManager.SetAimCursor();

                    //Turn off the other Arms to hide them
                    Grabber_Bicep.SetActive(false);
                    Grabber_Forearm.SetActive(false);
                    Grabber_Hand.SetActive(false);
                    Magnet_Bicep.SetActive(false);
                    Magnet_Forearm.SetActive(false);
                    Magnet_Hand.SetActive(false);
                }

                beamCollider.GetComponent<SpriteRenderer>().enabled = false; // Disable beam collider if not in use

                if (armPanel) armPanel.SetActive(false); // Disable the current arm panel if there is one, to prevent multiple panels from being active at once
                if (hasCannon) currentAbility.SetText("Cannon");
                else currentAbility.SetText("(Arm Unavailable)");
                beamCollider.GetComponent<SpriteRenderer>().enabled = false; // hide magnet beam collider

                if (Input.GetMouseButtonDown(0) && hasCannon)
                {
                    Shoot();
                    Debug.Log(cannonCooldown - (Time.time - lastShotTime));
                }

                if (hasCannon)
                {
                    armPanel = cannonPanel;
                    armPanel.SetActive(true);

                    float timeUntilNextShot = Mathf.Max(0, cannonCooldown - (Time.time - lastShotTime)); // Calculate time until next shot, using Max to prevent negative numbers
                    float fillAmount = 1 - (timeUntilNextShot / cannonCooldown); // Calculate fill amount based on cooldown


                    switch (fillAmount) // Uses Max to prevent negative numbers. Fill values aren't perfectly linear so I use a switchcase to activate individual lights.
                    {
                        case (<= 0.1f):
                            cannonLights.GetComponent<Image>().fillAmount = 0;
                            break;
                        case (> 0.1f and <= 0.2f):
                            cannonLights.GetComponent<Image>().fillAmount = 0.1f;
                            break;
                        case (> 0.2f and <= 0.3f):
                            cannonLights.GetComponent<Image>().fillAmount = 0.225f;
                            break;
                        case (> 0.3f and <= 0.4f):
                            cannonLights.GetComponent<Image>().fillAmount = 0.35f;
                            break;
                        case (> 0.4f and <= 0.5f):
                            cannonLights.GetComponent<Image>().fillAmount = 0.475f;
                            break;
                        case (> 0.5f and <= 0.6f):
                            cannonLights.GetComponent<Image>().fillAmount = 0.610f;
                            break;
                        case (> 0.6f and <= 0.95f):
                            cannonLights.GetComponent<Image>().fillAmount = 0.735f;
                            break;
                        case (>= 0.95f):
                            cannonLights.GetComponent<Image>().fillAmount = 1f;
                            break;
                        default:
                            cannonLights.GetComponent<Image>().fillAmount = 1f;
                            break;

                    }
                }
                break;

            //Magnet Arm
            case 2: // Magnet

                if (hasMagnet)
                {
                    cursorManager.SetSquareCursor();
                    //Turn on the Magnet's visuals to make them visible
                    Magnet_Bicep.SetActive(true);
                    Magnet_Forearm.SetActive(true);
                    Magnet_Hand.SetActive(true);


                    //Turn off the other Arms to hide them
                    Grabber_Bicep.SetActive(false);
                    Grabber_Forearm.SetActive(false);
                    Grabber_Hand.SetActive(false);
                    Cannon_Bicep.SetActive(false);
                    Cannon_Forearm.SetActive(false);
                }

                beamCollider.GetComponent<SpriteRenderer>().enabled = true; // Disable beam collider if not in use
                if (armPanel) armPanel.SetActive(false); // Disable the current arm panel if there is one, to prevent multiple panels from being active at once
                if (hasMagnet) currentAbility.SetText("Magnet");
                else currentAbility.SetText("(Arm Unavailable)");

                beamCollider.GetComponent<SpriteRenderer>().enabled = true; // Activate magnet beam collider sprite.

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
        if (Time.time - lastShotTime >= cannonCooldown) // Checks if enough time has passed since the last shot
        {
            lastShotTime = Time.time; // Updates the last shot timestamp. 
            GameObject bullet = Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), playerRoot.GetComponent<Collider2D>()); //don't kill the player!!
            cameraRoot.GetComponent<Shake>().gunShake();
        }
        else
        {
            Debug.Log("Cannon is on cooldown. Play unloaded click sound here");
        }
    }

    private void Magnet()
    {
        
        objectsInBeam.RemoveAll(rb => rb == null);// Clean up any destroyed objects from the list
        List<Collider2D> collidersInBeam = new List<Collider2D>(); //Make a new list to store the colliders currently in the beam
        beamCollider.GetComponent<Collider2D>().OverlapCollider(contactFilter, collidersInBeam); // Get all colliders currently in the beam

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
        beamCollider.GetComponent<Collider2D>().OverlapCollider(contactFilter, collidersInBeam); // Get all colliders currently in the beam

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
            // re-enable collision
            Collider2D grabbedCollider = grabbedObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(grabbedCollider, playerRoot.GetComponent<Collider2D>(), false);//Exclude the grabbed object from the player's collider to prevent physics issues

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

    public void TakeDamage(int d)
    {
        if (health - d <= 0)
        {
            health = 0; //Temporary scene reset stand-in. Should load death screen./////////////////////////
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Do we have to unload this scene first? idk ////////////////////////////
        }
        else
        {
            health -= d;
            cameraRoot.GetComponent<Shake>().bigShake(); // Call the gun shake effect

            painPanel.SetActive(true);

        }
    }


}


