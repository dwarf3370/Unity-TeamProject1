using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class CSVDownload
{
    const string skillDataUrl = "https://docs.google.com/spreadsheets/d/1KKp21OkOGInFUfQvvE-ZlDCtzdnPYfzevhZbfoynPkQ/export?gid=367973711&format=csv";
    const string monsterDataUrl = "https://docs.google.com/spreadsheets/d/1NXzXEfqV4n5AN5xSIf0AOBBvnqDwgKlSXRmfL1Ra3gs/export?gid=0&format=csv";
    const string dropDataUrl = "https://docs.google.com/spreadsheets/d/1Rlhk9E_9iojPdKPChQ2onZpu_D9cBKOQcgabaQ2OJdo/export?gid=857470714&format=csv";

    public static IEnumerator SkillDataDownloadRoutine()
    {
        // Web에 요청을 보내기 위한 UnityWebRequest 객체
        // urlPath를 통해 웹사이트에 요청
        UnityWebRequest skillDataRequest = UnityWebRequest.Get(skillDataUrl);

        // 요청이 완료될 때 까지 대기 (파일 다운로드)
        yield return skillDataRequest.SendWebRequest();

        // 다운로드가 완료된 상황
        string skillTableText = skillDataRequest.downloadHandler.text;
        if (skillTableText == null)
        {
            Debug.LogError("Skill Data Download Error!");
            yield break;
        }

        Debug.Log("Skill Data Download OK");
        yield return skillTableText;
    }

    public static IEnumerator MonsterDataDownloadRoutine()
    {
        UnityWebRequest monsterDataRequest = UnityWebRequest.Get(monsterDataUrl);

        yield return monsterDataRequest.SendWebRequest();

        string monsterTableText = monsterDataRequest.downloadHandler.text;
        if (monsterTableText == null)
        {
            Debug.LogError("Monster Data Download Error!");
            yield break;
        }

        Debug.Log("Monster Data Download OK");
        yield return monsterTableText;
    }

    public static IEnumerator DropDataDownloadRoutine()
    {
        UnityWebRequest dropDataRequest = UnityWebRequest.Get(dropDataUrl);

        yield return dropDataRequest.SendWebRequest();

        string dropTableText = dropDataRequest.downloadHandler.text;
        if (dropTableText == null)
        {
            Debug.LogError("Drop Data Download Error!");
            yield break;
        }

        Debug.Log("Drop Data Download OK");
        yield return dropTableText;
    }
}
