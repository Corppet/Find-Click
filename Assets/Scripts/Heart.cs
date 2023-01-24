using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Heart : MonoBehaviour
{
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite brokenHeart;

    private Image image;

    public void BreakHeart()
    {
        image.sprite = brokenHeart;
    }

    public void RestoreHeart()
    {
        image.sprite = fullHeart;
    }

    private void OnEnable()
    {
        image = GetComponent<Image>();
    }
}
