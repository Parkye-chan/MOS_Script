using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraCheck : MonoBehaviour
{

    CinemachineVirtualCamera cinemachine;

    private void Start()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cinemachine.enabled == false)
            cinemachine.enabled = true;
    }
}
