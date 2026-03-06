using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentBehavior : MonoBehaviour
{

    //Equipment Variables
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile to be fired
    [SerializeField] private Transform firingPoint; // Point from which the projectile will be fired

    //Magnet Variables
    [SerializeField] private float magnetStrength = 5f; // Strength of the magnet pull
    [SerializeField] private float magnetRange = 10f; // Range of the magnet pull
    [SerializeField] private float heldRange = 1f; // Range at which an item is considered "held".
    [SerializeField] private Collider2D beamCollider; // Collider used to detect magnetic objects
    public PlayerMovement playerRoot; // Used to tell if player is flipped, for beam direction

    // List to track objects currently inside the beam
    private List<Rigidbody2D> objectsInBeam = new List<Rigidbody2D>();
    private ContactFilter2D contactFilter; // Empty contact filter for OverlapCollider
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
        if (Input.GetMouseButtonDown(0))
        {
            //Magnet();
        }

        if (Input.GetMouseButton(0))
        {
            Magnet();
        }
    }

    private void Shoot()
    {
        Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
    }

    private void Magnet()
    {
        // Clean up any destroyed objects from the list
        objectsInBeam.RemoveAll(rb => rb == null);
        List<Collider2D> collidersInBeam = new List<Collider2D>();
        beamCollider.OverlapCollider(contactFilter, collidersInBeam);

        foreach (Collider2D col in collidersInBeam)
        {
            if (col.CompareTag("magnetic")){ // If object is magnetic

                Debug.Log("magnetic object detected: " + col.gameObject.name);

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
}


