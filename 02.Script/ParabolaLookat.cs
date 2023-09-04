using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaLookat : MonoBehaviour
{
    public float speed = 10;
    Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();      
    }


    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 dir = rb.velocity.normalized;

        Vector2 direction = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion angleAxis = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, speed * Time.deltaTime);
         transform.rotation = rotation;
                 */

        transform.right = rb.velocity;

    }


}
