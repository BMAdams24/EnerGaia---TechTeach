using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int currentHearts;

    public float invulnerabilityTime = 3f;
    private bool isInvulnerable = false;


    void Start()
    {
        currentHearts = GameManager.Instance.currentHearts;
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable)
        {
            return;
        }

        GameManager.Instance.TakeDamage(amount);
        currentHearts = GameManager.Instance.currentHearts;

        StartCoroutine(Invulnerability()); //Calls the Invulnerability function
    }

    private System.Collections.IEnumerator Invulnerability() //Funtion for invulnerability after being hit. turns it on, sets timer based on invulnerability time, then turns it off
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }
}
