using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;

public class LodingSync : MonoBehaviourPunCallbacks
{
    [SerializeField] private Slider slider;
    [SerializeField] private string SceneName;
    private float time;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.StartCoroutine(LoadAsynSceneCoroutine());

        else
            photonView.StartCoroutine(PlayerSceneCoroutine());
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    IEnumerator LoadAsynSceneCoroutine()
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            slider.value = time / 5f;

            if (time > 5)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

    }

    IEnumerator PlayerSceneCoroutine()
    {
        while (true)
        {
            slider.value = time / 5f;
            yield return null;
        }
    }
}