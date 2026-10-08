using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TeacherManager : MonoBehaviour
{
    public TMP_Text joinCodeText;
    public TMPro.TMP_Text feedbackText;

    public TMP_InputField questionInput;
    public TMP_InputField[] answers;
    public TMP_Dropdown correctDropdown;

    private void Start()
    {
        GenerateJoinCode();
    }

    void GenerateJoinCode()
    {
        string code = Random.Range(1000, 9999).ToString();
        GameSession.Instance.joinCode = code;
        joinCodeText.text = "Code: " + code;
    }

    public void AddQuestion()
    {
        if (GameSession.Instance == null)
        {
            Debug.LogError("GameSession missing!");
            return;
        }

        QuestionData q = new QuestionData();

        q.questionText = questionInput.text;
        q.answers = new string[answers.Length];

        for (int i = 0; i < answers.Length; i++)
        {
            q.answers[i] = answers[i].text;
        }

        q.correctAnswerIndex = correctDropdown.value;

        GameSession.Instance.questions.Add(q);

        Debug.Log("Question Added: " + q.questionText);

        //Clear the Inputs
        questionInput.text = "";

        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].text = "";
        }

        correctDropdown.value = 0;

        //Feedback
        if (feedbackText != null)
        {
            feedbackText.text = "Question Added!";
            Invoke(nameof(ClearFeedback), 2f);
        }
    }

    void ClearFeedback()
    {
        if (feedbackText != null)
            feedbackText.text = "";
    }

    public void StartGame()
    {
        SceneManager.LoadScene("TutorialLevel");
    }
}
