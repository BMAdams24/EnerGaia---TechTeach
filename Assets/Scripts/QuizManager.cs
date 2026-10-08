using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    private QuestionData currentQuestion;

    public BossController boss;

    public UnityEngine.UI.Image imageUI;

    private int wrongAttempts = 0;
    public int maxAttempts = 3;

    //Boss fight quiz UI
    public TMP_Text questionTextUI;
    public Text[] answerTexts;
    public Button[] answerButtons;


    void Start()
    {
        boss = FindFirstObjectByType<BossController>();
    }

    public void CorrectAnswer()
    {
        if (boss == null)
        {
            Debug.LogError("Boss is NULL!");
            return;
        }

        boss.ApplyQuizDamage(25f);
        wrongAttempts = 0;

        SaveResult(true);
    }

    public void WrongAnswer()
    {
        wrongAttempts++;

        SaveResult(false);

        if (wrongAttempts >= maxAttempts)
        {
            boss.EndStun();
            wrongAttempts = 0;
        }
    }

    public void LoadRandomQuestion()
    {
        if (GameSession.Instance == null || GameSession.Instance.questions.Count == 0)
        {
            Debug.LogError("NO QUESTIONS IN GAMESESSION!");
            return;
        }

        int index = Random.Range(0, GameSession.Instance.questions.Count);
        currentQuestion = GameSession.Instance.questions[index];

        Debug.Log("Loaded Question: " + currentQuestion.questionText);

        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        if (currentQuestion == null)
        {
            Debug.LogError("Current Question is NULL!");
            return;
        }

        questionTextUI.text = currentQuestion.questionText;

        // Create index list instead of shuffling strings
        List<int> answerIndices = new List<int>();
        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            answerIndices.Add(i);
        }

        Shuffle(answerIndices);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i >= answerIndices.Count) break;

            int answerIndex = answerIndices[i];

            // Set text
            TMP_Text textComponent = answerButtons[i].GetComponentInChildren<TMP_Text>();

            if (textComponent != null)
            {
                textComponent.text = currentQuestion.answers[answerIndex];
            }
            else
            {
                Debug.LogError("NO TMP TEXT FOUND ON BUTTON!");
            }

            // Clear old listeners
            answerButtons[i].onClick.RemoveAllListeners();

            // Assign correct/wrong dynamically
            if (answerIndex == currentQuestion.correctAnswerIndex)
            {
                answerButtons[i].onClick.AddListener(() => CorrectAnswer());
            }
            else
            {
                answerButtons[i].onClick.AddListener(() => WrongAnswer());
            }
        }

        // Handle image
        if (imageUI != null)
        {
            imageUI.sprite = currentQuestion.image;
            imageUI.gameObject.SetActive(currentQuestion.image != null);
        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }


    void SaveResult(bool wasCorrect)
    {
        if (GameSession.Instance == null) return;

        string playerName = PlayerPrefs.GetString("StudentName", "Player");

        StudentResult student = GameSession.Instance.studentResults
            .Find(s => s.studentName == playerName);

        if (student == null)
        {
            student = new StudentResult();
            student.studentName = playerName;
            GameSession.Instance.studentResults.Add(student);
        }

        QuestionResult result = new QuestionResult();
        result.questionText = currentQuestion.questionText;
        result.correct = wasCorrect;

        student.results.Add(result);
    }
}
