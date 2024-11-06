using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] string nextScene;

    public void MoveNextScene()
    {
        SoundManager.Instance.Play(Enums.ESoundType.SFX, "Teleport");
        GameManager.Instance.LoadScene(nextScene);
    }
}
