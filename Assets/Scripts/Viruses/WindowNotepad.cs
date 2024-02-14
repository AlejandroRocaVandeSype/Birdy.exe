using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowNotepad : WindowBasic
{
    // QuickFix -> So OnClick function from button can be used
    public void Close()
    {
        CloseWindow();
    }

    protected override void CloseWindow()
    {
        Destroy(gameObject);

    }
}
