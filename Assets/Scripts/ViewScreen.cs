using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera = null;
    private Vector2 tempVec;
    private float ratioX = 1.3103305785123966942148760330579f;
    private float ratioY = 1.2954545454545454545454545454545f;
    private float distanceFromPlayerZ = 90;
    private float distanceFromPlayerX = -10;
    
    // Update is called once per frame
    void Update()
    {
        tempVec.x = 546.6f - mainCamera.transform.position.x + distanceFromPlayerX;
        tempVec.y = 507.6f - mainCamera.transform.position.z + distanceFromPlayerZ;
        transform.localPosition = new Vector2(-121 + (tempVec.x / ratioX), -121 + (tempVec.y / ratioY));
    }
}
