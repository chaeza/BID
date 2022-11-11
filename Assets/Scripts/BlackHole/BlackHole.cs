using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlackHole : MonoBehaviourPun
{
    // Store nearby colliders.
    private Collider[] colliders = null;
    // variable to hold the time
    private float time;
    // Variable for direction
    private Vector3 dir;


    private void Update()
    {

        // save time
        time += Time.deltaTime;
        // Creates a sphere inside and returns an array of colliders that touched the sphere // x radius
        colliders = Physics.OverlapSphere(transform.position, 30f);
        BlackHoleCheck();
    }

    private void BlackHoleCheck()
    {

        // Run the loop to control the objects in the collider array.
        foreach (Collider collider in colliders)
        {
            // measure distance
            float dis = Vector3.Distance(this.transform.position, collider.transform.position);

            if (time > 6)
            {
                dir = this.transform.position - collider.transform.position;

                collider.gameObject.transform.position += dir * 0.8f * Time.deltaTime;
            }

            if (dis <= 0.3f)
            {
                collider.gameObject.transform.position += dir * 1f * Time.deltaTime;
            }

            if (dis <= 0.05f)
            {
                collider.gameObject.transform.position += dir * 1.2f * Time.deltaTime;
            }

            if (time >= 10)
            {
                if (collider.tag == "Player" || collider.tag == "MainPlayer")
                {

                    collider.gameObject.SetActive(false);
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
