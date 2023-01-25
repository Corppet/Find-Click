using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance { get; private set; }

    [SerializeField] private int lives = 3;
    [Tooltip("The time interval between each difficulty increase.")]
    [SerializeField] private float difficultyRampInterval = 3f;
    [SerializeField] private KeyCode mainMenuKey = KeyCode.Escape;
    [SerializeField] private KeyCode restartKey = KeyCode.R;

    [Space(5)]

    [SerializeField] private GameObject memoryCannonsParent;
    [SerializeField] private GameObject[] memoryObjects;
    [SerializeField] private SpriteRenderer colorMask;
    [SerializeField] private Heart[] livesUI;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string menuScene = "Main Menu";

    [HideInInspector] public UnityEvent OnHit;
    [HideInInspector] public UnityEvent OnMiss;
    [HideInInspector] public MemoryType currentMemory;

    private int score;
    private MemoryCannon[] memoryCannons;

    public static void ReturnToMenu()
    {
        SceneManager.LoadScene(instance.menuScene);
    }

    public static void LoadMainScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void Hit()
    {
        score++;
    }

    private void Miss()
    {
        lives--;
        livesUI[lives].BreakHeart();
        if (lives <= 0)
            GameOver();
    }
    
    private void GameOver()
    {
        CursorController.instance.isInPlay = false;

        scoreText.text = "Score: " + score;
        gameOverPanel.SetActive(true);
    }

    private IEnumerator RampDifficulty()
    {
        yield return new WaitForSeconds(difficultyRampInterval);

        // return if the game is over
        if (!CursorController.instance.isInPlay)
            yield break;

        // increase difficulty
        foreach (MemoryCannon cannon in memoryCannons)
        {
            if (cannon.maxLaunchInterval > 0.5f)
                cannon.maxLaunchInterval -= 0.5f;
            else
                cannon.maxLaunchInterval = 0.5f;

            if (cannon.launchForce < 500f)
                cannon.launchForce += 5f;
            else
                cannon.launchForce = 500f;
        }

        StartCoroutine(RampDifficulty());
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        memoryCannons = memoryCannonsParent.GetComponentsInChildren<MemoryCannon>();
        gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        OnHit.AddListener(Hit);
        OnMiss.AddListener(Miss);

        score = 0;

        // choose a random memory type
        currentMemory = (MemoryType)Random.Range(0, 4);
        switch (currentMemory)
        {
            case MemoryType.Red:
                colorMask.color = Color.red;
                break;
            case MemoryType.Green:
                colorMask.color = Color.green;
                break;
            case MemoryType.Blue:
                colorMask.color = Color.blue;
                break;
            case MemoryType.Yellow:
                colorMask.color = Color.yellow;
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(mainMenuKey))
        {
            ReturnToMenu();
        }

        if (Input.GetKeyDown(restartKey))
        {
            LoadMainScene();
        }
    }
}
