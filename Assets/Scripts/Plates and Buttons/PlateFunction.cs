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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Keycard"))
        {
            if (!objectsOnTop.Contains(other))
            {
                objectsOnTop.Add(other);
            }

            if (objectsOnTop.Count == 1)
            {
                plateAnim.SetBool("on", true);
                plateAnim.SetBool("off", false);
                pressed = true;

                FindObjectOfType<AudioManager>().PlayClick();

                Debug.Log("plate pressed!");
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Keycard"))
        {
            if (objectsOnTop.Contains(other))
            {
                objectsOnTop.Remove(other);
            }

            if (objectsOnTop.Count == 0)
            {
                plateAnim.SetBool("off", true);
                plateAnim.SetBool("on", false);
                pressed = false;

                FindObjectOfType<AudioManager>().PlayCancel();

                Debug.Log("plate released!");
            }
        }
    }
}