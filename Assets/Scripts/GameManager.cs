using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Preview,
    Play
}

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance { get; private set; }

    [Header("Play Window Properties")]
    [SerializeField] private PlayWindowBorders playWindowBorders;
    [SerializeField] private int horizontalSegmentCount;
    [SerializeField] private int verticalSegmentCount;

    [Space(10)]

    public UnityEvent OnHit;
    public UnityEvent OnMiss;

    [SerializeField] private GameObject[] memoryObjects;
    [SerializeField] private Camera mainCamera;

    [HideInInspector] public GameState gameState;

    private void HitTest()
    {
        Debug.Log("Hit");
    }

    private void MissTest()
    {
        Debug.Log("Miss");
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
    }

    private void Start()
    {
        OnHit.AddListener(HitTest);
        OnMiss.AddListener(MissTest);
    }
}

[System.Serializable]
public struct PlayWindowBorders
{
    public float left;
    public float right;
    public float top;
    public float bottom;
}

public class PlaySegment
{
    Vector3 startPos; // bottom left corner of the segment
    float width;
    float height;
}