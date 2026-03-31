using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlateFunction : MonoBehaviour
{
    public Animator plateAnim;
    public bool pressed;
    public List<Collider2D> objectsOnTop = new List<Collider2D>();

    // attach player to disqualify magnet hitbox from activating plate
    public GameObject player;
    Collider2D beamCollider;

    // Start is called before the first frame update
    void Start()
    {
        beamCollider = player.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other != beamCollider && !other.CompareTag("Keycard"))
        {
            if (!objectsOnTop.Contains(other))
            {
                objectsOnTop.Add(other);
            }

            //trigger the press with the first object on top
            if (objectsOnTop.Count == 1)
            {
                plateAnim.SetBool("on", true);
                plateAnim.SetBool("off", false);
                pressed = true;
                Debug.Log("plate pressed!");
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other != beamCollider && !other.CompareTag("Keycard"))
        {
            if (objectsOnTop.Contains(other))
            {
                objectsOnTop.Remove(other);
            }

            //only release plate if there's nothing on top
            if (objectsOnTop.Count == 0)
            {
                plateAnim.SetBool("off", true);
                plateAnim.SetBool("on", false);
                pressed = false;
                Debug.Log("plate released!");
            }
        }
    }
}
