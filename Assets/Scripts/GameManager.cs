using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    public GameObject player;

    [Header("Health")]
    public int maxHearts = 5;
    public int currentHearts;

    [Header("Checkpoint")]
    public Vector3 checkpointPosition;
    public string checkpointScene;

    public string sceneToLoad = "DeathScreen";

    public bool hasBoots = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            currentHearts = maxHearts;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Health
    public void TakeDamage(int amount)
    {
        currentHearts -= amount;

        if (currentHearts <= 0)
        {
            currentHearts = 0;
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void ResetHealth()
    {
        currentHearts = maxHearts;
    }

    //Checkpoint
    public void SetCheckpoint(Vector3 position)
    {
        checkpointPosition = position;
        checkpointScene = SceneManager.GetActiveScene().name;
    }

    //Respawn (Pause menu)
    public void RespawnPlayerLoseHeart()
    {
        Time.timeScale = 1f;

        TakeDamage(1);

        SceneManager.LoadScene(checkpointScene);
        SceneManager.sceneLoaded += OnSceneLoadedRespawn;
    }

    //Respwan (Death menu)
    public void RespawnFromDeath()
    {
        Time.timeScale = 1f;

        ResetHealth();

        SceneManager.LoadScene(checkpointScene);
        SceneManager.sceneLoaded += OnSceneLoadedRespawn;
    }

    //Restart Game (Death menu)
    public void RestartGame()
    {
        Time.timeScale = 1f;

        ResetHealth();

        SceneManager.LoadScene("TutorialLevel");
        SceneManager.sceneLoaded += OnSceneLoadedStart;
    }

    //Scene load handlers
    void OnSceneLoadedRespawn(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedRespawn;

        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = checkpointPosition;
        }
    }

    void OnSceneLoadedStart(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedStart;

        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = checkpointPosition;
        }
    }
}