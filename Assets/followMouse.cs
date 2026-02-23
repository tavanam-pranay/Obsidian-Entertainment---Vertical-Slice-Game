using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMouse : MonoBehaviour
{

    [Range(1f, 10f)]
    public float speed = 5f; // Speed of move to target
    Vector2 restPos; // Position the target will back lerp to
    Vector2 restOffset; // Offset from the parent object to the target's rest position





    void Start()
    {
        if (transform.parent == null)
        {
            Debug.LogError("The target requires a parent object to exist offset to.");
            enabled = false; // Disable the script if no parent is found
            return;
        }

        restOffset = (Vector2)(transform.position - transform.parent.position);
        restPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        restPos = (Vector2)transform.parent.position + restOffset;
        //When E is pressed, the object will lerp to the mouse position.
        if (Input.GetKey(KeyCode.E))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, restPos, Time.deltaTime * speed);
        }
    }
}
