using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSkill : MonoBehaviour
{
    public enum ItemState
    {
        CHARM,
        FIREBALL,
        MISSILE,
        SPARK,
        BOOM
    }
    [SerializeField]
    private GameObject objCharm;
    [SerializeField]
    private GameObject objFireBall;
    [SerializeField]
    private GameObject objMissile;
    [SerializeField]
    private GameObject objSpark;
    [SerializeField]
    private GameObject objBoom;
    [SerializeField]
    private Transform FirePos;
    [SerializeField]
    private float ShootPower;

    [HideInInspector]
    public int itemCount = 0;
    public ItemState itemState = ItemState.CHARM;


    public void UseItem(Vector2 vDir)
    {
        switch (itemState)
        {
            case ItemState.CHARM:
                {
                    GameObject tempbullet;

                    //tempbullet = PoolingManager.instance.GetPlayerBullet();
                    tempbullet = Instantiate(objCharm);
                    if (tempbullet != null)
                    {
                        tempbullet.transform.position = FirePos.position;
                        tempbullet.transform.rotation = FirePos.rotation;
                        tempbullet.SetActive(true);
                    }
                    tempbullet.GetComponent<Rigidbody2D>().AddForce(ShootPower * vDir, ForceMode2D.Impulse);
                    itemCount--;
                }
                break;
            case ItemState.FIREBALL:
                {
                    //곡선으로 날아가는 불덩이 5번
                    itemCount--;
                }
                break;
            case ItemState.MISSILE:
                {
                    //추적 미사일 2개 5번
                    GameObject tempbullet;

                    //tempbullet = PoolingManager.instance.GetPlayerBullet();
                    tempbullet = Instantiate(objMissile);
                    if (tempbullet != null)
                    {
                        tempbullet.transform.position = FirePos.position;
                        tempbullet.transform.rotation = FirePos.rotation;
                        tempbullet.SetActive(true);
                    }
                    tempbullet.GetComponent<Rigidbody2D>().AddForce(ShootPower * vDir, ForceMode2D.Impulse);
                    itemCount--;
                }
                break;
            case ItemState.SPARK:
                {

                    itemCount--;
                }
                break;
            case ItemState.BOOM:
                {

                    itemCount--;
                }
                break;            
        }
    }
}
