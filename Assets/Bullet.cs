using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private float speed = 10f;

    [Range(0, 10)]
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime); // Destroy the bullet after lifetime
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed; // Move the bullet in the direction it's facing
    }

}
