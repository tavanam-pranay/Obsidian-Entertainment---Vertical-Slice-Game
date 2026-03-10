using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyDetection_Radar : EnemyDetection
{
    //public float radarRange;
    public Vector2 lastPlayerPos;
    public float pingFreq;
    private float currentTimer;
    // Start is called before the first frame update
    void Start()
    {
        parentEnemy = GetComponentInParent<EnemyBase>();
        currentTimer = pingFreq;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, detectionRange * transform.up, Color.blue);
        Debug.DrawRay(transform.position, hostileRange * transform.up, Color.red);
        currentTimer -= Time.deltaTime;
        if (currentTimer < 0)
        {
            RaycastHit2D hitDetect = Physics2D.Raycast(transform.position, transform.up, detectionRange);
            if (hitDetect)
            {
                //Debug.Log($"Hit: {hit.collider.name} at distance {hit.distance}");
                //Debug.Log($"Hit: {hit.collider.name}");

                if (hitDetect.collider.CompareTag("Player"))
                {
                    Debug.Log("Player detected!");
                    parentEnemy.currentState = EnemyStates.Alert;
                    lastPlayerPos = hitDetect.collider.gameObject.transform.position;

                    RaycastHit2D hitAttack = Physics2D.Raycast(transform.position, transform.up, hostileRange);
                    if (hitAttack.collider.CompareTag("Player"))
                    {
                        parentEnemy.currentState = EnemyStates.Hostile;
                        parentEnemy.attackingObj.enabled = true;
                        parentEnemy.attackingObj.hostileRange = hostileRange;
                        //this.enabled = false;
                    }
                }
                else
                {
                    parentEnemy.currentState = EnemyStates.Scanning;
                }
            }

            currentTimer = pingFreq;
        }
    }

}
