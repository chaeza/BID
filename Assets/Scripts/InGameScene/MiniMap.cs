using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    // player position
    private GameObject target;
    // position correction
    private Vector2 tempVec;
    private float ratioX = 1.3103305785123966942148760330579f;
    private float ratioY = 1.2954545454545454545454545454545f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("Player");
        tempVec.x = 546.6f - target.transform.position.x;
        tempVec.y = 507.6f - target.transform.position.z;
        if (target == null)
            return;
        else
        {
            transform.localPosition = new Vector2(-121 + (tempVec.x / ratioX), -121 + (tempVec.y / ratioY));
        }
    }
}
