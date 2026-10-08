using UnityEngine;

public class FallZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) //If the player falls in the FallZone, respawn at checkpoint and lose a heart
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.RespawnPlayerLoseHeart();
        }
    }
}
