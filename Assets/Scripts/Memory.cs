using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MemoryType
{
    Red,
    Green,
    Blue,
    Yellow
}

public class Memory : MonoBehaviour
{
    public MemoryType memoryType;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public void RestoreColor()
    {
        spriteRenderer.color = originalColor;
    }

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
}
