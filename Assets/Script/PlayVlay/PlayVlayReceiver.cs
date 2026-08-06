using UnityEngine;

public class PlayVlayReceiver : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    void Start()
    {
        PlayVlayBridge.RegisterCallbacks();

        PlayVlayBridge.Ready();
    }

    public void OnInit(string json)
    {
        Debug.Log("Init : " + json);

        // Parse JSON if needed
    }

    public void OnStart(string value)
    {
        Debug.Log("Start");

        gameController.StartGame();
    }

    public void OnPause(string value)
    {
        Debug.Log("Pause");

        gameController.PauseGame();
    }

    public void OnResume(string value)
    {
        Debug.Log("Resume");

        gameController.ResumeGame();
    }

    public void OnRestart(string value)
    {
        Debug.Log("Restart");

        gameController.RestartGame();
    }

    public void OnSetMuted(string value)
    {
        bool muted = value == "1";

        AudioListener.volume = muted ? 0 : 1;
    }
}