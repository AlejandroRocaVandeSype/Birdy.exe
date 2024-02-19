
public class WindowError : WindowBasic
{
    void Start()
    {
        // Everytime a window of this type is created make the sound
        SoundManager.Instance.PlaySound("ErrorSound", false);
    }


    protected override void CloseWindow()
    {
        Destroy(gameObject);

        if(GameManager.Instance.gameStage != GameManager.GameStage.Gameplay)
            GameManager.Instance.gameStage = GameManager.GameStage.Gameplay;

    }
}
