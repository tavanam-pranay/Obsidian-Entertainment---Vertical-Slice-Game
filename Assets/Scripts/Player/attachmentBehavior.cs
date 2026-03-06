using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentBehavior : MonoBehaviour
{

    //Equipment Variables
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile to be fired
    [SerializeField] private Transform firingPoint; // Point from which the projectile will be fired
    [SerializeField] private float magnetStrength = 5f; // Strength of the magnet pull
    [SerializeField] private float magnetRange = 10f; // Range of the magnet pull
    public PlayerMovement playerRoot; // Used to tell if player is flipped, for beam direction

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // If the player presses the fire button, instantiate a projectile at the firing point
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
        Vector2 beamSize = new Vector2(10f, 3f); // Size of the boxcast\
        Vector2 directionOfBeam = firingPoint.right; // Direction of the beam is the right vector of the firing point (assuming the firing point is oriented correctly in the editor)
        float angle = firingPoint.eulerAngles.z;
        if (playerRoot.isFlipped) // If the player is flipped, reverse the direction of the beam
        {
            angle *= -1f; // Flip the angle to match the flipped direction
            angle += 90f; // Rotate the boxcast 90 degrees to match the direction of the beam   
        }
        else
        {
            angle = firingPoint.eulerAngles.z; // If not flipped, just use the firing point's rotation for the boxcast angle
            angle += 90f; // Rotate the boxcast 90 degrees to match the direction of the beam
        }

        //TODO: Make the boxcast not centered on firingpoint. Currently casts backwards toward player for half its width.
        RaycastHit2D[] hit2Ds = Physics2D.BoxCastAll(firingPoint.position, beamSize, angle, directionOfBeam, magnetRange); // Raycast in that direction to see if it hits anything within 10 units

        foreach (RaycastHit2D hit in hit2Ds) // For each thing hit by the raycast
        {
            if (hit.collider.CompareTag("metal")) // If it has the tag "Metal", pull it toward the player.
            {

                Rigidbody2D metalRb = hit.collider.GetComponent<Rigidbody2D>(); // Get the rigidbody of the metal object

                if (metalRb == null)
                {
                    Debug.LogError("An object tagged 'metal' doesn't have a rigidbody!");
                }
                else
                {
                    Vector2 directionToPlayer = (Vector2)firingPoint.position - metalRb.position; // Get the direction from the metal to the player
                    metalRb.AddForce(directionToPlayer.normalized * magnetStrength, ForceMode2D.Impulse); // Add force in direction toward player.
                }
            }
        }


    }

}