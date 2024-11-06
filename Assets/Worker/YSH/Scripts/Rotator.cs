using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] bool isSpaceWorld = true;
    [SerializeField] Vector3 rotValue;

    Space space;

    private void Awake()
    {
        space = isSpaceWorld ? Space.World : Space.Self;
    }

    private void Update()
    {
        transform.Rotate(rotValue * Time.deltaTime, space);
    }
}
