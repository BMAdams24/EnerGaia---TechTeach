using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    public PlayerController player;

    public TMPro.TextMeshProUGUI cooldownText;


    void Start()
    {
        cooldownText.enabled = false;
    }

    void Update()
    {
        if (player.GetCooldownNormalized() > 0)
        {
            cooldownText.enabled = true;
        }

        else
        {
            cooldownText.enabled = false;
        }
        
        cooldownText.text = Mathf.Ceil(player.GetCooldownNormalized() * player.maxCooldown) + "s";
    }
}
