using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float moveRate;
    [SerializeField] GameObject BackGroundImage;

    Vector3 MainCamPos;
    Vector3 BGCamPos;

    private void Start()
    {
        // BackGroundImage.transform.position = new Vector3(-100, -100, -100);
        MainCamPos = mainCamera.transform.position;
        BGCamPos = transform.position;
        BackGroundImage.transform.position = BGCamPos + new Vector3(3f, 3f, 10f);
    }

    private void Update()
    {
        transform.position = BGCamPos + (mainCamera.transform.position-MainCamPos) * moveRate;
    }
}
