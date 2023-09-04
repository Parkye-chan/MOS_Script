using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRespawners : MonoBehaviour
{
    public TrapRoomRespawner[] respawns;

    private void Start()
    {
        respawns = GetComponentsInChildren<TrapRoomRespawner>();
    }
}
