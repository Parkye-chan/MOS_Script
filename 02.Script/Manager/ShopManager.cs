using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    GameObject PackageScroll;
    [SerializeField]
    GameObject DiamondScroll;
    [SerializeField]
    GameObject GoldScroll;
    [SerializeField]
    GameObject PurchasePanel;
    [SerializeField]
    List<GameObject> PackageBtn = new List<GameObject>();
    [SerializeField]
    List<GameObject> GoldBtn = new List<GameObject>();
    [SerializeField]
    List<GameObject> DiamondBtn = new List<GameObject>();
    [SerializeField]
    List<GameObject> ItemBtn = new List<GameObject>();


    private void KindsBtnOnOff(int btn,bool onoff)
    {
        PackageBtn[btn].SetActive(onoff);
        GoldBtn[btn].SetActive(onoff);
        DiamondBtn[btn].SetActive(onoff);
        ItemBtn[btn].SetActive(onoff);
    }

    public void OnTouchPackage()
    {
        PackageScroll.SetActive(true);
        DiamondScroll.SetActive(false);
        GoldScroll.SetActive(false);
        KindsBtnOnOff(1, false);
        PackageBtn[1].SetActive(true);

    }

    public void OnTouchDiamond()
    {
        PackageScroll.SetActive(false);
        DiamondScroll.SetActive(true);
        GoldScroll.SetActive(false);
        KindsBtnOnOff(1, false);
        DiamondBtn[1].SetActive(true);
    }

    public void OnTouchGold()
    {
        PackageScroll.SetActive(false);
        DiamondScroll.SetActive(false);
        GoldScroll.SetActive(true);
        KindsBtnOnOff(1, false);
        GoldBtn[1].SetActive(true);
    }

    public void OnTouchGoods()
    {
        PurchasePanel.SetActive(true);
    }

    public void ClosePurchasPanel()
    {
        PurchasePanel.SetActive(false);
    }

    public void OnTouchPurchase()
    {
        Debug.Log("Buy goods");
    }

}
