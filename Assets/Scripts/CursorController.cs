using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CursorController : MonoBehaviour
{
    [HideInInspector] public static CursorController instance { get; private set; }

    [Header("Cursor Settings")]
    [SerializeField] private GameObject cursorPrefab;
    public Color trailColor = new Color(1, 0, 0.38f);
    public float distanceFromCamera = 5;
    public float startWidth = 0.1f;
    public float endWidth = 0f;
    public float trailTime = 0.24f;

    [Space(5)]

    [SerializeField] private AudioClip[] hitSelectSounds;
    [SerializeField] private AudioClip[] hitDeleteSounds;
    [SerializeField] private AudioClip[] missSounds;

    [Space(5)]

    [SerializeField] private Canvas canvas;

    [HideInInspector] public bool isInPlay;

    private GameManager gameManager;
    private Transform cursor;
    private AudioSource audioSource;

    public void PlayHitSelect()
    {
        audioSource.PlayOneShot(hitSelectSounds[Random.Range(0, hitSelectSounds.Length)]);
    }

    public void PlayHitDelete()
    {
        audioSource.PlayOneShot(hitDeleteSounds[Random.Range(0, hitDeleteSounds.Length)]);
    }

    public void PlayMiss()
    {
        audioSource.PlayOneShot(missSounds[Random.Range(0, missSounds.Length)]);
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

        isInPlay = true;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        gameManager.OnMiss.AddListener(PlayMiss);

        // hide default cursor and instantiate cursor prefab
        Cursor.visible = false;
        cursor = Instantiate(cursorPrefab, canvas.transform).transform;
    }

    private void Update()
    {
        // move custom cursor
        cursor.position = Input.mousePosition;

        if (!isInPlay)
            return;

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // select object
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D target = Physics2D.OverlapPoint(cursorPosition);
            if (target && target.CompareTag("Memory"))
            {
                Memory memory = target.GetComponent<Memory>();
                if (memory.memoryType == gameManager.currentMemory)
                {
                    gameManager.OnHit.Invoke();
                    PlayHitSelect();
                }
                else
                {
                    gameManager.OnMiss.Invoke();
                }
                Destroy(target.gameObject);
            }
            else
            {
                gameManager.OnMiss.Invoke();
            }
        }
        
        // delete non-current memory objects
        if (Input.GetMouseButtonDown(1))
        {
            Collider2D target = Physics2D.OverlapPoint(cursorPosition);
            if (target && target.CompareTag("Memory"))
            {
                Memory memory = target.GetComponent<Memory>();
                if (memory.memoryType != gameManager.currentMemory)
                {
                    gameManager.OnHit.Invoke();
                    PlayHitDelete();
                }
                else
                {
                    gameManager.OnMiss.Invoke();
                }
                Destroy(target.gameObject);
            }
            else
            {
                gameManager.OnMiss.Invoke();
            }
        }
    }
}
