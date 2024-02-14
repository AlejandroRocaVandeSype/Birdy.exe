using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class WindowPopUp : WindowBasic
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
