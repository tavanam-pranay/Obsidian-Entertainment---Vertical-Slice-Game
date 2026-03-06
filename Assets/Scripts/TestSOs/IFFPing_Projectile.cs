using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFFPing_Projectile : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float speed = 40f;

    [Range(0, 10)]
    [SerializeField] private float lifetime = 1f;

    [Range(1, 250)] public int pingFrequency;
    public float neutralizeTime;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime); // Destroy the ping after lifetime
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed; // Move the bullet in the direction it's facing
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Ping hit enemy! Identified enemy = " + other.gameObject.name);
        }
    }
}
