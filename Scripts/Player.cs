using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
// If you use the legacy UI Text, uncomment the next line and comment out TMPro line
// using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public Transform cam;
    public float moveSpeed = 7f;
    public float movement;
    public float speed = 10;
    public bool sprint1, sprint2;

    public float sprintSpeed = 20f; // Increased speed when Shift is held
    private float currentSpeed;

    // Score
    private int score = 0;

    // UI: TextMeshPro recommended
    [Header("UI")]
    public TMP_Text scoreTextTMP;

    [Header("Audio")]
    public AudioClip coinCollectClip;        // assign in Inspector
    [Tooltip("If left empty, an AudioSource on the Player will be added at runtime.")]
    public AudioSource audioSource;          // optional to assign in Inspector

    [Header("Game Over UI (assign the GameObjects)")]
    [Tooltip("Drag the Game Over Text GameObject here (the whole GameObject, not just the Text component).")]
    public GameObject gameOverTextObject;
    [Tooltip("Drag the Retry Button GameObject here (the whole Button GameObject).")]
    public GameObject retryButtonObject;

    [Tooltip("Tag that triggers game over. Default 'Obstacle' - change to 'Ground' if needed.")]
    public string deathTag = "Obstacle";

    private bool isAlive = true;


    void Awake()
    {
        // Ensure we have an AudioSource to play sounds
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 0f; // 2D sound for UI-like pickup
                audioSource.dopplerLevel = 0f;
            }
        }

        if (gameOverTextObject != null) gameOverTextObject.SetActive(false);
        if (retryButtonObject != null) retryButtonObject.SetActive(false);

    }


    // If you want to use the legacy UI Text instead, use this:
    // public Text scoreText;

    void Start()
    {
        UpdateScoreUI();
    }

    void Update()
    {

        if (!isAlive) return;

        sprint1 = Input.GetKey(KeyCode.LeftShift);
        sprint2 = Input.GetKey(KeyCode.RightShift);
        if (sprint1 || sprint2)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        
       if (score >= 10)
       {
           sprint1 = false;
           sprint2 = false;
           currentSpeed = sprintSpeed + 0;
       }
       else if(score >=20){
           sprint1 = false;
           sprint2 = false;
           currentSpeed = sprintSpeed + 2;
       }
       else if (score >=30)
       {
           sprint1 = false;
           sprint2 = false;
           currentSpeed = sprintSpeed + 3;
       }

       


        movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0f, 0f) * Time.deltaTime * speed;
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;
        transform.Translate(cam.transform.forward * Time.deltaTime * currentSpeed);
    }

    // Use OnTriggerEnter for coin collection and ground spawn
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            // spawn next ground piece
            Object.FindAnyObjectByType<GroundSpawner>().Spawn();
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            // Play sound
            if (coinCollectClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(coinCollectClip);
            }
            else if (coinCollectClip != null)
            {
                // Fallback: play at player position
                AudioSource.PlayClipAtPoint(coinCollectClip, transform.position, 1f);
            }

            // Update score
            score += 1;
            UpdateScoreUI();

            Destroy(other.gameObject, 0f);
            Debug.Log("COIN");

        }
    }

    // Keep OnTriggerExit behavior for destructing obstacles/ground as you had
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            isAlive = false;

            if (gameOverTextObject != null) gameOverTextObject.SetActive(true);
            else Debug.LogWarning("gameOverTextObject not assigned on Player.");

            if (retryButtonObject != null) retryButtonObject.SetActive(true);
            else Debug.LogWarning("retryButtonObject not assigned on Player.");

            // Optional: freeze gameplay. Comment out if you don't want to freeze everything.
            Time.timeScale = 0f;

     
            Destroy(other.gameObject, 5f);
            Debug.Log("obj");
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(other.gameObject, 5f);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreTextTMP != null)
        {
            scoreTextTMP.text = $"Score: {score}";
        }
        // If using legacy UI Text, uncomment:
        // if (scoreText != null) scoreText.text = "Score: " + score;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

}