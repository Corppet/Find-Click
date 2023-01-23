using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [HideInInspector] public static CursorController instance { get; private set; }
    [HideInInspector] public bool isInPlay;

    private Vector3 cursorPosition;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        isInPlay = true;
    }

    private void Update()
    {
        if (!isInPlay)
            return;

        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // select object
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            Collider2D target = Physics2D.OverlapPoint(cursorPosition);
            if (target && target.CompareTag("Memory"))
                GameManager.instance.OnHit.Invoke();
            else
                GameManager.instance.OnMiss.Invoke();
        }
    }
}
