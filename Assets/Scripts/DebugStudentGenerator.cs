using UnityEngine;

public class DebugStudentGenerator : MonoBehaviour
{
    void Start()
    {
        if (GameSession.Instance.studentResults.Count > 0)
            return;

        AddFakeStudent("Brady");
        AddFakeStudent("Isaac");
        AddFakeStudent("Grace");
        AddFakeStudent("Lillian");
        AddFakeStudent("Katelin");
        AddFakeStudent("Siddhi");
    }

    void AddFakeStudent(string name)
    {
        StudentResult student = new StudentResult();
        student.studentName = name;

        foreach (QuestionData q in GameSession.Instance.questions)
        {
            QuestionResult r = new QuestionResult();
            r.questionText = q.questionText;
            r.correct = Random.value > 0.5f;

            student.results.Add(r);
        }

        GameSession.Instance.studentResults.Add(student);
    }
}
