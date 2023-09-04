using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class TestParallax : MonoBehaviour
{

    private Camera camera;
    public Transform Player;

    Vector2 startPos;

    float startPosZ;

    Vector2 travel => (Vector2)camera.transform.position - startPos;
    

    float distanceFromSubject => transform.position.z - Player.position.z;

    float clippingPlane => (camera.transform.position.z + (distanceFromSubject > 0 ? camera.farClipPlane : camera.nearClipPlane));
    float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;

    private void Awake()
    {
        GameObject temp = GameObject.Find("Regular Camera");
        camera = temp.GetComponent<Camera>();
    }

    void Start()
    {
        if (MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter)
        Player = MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter.transform;
        
        startPos = transform.position;
        startPosZ = transform.position.z;
    }

    private void Update()
    {
        if (Player)
        {
            Vector2 newpos = startPos + travel * parallaxFactor;
            transform.position = new Vector3(newpos.x, newpos.y, startPosZ);
        }
        else
            Player = MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter.transform;
    }
}
