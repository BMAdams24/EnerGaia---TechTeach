using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StudentManager : MonoBehaviour
{
    public TMP_InputField codeInput;
    public TMP_InputField nameInput;

    public void JoinGame()
    {
        if (codeInput.text == GameSession.Instance.joinCode)
        {
            PlayerPrefs.SetString("StudentName", nameInput.text);

            Debug.Log("Joined as: " + nameInput.text);

            SceneManager.LoadScene("TutorialLevel");
        }
        else
        {
            Debug.Log("Wrong Code!");
        }
    }
}
