using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Transform robot;

    public Vector3 offset = new Vector3(0.8f, 1.2f, 0f);

    private Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && robot != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(robot.position + offset);
            dialoguePanel.transform.position = screenPos;
        }
    }

    public void ShowDialogue(string message, float duration = 5f)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        dialogueText.text = message;
        dialoguePanel.SetActive(true);

        currentRoutine = StartCoroutine(HideAfterDelay(duration));
    }

    IEnumerator HideAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        dialoguePanel.SetActive(false);
    }
}
