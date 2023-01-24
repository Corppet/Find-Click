using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMask : MonoBehaviour
{
    [SerializeField] private Color maskColor;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Memory"))
        {
            collision.GetComponent<SpriteRenderer>().color = maskColor;
        }
    }
}
