using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmExtend : MonoBehaviour
{
    public float extendSpeed = 10f;
    public float maxReach = 3f;
    
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            float newX = Mathf.MoveTowards(transform.localScale.x, maxReach, extendSpeed * Time.deltaTime);
            transform.localScale = new Vector3(newX, originalScale.y, originalScale.z);
        }
        
        else
        {
            float newX = Mathf.MoveTowards(transform.localScale.x, originalScale.x, extendSpeed * Time.deltaTime);
            transform.localScale = new Vector3(newX, originalScale.y, originalScale.z);
        }
    }
}
