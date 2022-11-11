using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlackHole : MonoBehaviourPun
{
    // ��ó �ݶ��̴����� ��� �д�.
    private Collider[] colliders = null;
    // �ð��� ����� ����
    private float time;
    // ������ ����� ����
    private Vector3 dir;


    private void Update()
    {
        // �ð� ����
        time += Time.deltaTime;
        // ���ο� ��ü�� �����Ͽ� �� ��ü�� ���� �ݶ��̴����� �迭�� ��ȯ  // x �ݰ�
        colliders = Physics.OverlapSphere(transform.position, 30f);
        BlackHoleCheck();
    }

    private void BlackHoleCheck()
    {
        // �ݺ����� ���� �ݶ��̴� �迭�� �����ϴ� ������Ʈ�� ��Ʈ���Ѵ�.
        foreach (Collider collider in colliders)
        {
            // �Ÿ� ����
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
