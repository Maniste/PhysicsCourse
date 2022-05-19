using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class saveload
{
    private static string path = "/SaveDataFile/";
    private static string filename = "SaveFile.txt";

    public static SaveData Savedatascript = new SaveData();
    public static void savetofile()
    {
        if (!CheckIfExist())
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + path);

        string json = JsonUtility.ToJson(Savedatascript);
        System.IO.File.WriteAllText(Application.persistentDataPath + path + filename, json);
    }
    public static void loadsave()
    {
        string json = System.IO.File.ReadAllText(Application.persistentDataPath + path + filename);
        Savedatascript = JsonUtility.FromJson<SaveData>(json);
    }
    public static void savelevel(int number)
    {
        Savedatascript.CurrentLevel = number;
        Debug.Log("SavingGame");
        savetofile();
    }
    public static int GetLevel() { return Savedatascript.CurrentLevel; }
    public static bool CheckIfExist()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + path + filename))
            return true;
        else return false;

    }
    public static void DeleteSave()
    {
        System.IO.File.Delete(Application.persistentDataPath + path + filename);
    }
}