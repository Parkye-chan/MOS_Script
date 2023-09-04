using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicBoom : MonoBehaviour
{

    private Vector3 startPos;
    private Vector3 Dir = Vector3.left;
    private float curTime;

    public bool Left = true;
    public float Speed = 3.0f;
    public float DestroyTime = 3.0f;
    void Start()
    {
        if (Left)
            Dir = Vector3.left;
        else
            Dir = Vector3.right;

        curTime = DestroyTime;
    }

    void Update()
    {
        transform.Translate(Dir*Speed *Time.deltaTime);

        curTime -= Time.deltaTime;

        if (curTime < 0)
            gameObject.SetActive(false);
    }

}
