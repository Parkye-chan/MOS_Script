using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using UnityEngine.UI;

public class ClickerTrap : MonoBehaviour
{

    public bool Right = true;
    public Transform FirePos;
    public int MaxPool = 3;
    public GameObject Spear;
    public string TriggerParam = "Trigger";

    private bool Shooting = false;
    private Vector3 shootDir = Vector3.right;
    private GameObject objectPools;
    private List<GameObject> SpearPool = new List<GameObject>();
    private Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
        objectPools = new GameObject("ClickerObjectPools");

        for (int i = 0; i < MaxPool; i++)
        {
            GameObject objSpear = Instantiate(Spear, objectPools.transform);
            objSpear.name = "objSpear" + i.ToString("00");
            objSpear.SetActive(false);
            SpearPool.Add(objSpear);
        }

        if (Right)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            shootDir = Vector3.right;
        }
        else
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
            shootDir = Vector3.left;
        }
    }


    private void Fire()
    {
        if (!Shooting)
        {
            GameObject tempbullet;
            tempbullet = GetSpear();
            if (tempbullet != null)
            {
                tempbullet.transform.position = FirePos.position;
                tempbullet.transform.rotation = FirePos.rotation;
                tempbullet.SetActive(true);
            }
            else
            {
                AddSpear();
                tempbullet = GetSpear();
                tempbullet.transform.position = FirePos.position;
                tempbullet.transform.rotation = FirePos.rotation;
                tempbullet.SetActive(true);
            }
            tempbullet.GetComponent<Projectile>().Direction = shootDir;
            Shooting = true;
        }
        else
            return;
    }

    private GameObject GetSpear()
    {
        for (int i = 0; i < SpearPool.Count; i++)
        {
            if (SpearPool[i].activeSelf == false)
            {
                return SpearPool[i];
            }
        }
        return null;
    }

    private void AddSpear()
    {
        int num = SpearPool.Count;

        for (int i = num; i < num + MaxPool; i++)
        {
            GameObject obj = Instantiate(Spear, objectPools.transform);
            obj.name = "objSpear" + i.ToString("00");
            obj.SetActive(false);
            SpearPool.Add(obj);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _anim.SetBool(TriggerParam, true);
            Fire();
            StopAllCoroutines();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            StartCoroutine(TimeCheck());
        }
    }

    IEnumerator TimeCheck()
    {
        yield return new WaitForSeconds(3.0f);
        _anim.SetBool(TriggerParam, false);
        Shooting = false;
    }
}
