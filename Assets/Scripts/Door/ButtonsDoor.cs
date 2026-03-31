using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ButtonsDoor : DoorOpen
{
    public List<GameObject> buttons;


    // Update is called once per frame
    void Update()
    {
        int count = 0;

        foreach (GameObject button in buttons)
        {
            if (button.GetComponent<WallButtonFunction>().clicked)
                count++;
        }

        if (count == 1 && !buttons[3].GetComponent<WallButtonFunction>().clicked) buttonsRelease();
        if (count == 2 && !buttons[0].GetComponent<WallButtonFunction>().clicked) buttonsRelease();
        if (count == 3 && !buttons[2].GetComponent<WallButtonFunction>().clicked) buttonsRelease();
        if (count == 4) doorOpen();

    }

    public void buttonsRelease()
    {
        foreach (GameObject button in buttons)
            button.GetComponent<WallButtonFunction>().buttonReleased();
    }
}
