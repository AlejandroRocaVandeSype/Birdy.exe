using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowError : WindowBasic
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This window only appears once. When it is closed the game actually starts
    protected override void CloseWindow()
    {
        Destroy(gameObject);
        GameManager.Instance.gameStage = GameManager.GameStage.Gameplay;

    }
}
