using System.Collections.Generic;
using UnityEngine;

public class SkillUnlockManager : MonoBehaviour
{
    public static SkillUnlockManager Instance;
    public List<int> unlockedSkills = new List<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log("SkillUnlockManager »ý¼ºµÊ");
        unlockedSkills = PlayerSaveManager.LoadUnlockedSkillList("UnlockedSkillList");
    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        Debug.Log("SkillUnlockManager ÆÄ±«µÊ");
        PlayerSaveManager.SaveUnlockedSkillList("UnlockedSkillList", unlockedSkills);
    }

    public bool IsSkillUnlocked(int skillID)
    {
        return unlockedSkills.Contains(skillID);
    }

    public void UnlockSkill(int skillID)
    {
        if (!unlockedSkills.Contains(skillID))
        {
            unlockedSkills.Add(skillID);
        }
    }
}
