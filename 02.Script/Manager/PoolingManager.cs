using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{

    public static PoolingManager instance;


    [Header("Object Pool")]
    //총알 프리팹
    public GameObject PlayerBullet;
    public GameObject PlayerFireBullet;
    public GameObject PlayerEarthBullet;
    public GameObject PlayerIceBullet;
    public GameObject PlayerPoisonBullet; 
    public GameObject PlayerEarthChargingBullet;
    public GameObject PlayerFireChargingBullet;
    public GameObject PlayerIceChargingBullet;
    public GameObject ForestBossBullet;
    public GameObject ForestNamedBullet;
    public GameObject ArcherBullet;


    //오브젝트풀에 생성할 개수
    public int maxPool = 7;
    public List<GameObject> PlayerBulletPool = new List<GameObject>();
    public List<GameObject> PlayerFireBulletPool = new List<GameObject>();
    public List<GameObject> PlayerEarthBulletPool = new List<GameObject>();
    public List<GameObject> PlayerIceBulletPool = new List<GameObject>();
    public List<GameObject> PlayerPoisonBulletPool = new List<GameObject>();
    public List<GameObject> PlayerEarthChargingBulletPool = new List<GameObject>();
    public List<GameObject> PlayerFireChargingBulletPool = new List<GameObject>();
    public List<GameObject> PlayerIceChargingBulletPool = new List<GameObject>();
    public List<GameObject> ForestBossBulletPool = new List<GameObject>();
    public List<GameObject> ForestNamedBulletPool = new List<GameObject>();
    public List<GameObject> ArcherBullettPool = new List<GameObject>();

    private GameObject objectPools;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }


    private void Start()
    {
        CreatePooling();
    }

    public void CreatePooling()
    {
        //총알을 생성해 차일드화할 부모 오브젝트 생성
        objectPools = new GameObject("ObjectPools");

        for (int i = 0; i < maxPool; i++)
        {
            //GameObject objPlayerBullet = Instantiate(PlayerBullet, objectPools.transform);
            //GameObject objPlayerFireBullet = Instantiate(PlayerFireBullet, objectPools.transform);
            //GameObject objPlayerEarthBullet = Instantiate(PlayerEarthBullet, objectPools.transform);
            //GameObject objPlayerIceBullet = Instantiate(PlayerIceBullet, objectPools.transform);
            //GameObject objPlayerPosionBullet = Instantiate(PlayerPoisonBullet, objectPools.transform);
            //GameObject objPlayerEarthChargingBullet = Instantiate(PlayerEarthChargingBullet, objectPools.transform);
            //GameObject objForestBossBullet = Instantiate(ForestBossBullet, objectPools.transform);
            //GameObject objPlayerFireChargingBullet = Instantiate(PlayerFireChargingBullet, objectPools.transform);
            //GameObject objPlayerIceChargingBullet = Instantiate(PlayerIceChargingBullet, objectPools.transform);
            GameObject objForestNamedBullet = Instantiate(ForestNamedBullet, objectPools.transform);
            GameObject objArcherBullet = Instantiate(ArcherBullet, objectPools.transform);

            //objPlayerBullet.name = "PlayerBullet_" + i.ToString("00");
            //objPlayerFireBullet.name = "PlayerFireBullet_" + i.ToString("00");
            //objPlayerEarthBullet.name = "PlayerEarthBulletPool_" + i.ToString("00");
            //objPlayerIceBullet.name = "objPlayerIceBullet_" + i.ToString("00");
            //objPlayerPosionBullet.name = "objPlayerPosionBullet_" + i.ToString("00");
            //objPlayerEarthChargingBullet.name = "PlayerEarthChargingBullet_" + i.ToString("00");
            //objForestBossBullet.name = "ForestBossBullet" + i.ToString("00");
            //objPlayerFireChargingBullet.name = "PlayerFireChargingBullet" + i.ToString("00");
            //objPlayerIceChargingBullet.name = "objPlayerIceChargingBullet" + i.ToString("00");
            objForestNamedBullet.name = "objForestNamedBullet" + i.ToString("00");
            objArcherBullet.name = "objArcherArrow" + i.ToString("00");

            //objPlayerBullet.SetActive(false);
            //objPlayerFireBullet.SetActive(false);
            //objPlayerEarthBullet.SetActive(false);
            //objPlayerIceBullet.SetActive(false);
            //objPlayerPosionBullet.SetActive(false);
            //objPlayerEarthChargingBullet.SetActive(false);
            //objForestBossBullet.SetActive(false);
            //objPlayerFireChargingBullet.SetActive(false);
            //objPlayerIceChargingBullet.SetActive(false);
            objForestNamedBullet.SetActive(false);
            objArcherBullet.SetActive(false);

            //PlayerBulletPool.Add(objPlayerBullet);
            //PlayerFireBulletPool.Add(objPlayerFireBullet);
            //PlayerEarthBulletPool.Add(objPlayerEarthBullet);
            //PlayerIceBulletPool.Add(objPlayerIceBullet);
            //PlayerPoisonBulletPool.Add(objPlayerPosionBullet);
            //PlayerEarthChargingBulletPool.Add(objPlayerEarthChargingBullet);
            //ForestBossBulletPool.Add(objForestBossBullet);
            //PlayerFireChargingBulletPool.Add(objPlayerFireChargingBullet);
            //PlayerIceChargingBulletPool.Add(objPlayerIceChargingBullet);
            ForestNamedBulletPool.Add(objForestNamedBullet);
            ArcherBullettPool.Add(objArcherBullet);
        }
    }

    public GameObject GetPlayerBullet()
    {
        for (int i = 0; i < PlayerBulletPool.Count; i++)
        {
            if (PlayerBulletPool[i].activeSelf == false)
            {
                return PlayerBulletPool[i];
            }
        }
        return null;
    }

    public GameObject GetPlayerFireBullet()
    {
        for (int i = 0; i < PlayerFireBulletPool.Count; i++)
        {
            if (PlayerFireBulletPool[i].activeSelf == false)
            {
                return PlayerFireBulletPool[i];
            }
        }
        return null;
    }

    public GameObject GetPlayerEarthBullet()
    {
        for (int i = 0; i < PlayerEarthBulletPool.Count; i++)
        {
            if (PlayerEarthBulletPool[i].activeSelf == false)
            {
                return PlayerEarthBulletPool[i];
            }
        }
        return null;
    }

    public GameObject GetPlayerIceBullet()
    {
        for (int i = 0; i < PlayerIceBulletPool.Count; i++)
        {
            if (PlayerIceBulletPool[i].activeSelf == false)
            {
                return PlayerIceBulletPool[i];
            }
        }
        return null;
    }
    public GameObject GetPlayerPosionBullet()
    {
        for (int i = 0; i < PlayerPoisonBulletPool.Count; i++)
        {
            if (PlayerPoisonBulletPool[i].activeSelf == false)
            {
                return PlayerPoisonBulletPool[i];
            }
        }
        return null;
    }
    public GameObject GetPlayerEarthChargingBullet()
    {
        for (int i = 0; i < PlayerEarthChargingBulletPool.Count; i++)
        {
            if (PlayerEarthChargingBulletPool[i].activeSelf == false)
            {
                return PlayerEarthChargingBulletPool[i];
            }
        }
        return null;
    }
    public GameObject GetForestBossBullet()
    {
        for (int i = 0; i < ForestBossBulletPool.Count; i++)
        {
            if (ForestBossBulletPool[i].activeSelf == false)
            {
                return ForestBossBulletPool[i];
            }
        }
        return null;
    }
    public GameObject GetPlayerFireChargingBullet()
    {
        for (int i = 0; i < PlayerFireChargingBulletPool.Count; i++)
        {
            if (PlayerFireChargingBulletPool[i].activeSelf == false)
            {
                return PlayerFireChargingBulletPool[i];
            }
        }
        return null;
    }

    public GameObject GetPlayerIceChargingBullet()
    {
        for (int i = 0; i < PlayerIceChargingBulletPool.Count; i++)
        {
            if (PlayerIceChargingBulletPool[i].activeSelf == false)
            {
                return PlayerIceChargingBulletPool[i];
            }
        }
        return null;
    }

    public GameObject GetForestNamedBullet()
    {
        for (int i = 0; i < ForestNamedBulletPool.Count; i++)
        {
            if (ForestNamedBulletPool[i].activeSelf == false)
            {
                return ForestNamedBulletPool[i];
            }
        }
        return null;
    }


    public GameObject GetArrow()
    {
        for (int i = 0; i < ArcherBullettPool.Count; i++)
        {
            if (ArcherBullettPool[i].activeSelf == false)
            {
                return ArcherBullettPool[i];
            }
        }
        return null;
    }

    public void addPlayerBullet()
    {
        int num = PlayerBulletPool.Count;

        for (int i = num; i < num + maxPool; i++)
        {
            GameObject obj = Instantiate(PlayerBullet, objectPools.transform);
            obj.name = "PlayerBullet_" + i.ToString("00");
            obj.SetActive(false);
            PlayerBulletPool.Add(obj);
        }

    }

    public void addPlayerFireBullet()
    {
        int num = PlayerBulletPool.Count;

        for (int i = num; i < num + maxPool; i++)
        {
            GameObject obj = Instantiate(PlayerFireBullet, objectPools.transform);
            obj.name = "PlayerFireBullet_" + i.ToString("00");
            obj.SetActive(false);
            PlayerBulletPool.Add(obj);
        }

    }
    public void addPlayerEarthBullet()
    {
        int num = PlayerEarthBulletPool.Count;

        for (int i = num; i < num + maxPool; i++)
        {
            GameObject objPlayerEarthBullet = Instantiate(PlayerEarthBullet, objectPools.transform);
            objPlayerEarthBullet.name = "PlayerEarthBullet_" + i.ToString("00");
            objPlayerEarthBullet.SetActive(false);
            PlayerEarthBulletPool.Add(objPlayerEarthBullet);
        }
    }
    public void addPlayerIceBullet()
    {
        int num = PlayerIceBulletPool.Count;

        for (int i = num; i < num + maxPool; i++)
        {
            GameObject objPlayerIceBullet = Instantiate(PlayerIceBullet, objectPools.transform);
            objPlayerIceBullet.name = "PlyerIceBullet_" + i.ToString("00");
            objPlayerIceBullet.SetActive(false);
            PlayerIceBulletPool.Add(objPlayerIceBullet);
        }
    }

    public void addPlayerPosionBullet()
    {
        int num = PlayerPoisonBulletPool.Count;

        for (int i = num; i < num + maxPool; i++)
        {
            GameObject objPlayerPosionBullet = Instantiate(PlayerPoisonBullet, objectPools.transform);
            objPlayerPosionBullet.name = "PlayerPosionBullet_" + i.ToString("00");
            objPlayerPosionBullet.SetActive(false);
            PlayerPoisonBulletPool.Add(objPlayerPosionBullet);
        }
    }

    public void addPlayerEarthChargingBullet()
    {
        int num = PlayerEarthChargingBulletPool.Count;

        for (int i = num; i < num + maxPool; i++)
        {
            GameObject objPlayerEarthChargingBullet = Instantiate(PlayerEarthChargingBullet, objectPools.transform);
            objPlayerEarthChargingBullet.name = "PlayerEarthChargingBullet_" + i.ToString("00");
            objPlayerEarthChargingBullet.SetActive(false);
            PlayerEarthChargingBulletPool.Add(objPlayerEarthChargingBullet);
        }
    }

    public void addForestBossEffect()
    {
        int num = ForestBossBulletPool.Count;

        for (int i = num; i < num + maxPool; i++)
        {
            GameObject objForestBossBullet = Instantiate(ForestBossBullet, objectPools.transform);
            objForestBossBullet.name = "ForestBossBullet" + i.ToString("00");
            objForestBossBullet.SetActive(false);
            ForestBossBulletPool.Add(objForestBossBullet);
        }
    }

    public void addPlayerFireChargingBullet()
    {
        int num = PlayerFireChargingBulletPool.Count;

        for (int i = num; i < num + maxPool; i++)
        {
            GameObject objPlayerFireChargingBullet = Instantiate(PlayerFireChargingBullet, objectPools.transform);
            objPlayerFireChargingBullet.name = "PlayerFireChargingBullet" + i.ToString("00");
            objPlayerFireChargingBullet.SetActive(false);
            PlayerFireChargingBulletPool.Add(objPlayerFireChargingBullet);
        }
    }

    public void addPlayerIceChargingBullet()
    {
        int num = PlayerIceChargingBulletPool.Count;

        for (int i = num; i < num + maxPool; i++)
        {
            GameObject objPlayerIceChargingBullet = Instantiate(PlayerIceChargingBullet, objectPools.transform);
            objPlayerIceChargingBullet.name = "PlayerFireChargingBullet" + i.ToString("00");
            objPlayerIceChargingBullet.SetActive(false);
            PlayerIceChargingBulletPool.Add(objPlayerIceChargingBullet);
        }
    }

    public void addForestNamedBullet()
    {
        int num = ForestNamedBulletPool.Count;

        for (int i = num; i < num + maxPool; i++)
        {
            GameObject objForestNamedBullet = Instantiate(ForestNamedBullet, objectPools.transform);
            objForestNamedBullet.name = "ForestNamedBullet" + i.ToString("00");
            objForestNamedBullet.SetActive(false);
            ForestNamedBulletPool.Add(objForestNamedBullet);
        }
    }

    public void addArrow()
    {
        int num = ArcherBullettPool.Count;

        for (int i = num; i < num + maxPool; i++)
        {
            GameObject objArcherArrow = Instantiate(ArcherBullet, objectPools.transform);
            objArcherArrow.name = "objArcherArrow" + i.ToString("00");
            objArcherArrow.SetActive(false);
            ArcherBullettPool.Add(objArcherArrow);
        }
    }

}
