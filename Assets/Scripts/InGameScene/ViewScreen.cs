using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera = null;
    private Vector2 tempVec;
    private float ratioX = 0.96588348396132983594601426304165f;
    private float ratioY = 1.0797068659017167862159843860718f;
    private float distanceFromPlayerZ = 90;
    private float distanceFromPlayerX = -10;

    void Update()
    {
        tempVec.x = 502.4f - mainCamera.transform.position.x + distanceFromPlayerX;
        tempVec.y = 472.1f - mainCamera.transform.position.z + distanceFromPlayerZ;
        transform.localPosition = new Vector2(-121 + (tempVec.x / ratioX), -121 + (tempVec.y / ratioY));
    }
}
