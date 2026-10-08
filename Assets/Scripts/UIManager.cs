using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject teacherPanel;
    public GameObject studentPanel;
    public GameObject dashboardPanel;

    public void OpenTeacher()
    {
        teacherPanel.SetActive(true);
    }

    public void OpenStudent()
    {
        studentPanel.SetActive(true);
    }

    public void OpenDashboard()
    {
        dashboardPanel.SetActive(true);
    }


    public void BackToRole()
    {
        teacherPanel.SetActive(false);
        studentPanel.SetActive(false);
        dashboardPanel.SetActive(false);
    }
}
