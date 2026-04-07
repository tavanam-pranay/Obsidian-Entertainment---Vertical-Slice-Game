using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButtonFunction : MonoBehaviour
{
    //public GameObject player;
    //public Collider2D beamCollider;
    public Animator plateAnim;
    public bool inRange;
    public bool clicked;

    void Start()
    {

        //if (beamCollider == null)
        //{
        //    Debug.Log("Beam collider not assigned");
        //    beamCollider = player.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponent<Collider2D>(); //Pranay's original code, unsure if works with new player prefab
        //}
    }

    public void OnTriggerEnter2D(Collider2D hand)
    {
        //if (hand != beamCollider)
            inRange = true;
    }

    public void OnTriggerExit2D(Collider2D hand)
    {
        //if (hand != beamCollider)
            inRange = false;
    }

    void Update()
    {
        if (inRange && Input.GetMouseButtonDown(0))
        {
            buttonClicked();
        }
    }

    public void buttonClicked()
    {
        clicked = true;
        plateAnim.SetBool("on", true);
        plateAnim.SetBool("off", false);
    }

    public void buttonReleased()
    {
        clicked = false;
        plateAnim.SetBool("on", false);
        plateAnim.SetBool("off", true);
    }
}
