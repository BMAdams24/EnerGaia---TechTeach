using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;

    public PlayerHealth player;

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < player.currentHearts)
            {
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true;
            }

            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
