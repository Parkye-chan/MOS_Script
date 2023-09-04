using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shiled : MonoBehaviour
{

    public GameObject Planet;       
    public float speed;


    public void OrbitAround()
    {
        transform.RotateAround(Planet.transform.position, Vector3.forward, speed * Time.deltaTime);
    }
}

