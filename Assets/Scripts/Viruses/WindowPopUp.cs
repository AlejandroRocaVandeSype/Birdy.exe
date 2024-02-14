using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowPopUp : WindowBasic
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
