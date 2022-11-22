using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera = null;
    private Vector2 tempVec;
    private float ratioX = 0.96794920964835228280278942788686f;
    private float ratioY = 1.0809775369401246252376416055294f;
    private float distanceFromPlayerZ = 90;
    private float distanceFromPlayerX = -10;

    void Update()
    {
        tempVec.x = 503.5f - mainCamera.transform.position.x + distanceFromPlayerX;
        tempVec.y = 472.67f - mainCamera.transform.position.z + distanceFromPlayerZ;
        transform.localPosition = new Vector2(-121 + (tempVec.x / ratioX), -121 + (tempVec.y / ratioY));
    }
}
