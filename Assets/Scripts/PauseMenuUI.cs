using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public void ResumeGame()
    {
        PauseManager.Instance.Resume();
    }

    public void RespawnPlayer()
    {
        GameManager.Instance.RespawnPlayerLoseHeart();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
