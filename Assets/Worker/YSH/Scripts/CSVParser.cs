using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CSVParser
{
    const string DATA_DIRECTORY = "Data";

    public static bool GetDataStringWithWeb(string dataText, out string[] lines)
    {
        if (dataText == null)
        {
            Debug.LogError("잘못된 파일입니다!");
            lines = null;
            return false;
        }
        lines = dataText.Split('\n');
        return true;
    }

    public static bool GetDataStringWithLocal(string fileName, out string[] lines)
    {
        TextAsset text = ResourceManager.Instance.Load<TextAsset>($"{DATA_DIRECTORY}/{fileName}");
        if (text == null)
        {
            Debug.LogError("잘못된 파일입니다!");
            lines = null;
            return false;
        }
        lines = text.text.Split('\n');
        return true;
    }
}
