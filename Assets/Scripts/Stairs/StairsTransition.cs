using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTransition : MonoBehaviour
{
    public GameObject player;
    public GameObject otherStairs;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D playerCol)
    {
        if (playerCol.CompareTag("Player"))
        {
            Debug.Log("player has hit transition");
            player.transform.position = otherStairs.GetComponent<BoxCollider2D>().transform.position - offset;
        }
    }
}
