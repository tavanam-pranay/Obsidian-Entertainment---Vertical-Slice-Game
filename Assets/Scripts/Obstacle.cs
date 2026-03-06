using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //When colliding with "projectile" tagged object, lower health by 1 and destroy the projectile

    public int health = 3;
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
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        opacity = (float)health / 3.0f; // Assuming max health is 3

        Color color = GetComponent<Renderer>().material.color; //Get current color
        color.a = opacity; //Set opacity
        GetComponent<Renderer>().material.color = color; //Apply new color with updated opacity

    }
}
