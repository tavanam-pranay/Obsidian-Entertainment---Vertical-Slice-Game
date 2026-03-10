using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateFunction : MonoBehaviour
{
    public Animator plateAnim;
    public GameObject door;

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
        plateAnim.enabled = true;
        doorOpen();
        Debug.Log("plate pressed!");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        doorClose();
        Debug.Log("plate released!");
    }

    public void doorOpen()
    {
        door.GetComponent<BoxCollider2D>().enabled = false;
        door.GetComponent<Animator>().enabled = true;
    }
    public void doorClose()
    {
        door.GetComponent<BoxCollider2D>().enabled = true;
        //animate door close
    }
}
