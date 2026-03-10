using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZoneManager : MonoBehaviour
{
    public PlayerMovement playerTarget;
    public List<EnemyBase> enemiesInZone;
    

    // Update is called once per frame
    void Start()
    {
        foreach (EnemyBase enemy in enemiesInZone)
        {
            enemy.detectorObj.player = playerTarget;
            enemy.attackingObj.player = playerTarget;
            enemy.enabled = false;
            enemy.detectorObj.enabled = false;
            enemy.attackingObj.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (EnemyBase enemy in enemiesInZone)
            {
                enemy.enabled = true;
                enemy.detectorObj.enabled = true;
                enemy.attackingObj.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (EnemyBase enemy in enemiesInZone)
            {
                enemy.enabled = false;
                enemy.detectorObj.enabled = false;
                enemy.attackingObj.enabled = false;
            }
        }
    }
}
