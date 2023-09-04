using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenWall : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public float fadetime;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }  

    IEnumerator FadeIn(float fadeOutTime)
    {

        Color tempColor = spriteRenderer.color;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeOutTime;
            spriteRenderer.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }
        spriteRenderer.color = tempColor;
    }

    IEnumerator FadeOut(float fadeOutTime)
    {
        Color tempColor = spriteRenderer.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeOutTime;
            spriteRenderer.color = tempColor;

            if (tempColor.a <= 0f) tempColor.a = 0f;

            yield return null;
        }
        spriteRenderer.color = tempColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(FadeOut(fadetime));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(FadeIn(fadetime));
        }
    }
}
