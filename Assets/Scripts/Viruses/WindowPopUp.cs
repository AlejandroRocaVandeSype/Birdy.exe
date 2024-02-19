
using UnityEngine;

public class WindowPopUp : WindowBasic
{

    public void Start()
    {
        SoundManager.Instance.PlaySound("WindowPopUp", false);
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

    // Spawn a virus next to the button
    public void SpawnButton()
    {
        Vector3 virusSpawnPos = transform.position;
        virusSpawnPos.x += 2f;
        GameManager.Instance.SpawnManager.Spawn(virusSpawnPos);
    }
}
