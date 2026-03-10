using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProtoEnemy : MonoBehaviour
{
    [Range(1, 250)] public int enemyFreq;
    [SerializeField] private EnemyStates currentState = EnemyStates.Scanning;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
