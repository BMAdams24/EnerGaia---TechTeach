using UnityEngine;

public class BootsPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                player.hasBoots = true;

                Debug.Log("Boots Equipped! Boost Unlocked!");

                Destroy(gameObject); // remove boots from world

                GameManager.Instance.hasBoots = true;
                player.UnlockBoots();
            }
        }
    }
}
