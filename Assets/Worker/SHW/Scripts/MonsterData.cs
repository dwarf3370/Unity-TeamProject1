public class MonsterData : IDataLoader
{
    public int ID;              // 몬스터 NO.
    public string Name;         // 몬스터 이름
    public bool AttackType;     // 공격 타입 true일 경우 원거리
    public float AttackRage;    // 공격 사거리
    public int Rage;            // 추적거리
    public bool CanSkill;       // 스킬여부 : true일 경우 스킬 존재
    public float Attack;          // 공격력
    public float Defense;         // 방어력
    public float Hp;            // 체력
    public float AttackSpeed;   // 공속
    public float WalkSpeed;     // 걷기속도
    public float RunSpeed;      // 뛰기이속

    public int SkillRage;    // 스킬 사거리 int
    public float SkillCool; // 쿨타임 float
    public float SkillTime;   // 지속시간 float

    public void Load(string[] fields)
    {
        ID = int.Parse(fields[0]);                  // Parse No.
        Name = fields[1];                           // Parse 이름
        AttackType = bool.Parse(fields[2]);         // Parse 공격 타입
        AttackRage = float.Parse(fields[3]);        // 공격 범위
        Rage = int.Parse(fields[4]);                // 추적거리
        CanSkill = bool.Parse(fields[5]);           // 스킬여부
        Attack = float.Parse(fields[6]);            // 공격력
        Defense = float.Parse(fields[7]);           // 방어력
        Hp = float.Parse(fields[8]);                // 체력
        AttackSpeed = float.Parse(fields[9]);       // 공속
        WalkSpeed = float.Parse(fields[10]);        // 걷기속도
        RunSpeed = float.Parse(fields[11]);         // 뛰기속도
        SkillRage = int.Parse(fields[12]);          // 스킬 사거리
        SkillCool = float.Parse(fields[13]);        // 스킬 쿨타임
        SkillTime = float.Parse(fields[14]);        // 스킬 지속시간
    }
}
