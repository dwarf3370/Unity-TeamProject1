using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropData : IDataLoader
{
    public int ID;
    public int MinGold;
    public int MaxGold;
    public bool IsDropHP;
    public float LowGradePercent;
    public float MidGradePercent;
    public float HighGradePercent;
    public float BonusGoldPercent;

    public void Load(string[] fields)
    {
        ID = int.Parse(fields[0]);                      // Parse ID
        MinGold = int.Parse(fields[1]);                 // Parse Minimum Gold
        MaxGold = int.Parse(fields[2]);                 // Parse Maximum Gold
        IsDropHP = bool.Parse(fields[3]);               // Parse HP Drop
        LowGradePercent = float.Parse(fields[4]);       // Parse Low Grade Skill Drop Percent
        MidGradePercent = float.Parse(fields[5]);       // Parse Mid Grade Skill Drop Percent
        HighGradePercent = float.Parse(fields[6]);      // Parse High Grade Skill Drop Percent
        BonusGoldPercent = float.Parse(fields[7]);      // Parse Bonus Gold Drop Percent
    }
}
