using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfBlockTrigger : MonoBehaviour
{
    [SerializeField] bool isDownTrigger;
    [SerializeField] Collider _collider;

    private void OnTriggerEnter(Collider other)
    {
        if (isDownTrigger)
        {
            if (other.transform.position.y < transform.position.y)
                _collider.isTrigger = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isDownTrigger)
        {
            if (other.transform.position.y >= transform.position.y-0.1f)
                _collider.isTrigger = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isDownTrigger)
        {
            _collider.isTrigger = false;
        }
    }
}
