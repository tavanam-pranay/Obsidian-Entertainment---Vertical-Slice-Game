using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButtonFunction : MonoBehaviour
{
    public Animator plateAnim;
    public bool inRange;
    public bool clicked;

    public void OnTriggerEnter2D(Collider2D hand)
    {
        inRange = true;
    }

    public void OnTriggerExit2D(Collider2D hand)
    {
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
