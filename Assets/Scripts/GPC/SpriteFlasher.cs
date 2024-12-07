using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlasher : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;

    private Coroutine _activeCR;

    private Color ogColor;
    private Sprite ogSprite;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ogColor = sr.color;
        ogSprite = sr.sprite;
    }

    public void ResetSelf()
    {
        sr.color = ogColor;
        sr.sprite = ogSprite;
    }

    public void Flash(Color color, float duration)
    {
        _activeCR = StartCoroutine(FlashColor(color, duration));
    }

    public void Flash(Sprite sprite, float duration)
    {
        _activeCR = StartCoroutine(FlashSprite(sprite, duration));
    }

    private IEnumerator FlashColor(Color color, float duration)
    {
        sr.color = color;
        yield return new WaitForSeconds(duration);
        sr.color = ogColor;
    }

    private IEnumerator FlashSprite(Sprite sprite, float duration)
    {
        sr.sprite = sprite;
        yield return new WaitForSeconds(duration);
        sr.sprite = ogSprite;
    }
}
