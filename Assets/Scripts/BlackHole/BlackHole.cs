using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlackHole : MonoBehaviourPun
{
    [SerializeField] private int blackHolePos;
    // Store nearby colliders.
    private BlackHolePos1[] collider1 = null;
    private BlackHolePos2[] collider2 = null;
    private BlackHolePos3[] collider3 = null;
    private BlackHolePos4[] collider4 = null;
    private BlackHolePos5[] collider5 = null;
    private BlackHolePos6[] collider6 = null;
    // variable to hold the time
    private float time;
    // Variable for direction
    private Vector3 dir;
    private void Start()
    {
        if (blackHolePos == 1) collider1 = FindObjectsOfType<BlackHolePos1>();
        else if (blackHolePos == 2) collider2 = FindObjectsOfType<BlackHolePos2>();
        else if (blackHolePos == 3) collider3 = FindObjectsOfType<BlackHolePos3>();
        else if (blackHolePos == 4) collider4 = FindObjectsOfType<BlackHolePos4>();
        else if (blackHolePos == 5) collider5 = FindObjectsOfType<BlackHolePos5>();
        else if (blackHolePos == 6) collider6 = FindObjectsOfType<BlackHolePos6>();
    }

    private void Update()
    {
        // save time
        time += Time.deltaTime;
        // Creates a sphere inside and returns an array of colliders that touched the sphere // x radius
        BlackHoleCheck(blackHolePos);
    }

    private void BlackHoleCheck(int Num)
    {

        // Run the loop to control the objects in the collider array.

        if (Num == 1) foreach (BlackHolePos1 collider in collider1)
            {
                // measure distance
                float dis = Vector3.Distance(this.transform.position, collider.transform.position);

                if (time > 6)
                {
                    dir = this.transform.position - collider.transform.position;

                    collider.gameObject.transform.position += dir * 0.8f * Time.deltaTime;
                }

                /*  if (dis <= 0.3f)
                  {
                      collider.gameObject.transform.position += dir * 1f * Time.deltaTime;
                  }

                  if (dis <= 0.05f)
                  {
                      collider.gameObject.transform.position += dir * 1.2f * Time.deltaTime;
                  }*/

                if (time >= 10)
                {
                    if (collider.tag == "Player" || collider.tag == "MainPlayer")
                    {
                        Debug.Log("플레이어 메인");
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "SpawnArea")
                    {
                        GameMgr.Instance.itemSpawner.RemoveItemList(collider.gameObject);
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "Portal")
                    {
                        collider.GetComponent<Portal>().portalData.isDestoryed = true;
                    }
                    else
                    {
                        collider.gameObject.SetActive(false);
                    }
                    this.gameObject.SetActive(false);
                }
            }
        else if (Num == 2) foreach (BlackHolePos2 collider in collider2)
            {
                // measure distance
                float dis = Vector3.Distance(this.transform.position, collider.transform.position);

                if (time > 6)
                {
                    dir = this.transform.position - collider.transform.position;

                    collider.gameObject.transform.position += dir * 0.8f * Time.deltaTime;
                }

                /*  if (dis <= 0.3f)
                  {
                      collider.gameObject.transform.position += dir * 1f * Time.deltaTime;
                  }

                  if (dis <= 0.05f)
                  {
                      collider.gameObject.transform.position += dir * 1.2f * Time.deltaTime;
                  }*/

                if (time >= 10)
                {
                    if (collider.tag == "Player" || collider.tag == "MainPlayer")
                    {
                        Debug.Log("플레이어 메인");
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "SpawnArea")
                    {
                        GameMgr.Instance.itemSpawner.RemoveItemList(collider.gameObject);
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "Portal")
                    {
                        collider.GetComponent<Portal>().portalData.isDestoryed = true;
                    }
                    else
                    {
                        collider.gameObject.SetActive(false);
                    }
                    this.gameObject.SetActive(false);
                }
            }
        else if (Num == 3) foreach (BlackHolePos3 collider in collider3)
            {
                // measure distance
                float dis = Vector3.Distance(this.transform.position, collider.transform.position);

                if (time > 6)
                {
                    dir = this.transform.position - collider.transform.position;

                    collider.gameObject.transform.position += dir * 0.8f * Time.deltaTime;
                }

                /*  if (dis <= 0.3f)
                  {
                      collider.gameObject.transform.position += dir * 1f * Time.deltaTime;
                  }

                  if (dis <= 0.05f)
                  {
                      collider.gameObject.transform.position += dir * 1.2f * Time.deltaTime;
                  }*/

                if (time >= 10)
                {
                    if (collider.tag == "Player" || collider.tag == "MainPlayer")
                    {
                        Debug.Log("플레이어 메인");
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "SpawnArea")
                    {
                        GameMgr.Instance.itemSpawner.RemoveItemList(collider.gameObject);
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "Portal")
                    {
                        collider.GetComponent<Portal>().portalData.isDestoryed = true;
                    }
                    else
                    {
                        collider.gameObject.SetActive(false);
                    }
                    this.gameObject.SetActive(false);
                }
            }
        else if (Num == 4) foreach (BlackHolePos4 collider in collider4)
            {
                // measure distance
                float dis = Vector3.Distance(this.transform.position, collider.transform.position);

                if (time > 6)
                {
                    dir = this.transform.position - collider.transform.position;

                    collider.gameObject.transform.position += dir * 0.8f * Time.deltaTime;
                }

                /*  if (dis <= 0.3f)
                  {
                      collider.gameObject.transform.position += dir * 1f * Time.deltaTime;
                  }

                  if (dis <= 0.05f)
                  {
                      collider.gameObject.transform.position += dir * 1.2f * Time.deltaTime;
                  }*/

                if (time >= 10)
                {
                    if (collider.tag == "Player" || collider.tag == "MainPlayer")
                    {
                        Debug.Log("플레이어 메인");
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "SpawnArea")
                    {
                        GameMgr.Instance.itemSpawner.RemoveItemList(collider.gameObject);
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "Portal")
                    {
                        collider.GetComponent<Portal>().portalData.isDestoryed = true;
                    }
                    else
                    {
                        collider.gameObject.SetActive(false);
                    }
                    this.gameObject.SetActive(false);
                }
            }
        else if (Num == 5) foreach (BlackHolePos5 collider in collider5)
            {
                // measure distance
                float dis = Vector3.Distance(this.transform.position, collider.transform.position);

                if (time > 6)
                {
                    dir = this.transform.position - collider.transform.position;

                    collider.gameObject.transform.position += dir * 0.8f * Time.deltaTime;
                }

                /*  if (dis <= 0.3f)
                  {
                      collider.gameObject.transform.position += dir * 1f * Time.deltaTime;
                  }

                  if (dis <= 0.05f)
                  {
                      collider.gameObject.transform.position += dir * 1.2f * Time.deltaTime;
                  }*/

                if (time >= 10)
                {
                    if (collider.tag == "Player" || collider.tag == "MainPlayer")
                    {
                        Debug.Log("플레이어 메인");
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "SpawnArea")
                    {
                        GameMgr.Instance.itemSpawner.RemoveItemList(collider.gameObject);
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "Portal")
                    {
                        collider.GetComponent<Portal>().portalData.isDestoryed = true;
                    }
                    else
                    {
                        collider.gameObject.SetActive(false);
                    }

                    this.gameObject.SetActive(false);
                }
            }
        else if (Num == 6) foreach (BlackHolePos6 collider in collider6)
            {
                // measure distance
                float dis = Vector3.Distance(this.transform.position, collider.transform.position);

                if (time > 6)
                {
                    dir = this.transform.position - collider.transform.position;

                    collider.gameObject.transform.position += dir * 0.8f * Time.deltaTime;
                }

                /*  if (dis <= 0.3f)
                  {
                      collider.gameObject.transform.position += dir * 1f * Time.deltaTime;
                  }

                  if (dis <= 0.05f)
                  {
                      collider.gameObject.transform.position += dir * 1.2f * Time.deltaTime;
                  }*/

                if (time >= 10)
                {
                    if (collider.tag == "Player" || collider.tag == "MainPlayer")
                    {
                        Debug.Log("플레이어 메인");
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "SpawnArea")
                    {
                        GameMgr.Instance.itemSpawner.RemoveItemList(collider.gameObject);
                        collider.gameObject.SetActive(false);
                    }
                    else if (collider.tag == "Portal")
                    {
                        collider.GetComponent<Portal>().portalData.isDestoryed = true;
                    }
                    else
                    {
                        collider.gameObject.SetActive(false);
                    }
                    this.gameObject.SetActive(false);
                }
            }
    }

}
