using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public Collider2D blockingCollider; //Non-Trigger
    public Collider2D triggerCollider; //Trigger

    [Header("Sprites")]
    public Sprite closedSprite;
    public Sprite openSprite;

    private SpriteRenderer sr;

    public string sceneToLoad = "BossArena";


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        CloseDoor();
    }


    public void OpenDoor()
    {
        blockingCollider.enabled = false;
        triggerCollider.enabled = true;
        sr.sprite = openSprite;

    }


    public void CloseDoor()
    {
        blockingCollider.enabled = true;
        triggerCollider.enabled = false;
        sr.sprite = closedSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something entered door trigger");

        if (collision.GetComponentInParent<PlayerController>() != null) //Does the collider belong to something that has the PlayerController on it? It's more reliable than using Tags such as "Player"
        {
            Debug.Log("Player detected — loading scene");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
