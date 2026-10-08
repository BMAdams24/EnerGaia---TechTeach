using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeacherDashboard : MonoBehaviour
{
    public Transform resultsParent;
    public GameObject resultRowPrefab;

    void ClearRows()
    {
        List<GameObject> rowsToDelete = new List<GameObject>();

        foreach (Transform child in resultsParent)
        {
            if (child.name.Contains("ResultRow"))
            {
                rowsToDelete.Add(child.gameObject);
            }
        }

        foreach (GameObject row in rowsToDelete)
        {
            Destroy(row);
        }
    }

    public void ShowByStudent()
    {
        ClearRows();

        if (GameSession.Instance == null)
        {
            Debug.LogError("GameSession missing");
            return;
        }

        foreach (StudentResult student in GameSession.Instance.studentResults)
        {
            CreateRow("Student: " + student.studentName);

            foreach (QuestionResult result in student.results)
            {
                string status = result.correct ? "Correct" : "Wrong";
                CreateRow("   • " + result.questionText + " : " + status);
            }
        }
    }

    public void ShowByQuestion()
    {
        ClearRows();

        Dictionary<string, int> correct = new Dictionary<string, int>();
        Dictionary<string, int> wrong = new Dictionary<string, int>();

        foreach (StudentResult student in GameSession.Instance.studentResults)
        {
            foreach (QuestionResult result in student.results)
            {
                if (!correct.ContainsKey(result.questionText))
                {
                    correct[result.questionText] = 0;
                    wrong[result.questionText] = 0;
                }

                if (result.correct)
                    correct[result.questionText]++;
                else
                    wrong[result.questionText]++;
            }
        }

        foreach (string question in correct.Keys)
        {
            CreateRow(question);
            CreateRow("   Correct: " + correct[question]);
            CreateRow("   Wrong: " + wrong[question]);
        }
    }

    void CreateRow(string text)
    {
        if (resultRowPrefab == null)
        {
            Debug.LogError("ResultRowPrefab missing");
            return;
        }

        GameObject row = Instantiate(resultRowPrefab, resultsParent);

        TMP_Text rowText = row.GetComponentInChildren<TMP_Text>();

        if (rowText != null)
        {
            rowText.text = text;
        }
    }
}
