using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool setCheckpoint = true;
    public bool triggerDialogue = false;
    public string dialogueMessage;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;

        if (collision.CompareTag("Player"))
        {
            if (setCheckpoint)
            {
                GameManager.Instance.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint Set");
            }

            if (triggerDialogue && DialogueManager.Instance != null)
            {
                DialogueManager.Instance.ShowDialogue(dialogueMessage);
            }

            hasTriggered = true;
        }
    }
}
