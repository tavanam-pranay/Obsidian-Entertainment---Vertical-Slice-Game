using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    public EnemyBase parentEnemy;
    public PlayerMovement player;
    public float reloadTime;
    private float currentTimer;
    public int attackDamage;
    public float bulletSpread;
    public float hostileRange;

    [Header("Ranged Attack Params")]
    public GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        parentEnemy = GetComponentInParent<EnemyBase>();
        currentTimer = reloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer -= Time.deltaTime;
        RaycastHit2D hitAttack = Physics2D.Raycast(transform.position, transform.up, hostileRange);
        if (hitAttack)
        {
            if (hitAttack.collider.CompareTag("Player") && currentTimer <= 0)
            {
                Shoot();
            }
        }
        else
        {

        }
    }

    private void Shoot()
    {
        Instantiate(projectilePrefab, transform.position, transform.rotation);
    }
}
