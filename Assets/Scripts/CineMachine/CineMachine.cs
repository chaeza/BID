using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineMachine : MonoBehaviour
{
    //virtual Camera
    [Header("가상 카메라")]
    [SerializeField] private CinemachineVirtualCamera sceneCam1= null;
    [SerializeField] private CinemachineVirtualCamera sceneCam2 = null;

    //virtual Camera's track * 2
    [Header("트랙")]
    [SerializeField] private CinemachineDollyCart dollyCart = null;
    [SerializeField] private CinemachineDollyCart dollyCart2 = null;

    private Coroutine coroutine = null;

    private void Awake()
    {
        sceneCam2.enabled = false;
        coroutine = StartCoroutine("Cut1");
    }
    private IEnumerator Cut1()
    {
        sceneCam2.enabled = false;
        sceneCam1.enabled = true;

        Debug.Log(sceneCam1);
        //Initialize the cart position to 0
        
        //sceneCam1.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0 ;
        dollyCart.m_Position = 0f;
        while (true)
        {
            dollyCart.m_Speed = 15;
            if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
            {

                coroutine = StartCoroutine("Cut2");
                yield break;
            }
             yield return null;
        }
    }
    private IEnumerator Cut2() 
    {
        sceneCam1.enabled = false;
        sceneCam2.enabled = true;

        //Initialize the cart position to 0
        dollyCart2.m_Position = 0f;
        while (true)
        {
            dollyCart2.m_Speed = 15;
            if (dollyCart2.m_Position >= dollyCart2.m_Path.PathLength)
            {
                coroutine = StartCoroutine("Cut1");
                yield break;
            }
             yield return null;
        }
    }
}
