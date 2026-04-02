using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ButtonsDoor : DoorOpen
{
    public List<GameObject> buttons;


    // Update is called once per frame
    async Task Update()
    {
        int count = 0;

        foreach (GameObject button in buttons)
        {
            if (button.GetComponent<WallButtonFunction>().clicked)
                count++;
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            if (count == buttons.Count) doorOpen();
            if (count == i + 1 && !buttons[i].GetComponent<WallButtonFunction>().clicked)
            {
                await Task.Delay(100);
                buttonsRelease();
            }
        }

    }

    public void buttonsRelease()
    {
        foreach (GameObject button in buttons)
            button.GetComponent<WallButtonFunction>().buttonReleased();
    }
}
