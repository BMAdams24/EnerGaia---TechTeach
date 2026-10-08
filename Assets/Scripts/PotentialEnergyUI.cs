using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PotentialEnergyUI : MonoBehaviour
{
    public Image fillImage;
    public PlayerController player; //Sets player to call the PlayerController skript


    void Start()
    {
        player = Object.FindFirstObjectByType<PlayerController>(); //Ensures that the UI find the Player object automatically (needed for switching scenes)
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float fillAmount = player.GetPotentialEnergyNormalized();
        fillImage.fillAmount = fillAmount;
    }
}
