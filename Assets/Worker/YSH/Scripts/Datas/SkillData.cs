using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : IDataLoader
{
    public int ID;
    public string Name;
    public string Description;
    public Enums.SkillType SkillType;
    public Enums.Grade Grade;
    public float Damage;                // 계수
    public int CoolTime;
    public int Price;
    public int Radius;                  // 피해 범위
    public string ClassName;
    public float Second;                // 도트 데미지 초
    public int Tick;                    // 초당 틱 횟수
    public int Range;                   // 사거리 (스캔이 시작되는 상대 위치로도 사용)
    public float CastTime;              // 시전시간
    public int ProjectileSpeed;         // 투사체 속도
    public bool CanPenetration;         // 관통 여부
    public Sprite SkillIcon;            // 스킬 아이콘 이미지

    public void Load(string[] fields)
    {
        ID = int.Parse(fields[0]);                  // Parse ID
        Name = fields[1];                           // Parse Name
        Description = fields[2];                    // Parse Description
        Enum.TryParse(fields[3], out SkillType);    // Parse SkillType
        Enum.TryParse(fields[4], out Grade);        // Parse Grade
        Damage = float.Parse(fields[5]);            // Parse Damage
        CoolTime = int.Parse(fields[6]);            // Parse CoolTime
        Price = int.Parse(fields[7]);               // Parse Price
        Radius = int.Parse(fields[8]);              // Parse Radius
        ClassName = fields[9];                      // Parse ClassName
        Second = float.Parse(fields[10]);           // Parse Second
        Tick = int.Parse(fields[11]);               // Parse Tick
        Range = int.Parse(fields[12]);              // Parse Range
        CastTime = float.Parse(fields[13]);         // Parse CastTime
        ProjectileSpeed = int.Parse(fields[14]);    // Parse Projectile Speed
        CanPenetration = bool.Parse(fields[15]);    // Parse Can Penetration

        // Parse Skill Icon Image
        SkillIcon = ResourceManager.Instance.Load<Sprite>($"Images/{fields[16]}");
    }
}
