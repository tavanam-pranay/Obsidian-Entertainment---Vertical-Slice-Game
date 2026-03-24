using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateFunction : MonoBehaviour
{
    public Animator plateAnim;
    public GameObject door;
    public List<Collider2D> objectsOnTop = new List<Collider2D>();

    // attach player's magnet collider as parameter to disqualify magnet hitbox from activating plate
    public Collider2D beamCollider;

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
        if (other != beamCollider)
        {
            if (!objectsOnTop.Contains(other))
            {
                objectsOnTop.Add(other);
            }

            //trigger the press with the first object on top
            if (objectsOnTop.Count == 1)
            {
                plateAnim.SetBool("On", true);
                plateAnim.SetBool("Off", false);
                doorOpen();
                Debug.Log("plate pressed!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != beamCollider)
        {
            if (objectsOnTop.Contains(other))
            {
                objectsOnTop.Remove(other);
            }

            //only release plate if there's nothing on top
            if (objectsOnTop.Count == 0)
            {
                plateAnim.SetBool("On", false);
                plateAnim.SetBool("Off", true);
                doorClose();
                Debug.Log("plate released!");
                //animate plate release
            }
        }
    }

    public void doorOpen()
    {
        door.GetComponent<BoxCollider2D>().enabled = false;
        door.GetComponent<Animator>().SetBool("open", true);
        door.GetComponent<Animator>().SetBool("close", false);
    }
    public void doorClose()
    {
        door.GetComponent<BoxCollider2D>().enabled = true;
        door.GetComponent<Animator>().SetBool("open", false);
        door.GetComponent<Animator>().SetBool("close", true);
    }
}
