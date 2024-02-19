

public class WindowNotepad : WindowBasic
{
    // QuickFix -> This way it can be called from the button
    public void Close()
    {
        CloseWindow();
    }


    protected override void CloseWindow()
    {
        Destroy(gameObject);
    }
}
