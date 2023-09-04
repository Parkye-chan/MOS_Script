using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public enum SlotState
    {
        ABILITY,
        ETC,
        PASSIVE,
        ELEMENTAL,
        EQITMENT
    }


    public Image itemImage; //아이템의 이미지
    public Image SelectImage; //선택글로우
    public Image CurImage; // 큰 슬롯
    public Sprite BigImage; //장착슬롯용 이미지
    public bool isGet = false;
    public Slot[] EqitSlots;
    public Image[] EqitImage;
    public Text ItemName;
    public Text ItemDiscrion;
    public Text testTxt;
    public Button EqitPanel;
    public Button UnEqitPanel;
    public bool isEqit = false;
    public SlotState slotState;
    public Text Testtxt;
    
    private SlotData slotData;
    private string curItemName;
    private string curItemDiscrion;

    private void Start()
    {

        Init();
    }

    public void SelectOn()
    {
        SelectImage.gameObject.SetActive(true);
    }

    public void SelectOff()
    {
        SelectImage.gameObject.SetActive(false);
    }

    public void ItemImageOff()
    {
        itemImage.gameObject.SetActive(false);
    }

    public void ItemImageOn()
    {
        itemImage.gameObject.SetActive(true);
    }

    public void ShowAbillity()
    {
        if (!isGet)
        {
            if (ItemName)
            {
                ItemName.text = "????";
            }

            if (ItemDiscrion)
            {
                ItemDiscrion.text = "아직 얻지 못한 아이템 입니다";
            }
        }
        else
        {
            if (ItemName)
            {
                if (curItemName == "")
                {
                    curItemName = slotData.ItemName;
                }

                ItemName.text = curItemName;
            }
            if(testTxt)
            {
                testTxt.text = curItemName;
            }
        }
    }

    public void ShowInventoryImage()
    {
        if (!itemImage.gameObject.activeSelf)
            return;
        else
        {
            CurImage.sprite = itemImage.sprite;
            CurImage.color = Color.white;
        }
    }

    public void ShowPassiveImage()
    {

        if (!itemImage.gameObject.activeSelf)
            return;

        CurImage.sprite = itemImage.sprite;
        CurImage.color = Color.white;
        if (ItemName)
        {
            ItemName.text = curItemName;
        }

        if (ItemDiscrion)
        {
            ItemDiscrion.text = curItemDiscrion;
        }
    }

    public void EqitPassive()
    {
        if (!isGet)
            return;

        if(isEqit)
        {
            
            isEqit = false;
            EqitPanel.interactable = true;
            UnEqitPanel.interactable = false;
            PlayerInfoManager.instance.PassiveEqit(slotData.ItemCode, isEqit);
            for (int i = 0; i < EqitSlots.Length; i++)
            {
                if(EqitImage[i].sprite == itemImage.sprite || EqitImage[i].sprite == BigImage)
                {
                    EqitImage[i].sprite = null;
                    EqitImage[i].color = Color.clear;
                    EqitSlots[i].isEqit = false;
                    EqitSlots[i].GetComponent<SlotData>().ItemCode = null;
                    //return;
                }
            }
        }
        else
        {
            if (EqitFullCheck() && !EqitCheck(slotData.ItemCode))
                return;

            isEqit = true;
            EqitPanel.interactable = false;
            UnEqitPanel.interactable = true;
            PlayerInfoManager.instance.PassiveEqit(slotData.ItemCode, isEqit);
            for (int i = 0; i < EqitSlots.Length; i++)
            {
                if (EqitImage[i].sprite == null)
                {
                    EqitImage[i].sprite = BigImage;
                    EqitImage[i].color = Color.white;
                    EqitSlots[i].isEqit = true;
                    EqitSlots[i].GetComponent<SlotData>().ItemCode = slotData.ItemCode;
                    return;
                }
            }
        }
    }

    public void NoShowPassiveImage()
    {
        CurImage.sprite = null;
        CurImage.color = Color.clear;
        if (ItemName)
        {
            ItemName.text = "????";
        }

        if (ItemDiscrion)
        {
            ItemDiscrion.text = "아직 얻지 못한 아이템 입니다";
        }
    }

    public void NoShowInventoryImageOff()
    {

            CurImage.sprite = null;
            CurImage.color = Color.clear;
    }

    public void EqitSlotCheck()
    {

        for (int i = 0; i < EqitSlots.Length; i++)
        {
            if (EqitCheck(slotData.ItemCode))
            {
                EqitPanel.interactable = false;
                UnEqitPanel.interactable = true;
                return;
            }
            else if (EqitFullCheck() && !EqitCheck(slotData.ItemCode))
            {
                EqitPanel.interactable = false;
                UnEqitPanel.interactable = false;
            }
            else if(!EqitFullCheck() && !EqitCheck(slotData.ItemCode))
            {
                EqitPanel.interactable = true;
                UnEqitPanel.interactable = false;
            }
        }
    }


    private bool EqitFullCheck()
    {
        for (int i = 0; i < EqitImage.Length; i++)
        {
            if (EqitImage[i].sprite == null)
                return false;
            else
                continue;
        }
        return true;
    }
    
    private bool EqitCheck(string itemcode)
    {
        for (int i = 0; i < EqitSlots.Length; i++)
        {
            if (EqitSlots[i].GetComponent<SlotData>().ItemCode == itemcode)
                return true;
            else
                continue;
        }
        return false;
    }

    public void GetItem()
    {
        isGet = true;

        if (itemImage)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.color = Color.white;
            for (int i = 0; i < Iconstorage.instance.SmallIcons.Count; i++)
            {
                if (Iconstorage.instance.SmallIcons[i].name == slotData.ImageCode)
                    itemImage.sprite = Iconstorage.instance.SmallIcons[i];
            }
            for (int i = 0; i < Iconstorage.instance.BigIcons.Count; i++)
            {
                if (Iconstorage.instance.BigIcons[i].name == slotData.ImageCode2)
                    BigImage = Iconstorage.instance.BigIcons[i];
            }
        }

        switch (slotState)
        {
            case SlotState.ABILITY:
                {
                    PlayerInfoManager.instance.SkillGetSave(slotData.ItemCode);
                }
                break;
            case SlotState.ETC:
                {

                }
                break;
            case SlotState.PASSIVE:
                {

                }
                break;
            case SlotState.ELEMENTAL:
                {

                }
                break;
        }
    }

    public void InitGetItem()
    {
        itemImage.gameObject.SetActive(true);
        itemImage.color = Color.white;
        
        if (ItemName)
        {
            ItemName.text = curItemName;
        }

        if(testTxt)
        {
            testTxt.text = curItemName;
        }

        for (int i = 0; i < Iconstorage.instance.SmallIcons.Count; i++)
        {
            if (Iconstorage.instance.SmallIcons[i].name == slotData.ImageCode)
                itemImage.sprite = Iconstorage.instance.SmallIcons[i];
        }
        for (int i = 0; i < Iconstorage.instance.BigIcons.Count; i++)
        {
            if (Iconstorage.instance.BigIcons[i].name == slotData.ImageCode2)
                BigImage = Iconstorage.instance.BigIcons[i];
        }
    }
    public void Test()
    {
        if (Testtxt)
        {
            slotData = GetComponent<SlotData>();
            Testtxt.text = slotData.ItemName;
        }
    }

    public void Init()
    {
        slotData = GetComponent<SlotData>();
        curItemName = slotData.ItemName;
        curItemDiscrion = slotData.Description;

        if (!isGet)
        {
            if (itemImage)
            {
                itemImage.gameObject.SetActive(false);
                itemImage.color = Color.clear;
            }
            if (ItemName)
                ItemName.text = "????";
            if (ItemDiscrion)
                ItemDiscrion.text = "아직 얻지 못한 아이템 입니다";
        }
        else
        {
            if (itemImage)
                InitGetItem();
        }


        switch (slotState)
        {
            case SlotState.ABILITY:
                break;
            case SlotState.ETC:
                break;
            case SlotState.PASSIVE:
                break;
            case SlotState.ELEMENTAL:
                {
                    if(isGet)
                    {
                        CurImage.color = Color.white;
                    }
                    else
                    {                    
                        CurImage.color = Color.clear;
                    }
                }
                break;
            case SlotState.EQITMENT:
                {
                    if (isEqit)
                    {
                        PlayerInfoManager.instance.PassiveEqit(slotData.ItemCode, isEqit);
                        for (int i = 0; i < EqitSlots.Length; i++)
                        {
                            if (EqitSlots[i].GetComponent<SlotData>().ItemCode == slotData.ItemCode)
                                EqitSlots[i].isEqit = true;

                        }
                    }
                    else
                    {
                        itemImage.color = Color.clear;
                    }
                }
                break;
        }
        
    }

}
