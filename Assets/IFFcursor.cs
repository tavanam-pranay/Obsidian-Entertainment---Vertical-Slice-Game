using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IFFcursor : MonoBehaviour
{
    //Based on the current IFF freq, move right.

    public TextMeshProUGUI freqMhzText; // Reference to the text that displays the current IFF frequency, used to determine how far to move the cursor

    private Vector3 leftMax; // Leftmost position of the cursor, taken at start
    private Vector3 rightMax;
    private int currentFreq; // Current IFF frequency, used to determine how far to move the cursor. 0 is the default, and 5 is the max.

    void Start()
    {
        leftMax = transform.position; // Set leftMax to the position of the cursor in-editor.
        rightMax = leftMax + new Vector3(105, 0, 0); //105 is the distance between the right and left max positions on the sprite
    }
    void Update()
    {
        currentFreq = int.Parse(freqMhzText.text);
        transform.position = Vector3.Lerp(leftMax, rightMax, (float)currentFreq / 50); // Lerp from min to max positions provided a "progress" value by the current frequency.
    }

}
