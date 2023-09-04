using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class SpreadTrap : MonoBehaviour
{

    [SerializeField]
    protected Transform[] FirePos;
    protected float curTiem = 0;
    protected int FireCnt = 0;
    protected Vector3 startPos;
    protected Vector3 curPos;
    public bool StartFire = false;
    protected Projectile projectile;
    private List<GameObject> BulletPool = new List<GameObject>();
    private GameObject objectPools;

    public float StopDist = 6.0f;
    public GameObject Bullet;
    public float FireTime = 2.0f;
    public int MaxBullet = 15;
    public MMFeedbacks ShootFeedback;

    private void OnEnable()
    {
        startPos = transform.position;
    }

    private void OnDisable()
    {
        startPos = Vector3.zero;
        curPos = Vector3.zero;
        FireCnt = 0;
        StartFire = false;
        curTiem = 0;
    }

    void Start()
    {
        FirePos = GetComponentsInChildren<Transform>();
        projectile = GetComponent<Projectile>();
        objectPools = new GameObject("ObjectPools");
        startPos = transform.position;

        for (int i = 0; i < MaxBullet; i++)
        {
            GameObject objBullet = Instantiate(Bullet, objectPools.transform);
            objBullet.name = "objBullet" + i.ToString("00");
            objBullet.SetActive(false);
            BulletPool.Add(objBullet);
            objectPools.transform.parent = transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {       
        if (!StartFire)
            StopProcess();
        else
            FireFunc();
    }
    
    protected void StopProcess()
    {
        curPos = transform.position;
        float dist = Vector3.Distance(startPos, curPos);
        if (dist >= StopDist)
        {
            StartFire = true;
            projectile.Direction = Vector3.zero;
        }
    }

    private GameObject GetBullet()
    {
        for (int i = 0; i < BulletPool.Count; i++)
        {
            if (BulletPool[i].activeSelf == false)
            {
                return BulletPool[i];
            }
        }
        return null;
    }

    private void AddBullet()
    {
        int num = BulletPool.Count;

        for (int i = num; i < num + MaxBullet; i++)
        {
            GameObject obj = Instantiate(Bullet, objectPools.transform);
            obj.name = "objBullet" + i.ToString("00");
            obj.SetActive(false);
            BulletPool.Add(obj);
        }
    }

    protected void FireFunc()
    {

        if (curTiem <= 0 && FireCnt < 3)
        {
            if(ShootFeedback)
                ShootFeedback.PlayFeedbacks();

            for (int i = 1; i < FirePos.Length; i++)
            {
                if (FirePos[i] == null)
                    continue;
                else
                {
                    GameObject tempBullet = GetBullet();

                    if (tempBullet == null)
                    {
                        AddBullet();
                        tempBullet = GetBullet();
                    }

                    Vector3 Dir = Vector3.Normalize(FirePos[i].position - transform.position);
                    // BulletStorage[i] = Instantiate(Bullet, FirePos[i].position, FirePos[i].rotation);
                    Projectile bullet = tempBullet.GetComponent<Projectile>();
                    bullet.Direction = Dir;
                    bullet.transform.position = FirePos[i].position;
                    bullet.transform.rotation = Quaternion.Euler(0, 0, FirePos[i].rotation.eulerAngles.z);
                    bullet.gameObject.SetActive(true);                   
                    curTiem = FireTime;

                }
            }
            FireCnt++;

            if (FireCnt == 3)
                StartCoroutine(destroythis());
        }
        else
            curTiem -= Time.deltaTime;
    }

    IEnumerator destroythis()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }
}
