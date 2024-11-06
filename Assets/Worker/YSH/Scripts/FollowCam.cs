using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] BoxCollider2D cameraBound;
    [SerializeField] Vector3 delta;

    Camera _cam;

    // cam half width
    float _halfWidth;
    // cam half height
    float _halfHeight;

    private void Awake()
    {
        _cam = GetComponent<Camera>();

        _halfWidth = _cam.orthographicSize * _cam.aspect;
        _halfHeight = _cam.orthographicSize;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetBound(GameObject cameraBound)
    {
        BoxCollider2D bound = cameraBound.GetComponent<BoxCollider2D>();
        if (bound == null)
        {
            return;
        }

        this.cameraBound = bound;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 pos = target.position + delta;

        if (cameraBound != null)
        {
            // clamp camera
            transform.position = new Vector3
                (
                    Mathf.Clamp(pos.x, cameraBound.bounds.min.x + _halfWidth, cameraBound.bounds.max.x - _halfWidth),
                    Mathf.Clamp(pos.y, cameraBound.bounds.min.y + _halfHeight, cameraBound.bounds.max.y - _halfHeight),
                    pos.z
                );
        }
        else
        {
            transform.position = pos;
        }
    }
}
