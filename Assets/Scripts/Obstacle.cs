using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //When colliding with "projectile" tagged object, lower health by 1 and destroy the projectile

    [SerializeField] private int health = 3;
    public bool isDestructible = true;
    public bool magnetic = false;

    private float opacity = 1.0f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(this.gameObject.name + "collided with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("projectile"))
        {
            health -= 1;
            Debug.Log("Health: " + health);
            Destroy(other.gameObject);
            if (health <= 0 && isDestructible)
            {
                Destroy(gameObject);
            }
        }
    }
    private void Start()
    {
        if (magnetic)
        {
            gameObject.tag = "magnetic";
        }
        else {

            gameObject.tag = "obstacle"; // Defaults to obstacle if not magnetic
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isDestructible)
        {
            opacity = (float)health / 3.0f; // Assuming max health is 3

            Color color = GetComponent<Renderer>().material.color; //Get current color
            color.a = opacity; //Set opacity
            GetComponent<Renderer>().material.color = color; //Apply new color with updated opacity
        }
    }
}
