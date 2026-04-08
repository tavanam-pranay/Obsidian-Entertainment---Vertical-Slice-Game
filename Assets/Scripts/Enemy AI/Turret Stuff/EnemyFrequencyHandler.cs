using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrequencyHandler : MonoBehaviour
{
    [Range(1, 250)] public int enemyFreq;
    [SerializeField] public EnemyStates currentState = EnemyStates.Scanning;

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
        if (other.CompareTag("Ping"))
        {
            IFFPing_Projectile ping = other.gameObject.GetComponent<IFFPing_Projectile>();
            if (ping.pingFrequency == enemyFreq) StartCoroutine(TemporaryIgnore(ping.neutralizeTime));
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
