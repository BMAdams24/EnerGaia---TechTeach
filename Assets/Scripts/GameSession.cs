using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;

    public string joinCode;

    public List<StudentResult> studentResults = new List<StudentResult>();
    public List<QuestionData> questions = new List<QuestionData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class StudentResult
{
    public string studentName;
    public List<QuestionResult> results = new List<QuestionResult>();
}

[System.Serializable]
public class QuestionResult
{
    public string questionText;
    public bool correct;
}
