using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : Singleton<DataManager>
{
    Dictionary<int, SkillData> _skillData = new Dictionary<int, SkillData>();
    Dictionary<int, MonsterData> _monsterData = new Dictionary<int, MonsterData>();
    Dictionary<int, DropData> _dropData = new Dictionary<int, DropData>();

    public Dictionary<int, SkillData> SkillDict { get { return _skillData; } }
    public Dictionary<int, MonsterData> MonsterDict { get { return _monsterData; } }
    public Dictionary<int, DropData> DropDict { get { return _dropData; } }

    public UnityAction OnLoadCompleted;

    protected override void Init()
    {
        StartCoroutine(LoadData());
    }

    IEnumerator LoadData()
    {
        bool bSuccess = false;

        #region SkillData Load
        // SkillData Load
        IEnumerator LoadEnumerator = LoadSkillData();
        yield return StartCoroutine(LoadEnumerator);

        bSuccess = (LoadEnumerator.Current as string) != null;

        if (!bSuccess)
        {
            Debug.LogError("Skill Data Load Fail...");
            yield break;
        }
        #endregion

        #region MonsterData Load
        // MonsterData Load
        LoadEnumerator = LoadMonsterData();
        yield return StartCoroutine(LoadEnumerator);

        bSuccess = (LoadEnumerator.Current as string) != null;

        if (!bSuccess)
        {
            Debug.LogError("Monster Data Load Fail...");
            yield break;
        }
        #endregion

        #region DropData Load
        // DropData Load
        LoadEnumerator = LoadDropData();
        yield return StartCoroutine(LoadEnumerator);

        bSuccess = (LoadEnumerator.Current as string) != null;

        if (!bSuccess)
        {
            Debug.LogError("Drop Data Load Fail...");
            yield break;
        }
        #endregion

        Debug.Log("Data Load Complete");
        OnLoadCompleted?.Invoke();
    }

    IEnumerator LoadSkillData()
    {
        IEnumerator skillDataEnumerator = CSVDownload.SkillDataDownloadRoutine();
        yield return StartCoroutine(skillDataEnumerator);

        string skillText = skillDataEnumerator.Current as string;
        if (skillText == null)
        {
            yield return null;
            yield break;
        }

        if (CSVParser.GetDataStringWithWeb(skillText, out string[] lines) == false)
        {
            Debug.Log("Skill Data Parse Error!");
            yield return null;
            yield break;
        }

        for (int line = 1; line < lines.Length; line++)
        {
            SkillData skillData = new SkillData();
            skillData.Load(lines[line].Split(','));

            _skillData.Add(skillData.ID, skillData);
        }

        Debug.Log("SkillData Load OK!");
        yield return skillText;
    }

    IEnumerator LoadMonsterData()
    {
        IEnumerator monsterDataEnumerator = CSVDownload.MonsterDataDownloadRoutine();
        yield return StartCoroutine(monsterDataEnumerator);

        string monsterText = monsterDataEnumerator.Current as string;
        if (monsterText == null)
        {
            yield return null;
            yield break;
        }

        if (CSVParser.GetDataStringWithWeb(monsterText, out string[] lines) == false)
        {
            Debug.Log("Monster Data Parse Error!");
            yield return null;
            yield break;
        }

        for (int line = 1; line < lines.Length; line++)
        {
            MonsterData monsterData = new MonsterData();
            monsterData.Load(lines[line].Split(','));

            _monsterData.Add(monsterData.ID, monsterData);
        }

        Debug.Log("MonsterData Load OK!");
        yield return monsterText;
    }

    IEnumerator LoadDropData()
    {
        IEnumerator dropDataEnumerator = CSVDownload.DropDataDownloadRoutine();
        yield return StartCoroutine(dropDataEnumerator);

        string dropText = dropDataEnumerator.Current as string;
        if (dropText == null)
        {
            yield return null;
            yield break;
        }

        if (CSVParser.GetDataStringWithWeb(dropText, out string[] lines) == false)
        {
            Debug.Log("Drop Data Parse Error!");
            yield return null;
            yield break;
        }

        for (int line = 1; line < lines.Length; line++)
        {
            DropData dropData = new DropData();
            dropData.Load(lines[line].Split(','));

            _dropData.Add(dropData.ID, dropData);
        }

        Debug.Log("DropData Load OK!");
        yield return dropText;
    }
}

public interface IDataLoader
{
    public void Load(string[] fields);
}