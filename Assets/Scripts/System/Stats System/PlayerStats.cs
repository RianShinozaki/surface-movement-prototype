using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

/*
 * SAVE FILE FORMAT
 * Add to this as needed

 * DATA TYPE (Name Example)
 CORE FOLDER (Save-X)
 |_____STATS (Stats)
 |    | Health.stat
 |    | Coins.stat
*/

public static class PlayerStats {
    public static int SaveFile = 0;

    static byte magicByte = 0x0010;

    #region File Paths
    //constants
    static readonly string DATAPATH = Application.dataPath;
    static readonly string PERSISTENTDATA = Application.persistentDataPath;

    //File dependants
    static string GetSaveFolderName(int saveFile) => PERSISTENTDATA + $"/Saves/Save-{saveFile + 1}";
    static string GetCoinsFilePath(int saveFile) => GetSaveFolderName(saveFile) + "/Stats/Coins.stat";
    #endregion

    public static PlayerCoins Coins{
        get{
            if(coins == null){
                LoadData(SaveFile);
            }
            return coins;
        }
    }
    static PlayerCoins coins;

    static void LoadData(int saveFile) {
        //Coin stat loading
        //fine I'll use JSON Red
        byte[] jsonBytes = File.ReadAllBytes(GetCoinsFilePath(SaveFile));
        for(int i = 0; i < jsonBytes.Length; i++){
            jsonBytes[i] ^= magicByte;
        }

        string json = System.Text.Encoding.UTF8.GetString(jsonBytes);

        coins = JsonUtility.FromJson<PlayerCoins>(json);
    }

    static void SaveData(int saveFile){
        //Coin stat saving
        string json = JsonUtility.ToJson(coins, true);

        byte[] bits = System.Text.Encoding.UTF8.GetBytes(json);

        for(int i = 0; i < bits.Length; i++){
            bits[i] ^= magicByte;
        }

        File.WriteAllBytes(GetCoinsFilePath(saveFile), bits);
    }
}

public class PlayerCoins {
    public uint CurrentCoins;
    public uint TotalCoinsPickedUp;
    public uint TotaCoinsSpent;
    public uint TotalCoinsLost;
}