using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System;
using MoreMountains.CorgiEngine;

public class DataManager
{

    public static void InitSave()
    {


        //string SAVE_DATA_DIRECTORY = Application.dataPath + "/saves/";
        string SAVE_DATA_DIRECTORY = Application.persistentDataPath + "/saves/";
        string SAVE_FIRLENAME = "/PlayerFile.txt";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Application.persistentDataPath + "/playerInfotest.dat");

        

       // StageData InitStage = new StageData(1011.ToString(), true, StageInfoData.LevelType.Normal);
        
        List<Dictionary<string, object>> data;
        List<OldStageData> InitStageList = new List<OldStageData>();
        data = CSVReader.Read("StageData");

        if (data != null)
        {
            for (int i = 0; i < data.Count; i++)
            {
                OldStageData InitStage = new OldStageData(data[i]["StageCode"].ToString(), false, StageInfoData.LevelType.Normal);
                InitStageList.Add(InitStage);
            }
        }
        InitStageList[0].Open = true;

        List<MaskData> InitMaskList = new List<MaskData>();
        List<MaskData> InitEqitMaskDataList = new List<MaskData>();
        data = CSVReader.Read("MaskData");

       
              
        InitMaskList[0].Maskget = true;
        InitEqitMaskDataList.Add(InitMaskList[0]);

        //SaveData save = new SaveData("temp", "1234", 0, 0, 100, 10, null, null, InitStageList, InitEqitMaskDataList, InitMaskList);

        //string Json = JsonUtility.ToJson(save);

       //// File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FIRLENAME, Json);
      //  Debug.Log(Json);

        //bf.Serialize(file, save);
        //file.Close();
        Debug.Log("initsave");
    }

    public static SaveData DataLoad()
    {
        /*
        if (File.Exists(Application.persistentDataPath + "/playerInfotest.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfotest.dat", FileMode.Open);
            SaveData save = bf.Deserialize(file) as SaveData;
            file.Close();           

            return save;
        }*/
        //if (File.Exists(Application.dataPath + "/saves/" + "/SaveFile.txt"))
        if (File.Exists(Application.persistentDataPath + "/saves/" + "/SaveFile.txt"))
        {
            //string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/SaveFile.txt");
            string loadJson = File.ReadAllText(Application.persistentDataPath + "/saves/" + "/SaveFile.txt");
            SaveData save = JsonUtility.FromJson<SaveData>(loadJson);

            return save;
        }

        else
            return null;
    }

    public static void StageStateSave(List<StageInfo> stageInfos)
    {
        //string SAVE_DATA_DIRECTORY = Application.dataPath + "/saves/";
        string SAVE_DATA_DIRECTORY = Application.persistentDataPath + "/saves/";
        string SAVE_FIRLENAME = "/StageSaveFile.txt";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

        //StageData StageDatas = new StageData();
        List<bool> m_visit = new List<bool>();
        List<bool> m_clear = new List<bool>();
        List<bool> m_active = new List<bool>();

        for (int i = 0; i < stageInfos.Count; i++)
        {
            for (int j = 0; j < stageInfos[i].Visit.Count; j++)
            {
                m_visit.Add(stageInfos[i].Visit[j]);
            }
            for (int k = 0; k < stageInfos[i].BossClear.Count; k++)
            {
                m_clear.Add(stageInfos[i].BossClear[k]);
            }
            for (int y = 0; y < stageInfos[i].Active.Count; y++)
            {
                m_active.Add(stageInfos[i].Active[y]);
            }
        }
        StageData StageDatas = new StageData(stageInfos, m_visit, m_clear,m_active);
        string Json = JsonUtility.ToJson(StageDatas);
        string key = "JESUS";
        Json = Encrypt(Json, key);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FIRLENAME, Json);
        //Debug.Log(stageInfos[0].Active[0]);
    }
    public static StageData StageStateLoad()
    {
        //if (File.Exists(Application.dataPath + "/saves/" + "/StageSaveFile.txt"))
        if (File.Exists(Application.persistentDataPath + "/saves/" + "/StageSaveFile.txt"))
        {
            string key = "JESUS";
            //string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/StageSaveFile.txt");
            string loadJson = File.ReadAllText(Application.persistentDataPath + "/saves/" + "/StageSaveFile.txt");
            loadJson = Decrypt(loadJson, key);
            StageData save = JsonUtility.FromJson<StageData>(loadJson);
            return save;
        }
        else
            return null;
    }

    public static void PlayerDataSave(List<SavePoint> point, CharacterDash dash, CharacterJump jump, CharacterWallClinging climb, MaskSkills skills)
    {
        //string SAVE_DATA_DIRECTORY = Application.dataPath + "/saves/";
        string SAVE_DATA_DIRECTORY = Application.persistentDataPath + "/saves/";
        string SAVE_FIRLENAME = "/PlayerFile.txt";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

        List<bool> m_checkpoint = new List<bool>();

        for (int i = 0; i < point.Count; i++)
        {
            m_checkpoint.Add(point[i].CurSave);
        }



        SaveData StageDatas = new SaveData(m_checkpoint,0,0, dash.AbilityPermitted, jump.NumberOfJumps,climb.AbilityPermitted,skills.DoubleDashOn);
        string Json = JsonUtility.ToJson(StageDatas);
        string key = "JESUS";
        Json = Encrypt(Json, key);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FIRLENAME, Json);
        //Debug.Log(stageInfos[0].Visit[1]);
    }

    public static SaveData PlayerDataLoad()
    {
        //if (File.Exists(Application.dataPath + "/saves/" + "/PlayerFile.txt"))
        if (File.Exists(Application.persistentDataPath + "/saves/" + "/PlayerFile.txt"))
        {
            string key = "JESUS";
            //string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/PlayerFile.txt");
            string loadJson = File.ReadAllText(Application.persistentDataPath + "/saves/" + "/PlayerFile.txt");
            loadJson = Decrypt(loadJson, key);
            SaveData save = JsonUtility.FromJson<SaveData>(loadJson);
            return save;
        }
        else
            return null;
            
    }

    public static void InventorySave(SlotManager slotManager)
    {
        //string SAVE_DATA_DIRECTORY = Application.dataPath + "/saves/";
        string SAVE_DATA_DIRECTORY = Application.persistentDataPath + "/saves/";
        string SAVE_FIRLENAME = "/ItemFile.txt";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
     
        InventoryData itemDatas = new InventoryData(slotManager.Elemetal_get, slotManager.Skill_get, slotManager.Inventory_get, slotManager.Passive_get, slotManager.EqitSlot_eqit);
        string Json = JsonUtility.ToJson(itemDatas);
        string key = "JESUS";
        Json = Encrypt(Json, key);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FIRLENAME, Json);
    }

    public static InventoryData InventorLoad()
    {
        //if (File.Exists(Application.dataPath + "/saves/" + "/ItemFile.txt"))
        if (File.Exists(Application.persistentDataPath + "/saves/" + "/ItemFile.txt"))
        {
            string key = "JESUS";
            //string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/ItemFile.txt");
            string loadJson = File.ReadAllText(Application.persistentDataPath + "/saves/" + "/ItemFile.txt");
            loadJson = Decrypt(loadJson, key);
            InventoryData save = JsonUtility.FromJson<InventoryData>(loadJson);
            return save;
        }
        else
            return null;
    }

    public static void GateSave(List<bool> GateData)
    {
        //string SAVE_DATA_DIRECTORY = Application.dataPath + "/saves/";
        string SAVE_DATA_DIRECTORY = Application.persistentDataPath + "/saves/";
        string SAVE_FIRLENAME = "/GateFile.txt";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

        GateMeetData gateSavedata = new GateMeetData(GateData);
        string Json = JsonUtility.ToJson(gateSavedata);
        string key = "JESUS";
        Json = Encrypt(Json, key);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FIRLENAME, Json);
    }

    public static void ClearSsave()
    {
        //string SAVE_DATA_DIRECTORY = Application.dataPath + "/saves/";
        string SAVE_DATA_DIRECTORY = Application.persistentDataPath + "/saves/";
        string SAVE_FIRLENAME = "/ClearTrophy.txt";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);



        string Json = "Xion";
        string key = "JESUS";
        Json = Encrypt(Json, key);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FIRLENAME, Json);
    }

    public static string ClearLoad()
    {
        //if (File.Exists(Application.dataPath + "/saves/" + "/ClearTrophy.txt"))
        if (File.Exists(Application.persistentDataPath + "/saves/" + "/ClearTrophy.txt"))
        {
            string key = "JESUS";
            //string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/ClearTrophy.txt");
            string loadJson = File.ReadAllText(Application.persistentDataPath + "/saves/" + "/ClearTrophy.txt");
            loadJson = Decrypt(loadJson, key);
            string save = loadJson;

            return save;
        }
        else
            return null;
    }

    public static void NewGameSave()
    {
        string SAVE_DATA_DIRECTORY = Application.persistentDataPath + "/saves/";
        string SAVE_FIRLENAME = "/ImNewwbie.txt";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

        string Json = "NotNewbie";
        string key = "JESUS";
        Json = Encrypt(Json, key);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FIRLENAME, Json);
    }

    public static string NewGameLoad()
    {
        if (File.Exists(Application.persistentDataPath + "/saves/" + "/ImNewwbie.txt"))
        {
            string key = "JESUS";
            //string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/ClearTrophy.txt");
            string loadJson = File.ReadAllText(Application.persistentDataPath + "/saves/" + "/ImNewwbie.txt");
            loadJson = Decrypt(loadJson, key);
            string save = loadJson;

            return save;
        }
        else
            return null;
    }


    public static GateMeetData GateLoad()
    {
        //if (File.Exists(Application.dataPath + "/saves/" + "/GateFile.txt"))
        if (File.Exists(Application.persistentDataPath + "/saves/" + "/GateFile.txt"))
        {
            //string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/GateFile.txt");
            string loadJson = File.ReadAllText(Application.persistentDataPath + "/saves/" + "/GateFile.txt");
            string key = "JESUS";
            loadJson = Decrypt(loadJson, key);
            GateMeetData save = JsonUtility.FromJson<GateMeetData>(loadJson);
            return save;
        }
        else
            return null;
    }

    public static void StageInSave(int stamina, int key)
    {
        /*
        if (File.Exists(Application.persistentDataPath + "/playerInfotest.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfotest.dat", FileMode.Open);
            SaveData save = bf.Deserialize(file) as SaveData;
            file.Close();

            file = File.Create(Application.persistentDataPath + "/playerInfotest.dat");


            save = new SaveData(save.nickName, save.ID, save.Gold, save.Key, stamina,key, save.MountedList, save.ownEquipList, save.StageOpenInfoList, save.MoutedMaskList, save.ownMaskList);

            bf.Serialize(file, save);
            file.Close();
        }
        if(File.Exists(Application.dataPath + "/saves/" + "/SaveFile.txt"))
        {
            string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/SaveFile.txt");

            SaveData save = JsonUtility.FromJson<SaveData>(loadJson);

            //save = new SaveData(save.nickName, save.ID, save.Gold, save.Key, stamina, key, save.MountedList, save.ownEquipList, save.StageOpenInfoList, save.MoutedMaskList, save.ownMaskList);
            string Json = JsonUtility.ToJson(save);
            File.WriteAllText(Application.dataPath + "/saves/" + "/SaveFile.txt", Json);
        }*/

    }


    public static void StageClearSave(int gold, int gem)
    {
        /*
        if (File.Exists(Application.persistentDataPath + "/playerInfotest.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfotest.dat", FileMode.Open);
            SaveData save = bf.Deserialize(file) as SaveData;
            file.Close();

            file = File.Create(Application.persistentDataPath + "/playerInfotest.dat");

            save = new SaveData(save.nickName,save.ID,gold,gem,save.Stamina,save.Key,null,null,save.StageOpenInfoList,save.MoutedMaskList,save.ownMaskList);

            bf.Serialize(file, save);
            file.Close();
        }
        */
        if (File.Exists(Application.dataPath + "/saves/" + "/SaveFile.txt"))
        {
            string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/SaveFile.txt");

            SaveData save = JsonUtility.FromJson<SaveData>(loadJson);

           // save = new SaveData(save.nickName, save.ID, gold, gem, save.Stamina, save.Key, null, null, save.StageOpenInfoList, save.MoutedMaskList, save.ownMaskList);
            string Json = JsonUtility.ToJson(save);
            File.WriteAllText(Application.dataPath + "/saves/" + "/SaveFile.txt", Json);
        }
    }

    public static void NextStageDataOpen(List<OldStageData> stages)
    {
        /*
        if (File.Exists(Application.persistentDataPath + "/playerInfotest.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfotest.dat", FileMode.Open);
            SaveData save = bf.Deserialize(file) as SaveData;
            file.Close();

            file = File.Create(Application.persistentDataPath + "/playerInfotest.dat");

            save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, save.MountedList,save.ownEquipList, stages, save.MoutedMaskList, save.ownMaskList);

            bf.Serialize(file, save);
            file.Close();
        }*/
        if (File.Exists(Application.dataPath + "/saves/" + "/SaveFile.txt"))
        {
            string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/SaveFile.txt");

            SaveData save = JsonUtility.FromJson<SaveData>(loadJson);

           // save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, save.MountedList, save.ownEquipList, stages, save.MoutedMaskList, save.ownMaskList);
            string Json = JsonUtility.ToJson(save);
            File.WriteAllText(Application.dataPath + "/saves/" + "/SaveFile.txt", Json);
        }
    }

    public static void SaveAllStone(List<ItemData> eqitItems, List<ItemData> ownItems)
    {
        /*
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfotest.dat", FileMode.Open);
        SaveData save = bf.Deserialize(file) as SaveData;
        file.Close();

        file = File.Create(Application.persistentDataPath + "/playerInfotest.dat");


        save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, eqitItems, ownItems, save.StageOpenInfoList, save.MoutedMaskList, save.ownMaskList);

        bf.Serialize(file, save);
        file.Close();
        */
        try
        {
            string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/SaveFile.txt");

            SaveData save = JsonUtility.FromJson<SaveData>(loadJson);

           // save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, eqitItems, ownItems, save.StageOpenInfoList, save.MoutedMaskList, save.ownMaskList);
            string Json = JsonUtility.ToJson(save);
            File.WriteAllText(Application.dataPath + "/saves/" + "/SaveFile.txt", Json);
        }
       catch
        {
            Debug.Log("data error");
        }
    }

    public static void SaveOwnStone(List<ItemData> ownItems)
    {
        /*
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfotest.dat", FileMode.Open);
        SaveData save = bf.Deserialize(file) as SaveData;
        file.Close();

        file = File.Create(Application.persistentDataPath + "/playerInfotest.dat");
              


        save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, save.MountedList, ownItems,save.StageOpenInfoList, save.MoutedMaskList, save.ownMaskList);

        bf.Serialize(file, save);
        file.Close();
        */
        try
        {
            string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/SaveFile.txt");

            SaveData save = JsonUtility.FromJson<SaveData>(loadJson);

           // save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, save.MountedList, ownItems, save.StageOpenInfoList, save.MoutedMaskList, save.ownMaskList);
            string Json = JsonUtility.ToJson(save);
            File.WriteAllText(Application.dataPath + "/saves/" + "/SaveFile.txt", Json);
        }
        catch
        {
            Debug.Log("data error");
        }
    }

    public static void SaveMountedStone(List<ItemData> MountedItems)
    {
        /*
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfotest.dat", FileMode.Open);
        SaveData save = bf.Deserialize(file) as SaveData;
        file.Close();

        file = File.Create(Application.persistentDataPath + "/playerInfotest.dat");


        save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, MountedItems, save.ownEquipList,save.StageOpenInfoList, save.MoutedMaskList, save.ownMaskList);

        bf.Serialize(file, save);
        file.Close();*/
        try
        {
            string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/SaveFile.txt");

            SaveData save = JsonUtility.FromJson<SaveData>(loadJson);

           // save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, MountedItems, save.ownEquipList, save.StageOpenInfoList, save.MoutedMaskList, save.ownMaskList);
            string Json = JsonUtility.ToJson(save);
            File.WriteAllText(Application.dataPath + "/saves/" + "/SaveFile.txt", Json);
        }
        catch
        {
            Debug.Log("data error");
        }
    }
    
    public static void GetMask(List<MaskData> ownMask)
    {
        /*
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfotest.dat", FileMode.Open);
        SaveData save = bf.Deserialize(file) as SaveData;
        file.Close();

        file = File.Create(Application.persistentDataPath + "/playerInfotest.dat");

        save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, save.MountedList, save.ownEquipList, save.StageOpenInfoList, save.MoutedMaskList, ownMask);

        bf.Serialize(file, save);
        file.Close();*/
        try
        {
            string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/SaveFile.txt");

            SaveData save = JsonUtility.FromJson<SaveData>(loadJson);

          //  save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, save.MountedList, save.ownEquipList, save.StageOpenInfoList, save.MoutedMaskList, ownMask);
            string Json = JsonUtility.ToJson(save);
            File.WriteAllText(Application.dataPath + "/saves/" + "/SaveFile.txt", Json);
        }
        catch
        {
            Debug.Log("data error");
        }
    }

    public static void SaveMountedMask(List<MaskData> mountedMask)
    {
        /*
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfotest.dat", FileMode.Open);
        SaveData save = bf.Deserialize(file) as SaveData;
        file.Close();

        file = File.Create(Application.persistentDataPath + "/playerInfotest.dat");

        save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, save.MountedList, save.ownEquipList, save.StageOpenInfoList, mountedMask,save.ownMaskList );

        bf.Serialize(file, save);
        file.Close();*/
        try
        {
            string loadJson = File.ReadAllText(Application.dataPath + "/saves/" + "/SaveFile.txt");

            SaveData save = JsonUtility.FromJson<SaveData>(loadJson);

          //  save = new SaveData(save.nickName, save.ID, save.Gold, save.Gem, save.Stamina, save.Key, save.MountedList, save.ownEquipList, save.StageOpenInfoList, mountedMask, save.ownMaskList);
            string Json = JsonUtility.ToJson(save);
            File.WriteAllText(Application.dataPath + "/saves/" + "/SaveFile.txt", Json);
        }
        catch
        {
            Debug.Log("data error");
        }
    }

    public static string Decrypt(string textToDecrypt, string key)

    {

        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;

        rijndaelCipher.Padding = PaddingMode.PKCS7;



        rijndaelCipher.KeySize = 128;

        rijndaelCipher.BlockSize = 128;

        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

        byte[] keyBytes = new byte[16];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)

        {

            len = keyBytes.Length;

        }

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;

        rijndaelCipher.IV = keyBytes;

        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

        return Encoding.UTF8.GetString(plainText);

    }


    public static string Encrypt(string textToEncrypt, string key)

    {

        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;

        rijndaelCipher.Padding = PaddingMode.PKCS7;



        rijndaelCipher.KeySize = 128;

        rijndaelCipher.BlockSize = 128;

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

        byte[] keyBytes = new byte[16];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)

        {

            len = keyBytes.Length;

        }

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;

        rijndaelCipher.IV = keyBytes;

        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));

    }
}
