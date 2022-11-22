using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MyPosInfo
{
    public Transform myPos;
    public float yPos;
    public float zPos;
    public float xPos;
}
public class MyPos : MonoBehaviour
{
    public MyPosInfo myPosInfo;

    private void Start()
    {
        transform.position = myPosInfo.myPos.transform.position;
        transform.Translate(myPosInfo.xPos, myPosInfo.yPos, myPosInfo.zPos);
    }
    private void FixedUpdate()
    {
        transform.position = myPosInfo.myPos.transform.position;
        transform.Translate(myPosInfo.xPos, myPosInfo.yPos, myPosInfo.zPos);
    }
}
