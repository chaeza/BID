using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlackHole : MonoBehaviourPun
{
    // 근처 콜라이더들을 담아 둔다.
    private Collider[] colliders = null;
    // 시간을 담당할 변수
    private float time;
    // 방향을 담당할 변수
    private Vector3 dir;


    private void Update()
    {
        // 시간 저장
        time += Time.deltaTime;
        // 내부에 구체를 생성하여 그 구체에 닿은 콜라이더들의 배열을 반환  // x 반경
        colliders = Physics.OverlapSphere(transform.position, 30f);
        BlackHoleCheck();
    }

    private void BlackHoleCheck()
    {
        // 반복문을 돌려 콜라이더 배열에 존재하는 오브젝트를 컨트롤한다.
        foreach (Collider collider in colliders)
        {
            // 거리 측정
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
