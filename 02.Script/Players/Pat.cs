using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pat : MonoBehaviour
{
    public float fadeOutTime = 0f;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(Pade());
    }

    IEnumerator Pade()
    {
        Color tempColor = sprite.color;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeOutTime;
            sprite.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }
        sprite.color = tempColor;
    }


}
