using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class PlayerSaveManager
{
    public static void SavePlayerSattus(int upgradedAtk, int upgradedDef, int upgradedHealth, int upgradedCooldown)
    {
        PlayerPrefs.SetInt("UpgradedAtk", upgradedAtk);
        PlayerPrefs.SetInt("UpgradedDef", upgradedDef);
        PlayerPrefs.SetInt("UpgradedHealth", upgradedHealth);
        PlayerPrefs.SetInt("UpgradedCooldown", upgradedCooldown);

        PlayerPrefs.Save();
    }

    public static void LoadPlayerStatus(out int upgradedAtk, out int upgradedDef, out int upgradedHealth, out int upgradedCooldown) 
    {
        upgradedAtk = PlayerPrefs.GetInt("UpgradedAtk", 0);
        upgradedDef = PlayerPrefs.GetInt("UpgradedDef", 0);
        upgradedHealth = PlayerPrefs.GetInt("UpgradedHealth", 0);
        upgradedCooldown = PlayerPrefs.GetInt("UpgradedCooldown", 0);
    }

    public static void SaveGold(int gold)
    {
        PlayerPrefs.SetInt("Gold", gold);

        PlayerPrefs.Save();
    }

    public static void LoadGold(out int gold)
    {
        gold = PlayerPrefs.GetInt("Gold", 0);
    }

    public static void SaveUnlockedSkillList(string key, List<int> list)
    {
        // List를 콤마로 구분된 문자열로 변환
        string listString = string.Join(",", list);

        // PlayerPrefs에 문자열로 저장
        PlayerPrefs.SetString(key, listString);
        PlayerPrefs.Save();
        Debug.Log("UnlockedSkillList 저장됨");
    }

    public static List<int> LoadUnlockedSkillList(string key)
    {
        // PlayerPrefs에서 문자열 불러오기
        string listString = PlayerPrefs.GetString(key, string.Empty);

        // 저장된 데이터가 없으면 빈 리스트 반환
        if (string.IsNullOrEmpty(listString))
        {
            Debug.Log("빈 UnlockedSkillList 불러옴");
            return new List<int>();
        }

        // 콤마로 문자열을 분리하고 int 리스트로 변환
        string[] stringArray = listString.Split(',');
        List<int> intlist = new List<int>();

        foreach (string str in stringArray)
        {
            if(int.TryParse(str, out int value))
            {
                intlist.Add(value);
            }
        }
        Debug.Log("UnlockedSkillList 불러옴");
        return intlist;
    }
}
