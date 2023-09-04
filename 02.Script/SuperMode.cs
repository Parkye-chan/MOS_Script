using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//(일정시간)동안 (무적 상태)가 된다
//일정시간동안 (데미지를 받지않게) 된다
public class SuperMode : MonoBehaviour
{

    [SerializeField]
    public float Time;
    public bool Use;
    SpriteRenderer SpriteRenderer;
    [SerializeField]
    SpriteRenderer[] spriteRenderers;
    [SerializeField]
    public float AlphaTime;
    bool Spark = false;
    IEnumerator Timmer()
    {
        Use = true;
        yield return new WaitForSeconds(Time);
        Use = false;
        if(SpriteRenderer)
        SpriteRenderer.color = Color.white;
        else
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = Color.white;
            }
        }
    }

    public void SetSuperMode()
    {
        StartCoroutine(Timmer());
    }

    IEnumerator SpriteTime(float time)
    {
        Spark = true;
        Color color = SpriteRenderer.color;
        if (color.a == 0)
        {
            color.a = 1;
            yield return new WaitForSeconds(time);
        }
        else
        {
            color.a = 0;
            yield return new WaitForSeconds(time);
        }
        SpriteRenderer.color = color;
        Spark = false;
    }

    IEnumerator SpritesTime(float time)
    {
        Spark = true;
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            Color color = spriteRenderers[i].color;

            if (color.a == 0)
            {
                color.a = 1;
                yield return new WaitForSeconds(time);
            }
            else
            {
                color.a = 0;
                yield return new WaitForSeconds(time);
            }
            spriteRenderers[i].color = color;
        }
        
        Spark = false;
    }

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        if(!SpriteRenderer)
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
      
        if (Use && SpriteRenderer)
        {
            if(!Spark)
            StartCoroutine(SpriteTime(AlphaTime));
        }

        else if(Use && spriteRenderers[0] != null)
        {
            if(!Spark)
            StartCoroutine(SpritesTime(AlphaTime));
        }

        if(!Spark && !Use)
        {
            if (SpriteRenderer && SpriteRenderer.color.a == 0)
            {                             
                SpriteRenderer.color = Color.white;
            }

            else if(spriteRenderers[0] != null && spriteRenderers[0].color.a == 0)
            {
                for (int i = 0; i < spriteRenderers.Length; i++)
                {
                    spriteRenderers[i].color = Color.white;
                }
            }      
        }
    }
}
