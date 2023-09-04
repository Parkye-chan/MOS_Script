using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class StageInfo : MonoBehaviour
{
    public List<Room> rooms = new List<Room>();
    public List<bool> Visit = new List<bool>();
    public List<BossZone> bosszone;
    public List<bool> BossClear = new List<bool>();
    public List<ObjectData> objects = new List<ObjectData>();
    public List<bool> Active = new List<bool>();

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            Visit.Add(rooms[i].RoomVisited);
        }
        for (int i = 0; i < bosszone.Count; i++)
        {
            BossClear.Add(bosszone[i].Clear);
        }

        if (objects.Count > 0)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                Active.Add(objects[i].isActive);
            }
        }
    }

    public void Updatedata()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            //rooms[i].RoomVisited = Visit[i];
            Visit[i] = rooms[i].RoomVisited;
        }
        for (int i = 0; i < bosszone.Count; i++)
        {
            //bosszone[i].Clear = BossClear[i];
            BossClear[i] = bosszone[i].Clear;
        }

        for (int i = 0; i < objects.Count; i++)
        {
            Active[i] = objects[i].isActive;
        }
    }

    public void LoadInit()
    {
        for (int i = 0; i < Visit.Count; i++)
        {
            rooms[i].RoomVisited = Visit[i];
        }

        for (int i = 0; i < BossClear.Count; i++)
        {
            bosszone[i].Clear = BossClear[i];
            bosszone[i].ClearCheck();
        }


        for (int i = 0; i < Active.Count; i++)
        {
            objects[i].isActive = Active[i];
            objects[i].Active();
        }

    }
}
