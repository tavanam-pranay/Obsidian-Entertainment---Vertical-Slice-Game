using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D circleCursor;
    [SerializeField] private Texture2D squareCursor;
    [SerializeField] private Texture2D aimCursor;

    private Vector2 cursorCenter;

    void Start()
    {
        cursorCenter = new Vector2(circleCursor.width / 2, circleCursor.height / 2);

        Cursor.SetCursor(circleCursor, cursorCenter, CursorMode.Auto);

    }

    public void SetCircleCursor()
    {
        Cursor.SetCursor(circleCursor, cursorCenter, CursorMode.Auto);
    }
    public void SetSquareCursor()
    {
        Cursor.SetCursor(squareCursor, cursorCenter, CursorMode.Auto);
    }
    public void SetAimCursor()
    {
        Cursor.SetCursor(aimCursor, cursorCenter, CursorMode.Auto);
    }
}
