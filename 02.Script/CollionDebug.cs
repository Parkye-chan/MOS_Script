using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollionDebug : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("enter"+collision.gameObject.name);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("stay"+collision.gameObject.name);
    }
}
