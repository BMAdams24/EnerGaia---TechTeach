using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    private GameObject pauseMenuUI;
    private bool isPaused = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        FindPauseMenu();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPauseMenu();

        Time.timeScale = 1f;
        isPaused = false;
    }

    void FindPauseMenu()
    {
        PauseMenuTag tag = FindAnyObjectByType<PauseMenuTag>(FindObjectsInactive.Include);

        if (tag != null)
        {
            pauseMenuUI = tag.gameObject;
            pauseMenuUI.SetActive(false);
        }
        else
        {
            Debug.LogError("PauseMenu not found in scene: " + SceneManager.GetActiveScene().name);
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;
        if (pauseMenuUI == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
