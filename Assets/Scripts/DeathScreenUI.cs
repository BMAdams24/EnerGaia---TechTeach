using UnityEditor.Search;
using UnityEngine;

public class DeathScreenUI : MonoBehaviour
{
    GameObject panel;

    public void Show()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Respawn()
    {
        GameManager.Instance.RespawnFromDeath();
    }

    public void Restart()
    {
        GameManager.Instance.RestartGame();
    }
}
