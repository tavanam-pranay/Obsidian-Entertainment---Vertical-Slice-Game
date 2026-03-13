using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret : MonoBehaviour
{
    
    [SerializeField] [Range(1, 7)] private float shootCooldown = 3f; // Time between shots 
    [SerializeField] private GameObject bulletPrefab; // Prefab for the bullet to be instantiated
    [SerializeField] private Transform firingPoint;
    public bool isShooting = false;
    private bool isInvoking = false; // Track whether a next shot is already scheduled

    private void Update()
    {
        // If isshooting is true, shoot every shootCooldown seconds. 
        if (isShooting && !isInvoking)
        {
            InvokeRepeating("Shoot", shootCooldown, 1f); // Fix this eventuall: Shoots every shootCooldown, not by the '1f' parameter. bug that shootCooldowbn cant be more than like, 5 seconds.
            isInvoking = true;
        }
        else if (!isShooting && isInvoking)
        {
            CancelInvoke("Shoot"); // Stop shooting when isShooting is false
            isInvoking = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) // If the player is within the turret's detection collider (which is a trigger), try and cast to the player.
        {
            Vector2 direction = (other.transform.position - transform.position).normalized; //Direction from turret to player

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100); //Raycast to every collider in the direction except for layer 2 (default ignore raycast layer). This turret's hitbox collider is on layer 2 to prevent it from hitting itself.

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                //Debug.Log("Player detected by turret!");
                Debug.DrawRay(transform.position, direction * 10, Color.green); //Draw the ray for debugging purposes
                isShooting = true; //Start shooting if the raycast hits the player

                //https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Mathf.Atan2.html 

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //Atan2 calculates the angle to rotate the turret towards the player
                transform.rotation = Quaternion.Euler(0, 0, angle - 90f); //Rotate the firing point towards the player

            }
            else if (hit.collider != null)
            {
                Debug.Log("Turret detected an obstacle: " + hit.collider.name);
                Debug.DrawRay(transform.position, direction * 10, Color.red); //Draw the ray for debugging purposes
                isShooting = false; //Stop shooting if the raycast hits an obstacle
            }
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        Debug.Log("Turret shot a bullet!");
    }

    private void OnTriggerExit2D(Collider2D other) // Ensures the turret stops shooting when player leaves.
    {
        if (other.CompareTag("Player"))
        {
            isShooting = false; // Stop shooting when player leaves the trigger area
        }
    }



}
