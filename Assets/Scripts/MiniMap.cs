using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    // player position
    private GameObject target;
    // position correction
    private Vector3 offset;
    private float ratio = 1.355117f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("mainPlayer");
        if (target == null)
            return;
        else
        {
            transform.localPosition = new Vector3(target.transform.position.x * ratio, target.transform.position.z * ratio, 0);
        }
    }
}
