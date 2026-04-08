using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private GameObject prompt;
    [SerializeField] private GameObject cursor;


    void Start()
    {
        prompt = transform.GetChild(0).gameObject;
        prompt.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.gameObject == cursor)
        {
            Debug.Log("Cursor entered tooltip trigger.");
            prompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == cursor)
        {
            prompt.SetActive(false);
        }
    }
}
