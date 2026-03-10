using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Ignorant,
    Scanning,
    Alert,
    Hostile
}
public class EnemyBase : MonoBehaviour
{
    [Range(1, 250)] public int enemyFreq;
    [SerializeField] public EnemyStates currentState = EnemyStates.Scanning;
    public bool isInvincible = false;
    public int enemyHealth;

    [Header("Mobility Stats")]
    public float moveSpeed;

    [Header("Detectors")]
    public EnemyDetection detectorObj;
    public EnemyAttacking attackingObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == EnemyStates.Alert && moveSpeed > 0)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ping"))
        {
            IFFPing_Projectile ping = other.gameObject.GetComponent<IFFPing_Projectile>();
            if (ping.pingFrequency == enemyFreq) StartCoroutine(TemporaryIgnore(ping.neutralizeTime)); //IFF ping is recognized as "Friend" by enemy mecha
            else currentState = EnemyStates.Alert; //IFF ping is recognized as "Foe" by enemy mecha

            Destroy(other.gameObject);
        }
    }

    private IEnumerator TemporaryIgnore(float ignoreTime)
    {
        Debug.Log(this.gameObject.name + " is Ignorant for " + ignoreTime + " seconds!");
        currentState = EnemyStates.Ignorant;
        yield return new WaitForSeconds(ignoreTime);
        Debug.Log(this.gameObject.name + " is no longer Ignorant, and has resumed Scanning mode!");
        currentState = EnemyStates.Scanning;
    }
}
