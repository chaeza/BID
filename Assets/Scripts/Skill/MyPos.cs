using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MyPosInfo
{
    public Transform myPos;
    public float yPos;
}
public class MyPos : MonoBehaviour
{
    public MyPosInfo myPosInfo;

    void Update()
    {
        transform.position = myPosInfo.myPos.transform.position;
        if (myPosInfo.yPos != 0) transform.Translate(0f,myPosInfo.yPos, 0f);
    }
}
