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
    [SerializeField] private float time;

    private void Awake()
    {

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient) photonView.StartCoroutine(LoadAsynSceneCoroutine());
        // else photonView.StartCoroutine(PlayerSceneCoroutine());
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
            slider.value = time / 3f;
            photonView.RPC("LoadingState", RpcTarget.All, slider.value);
            if (time > 3) operation.allowSceneActivation = true;

            yield return new WaitForFixedUpdate();
        }
    }
    [PunRPC]
    public void LoadingState(float sldierVlaue)
    {
        slider.value = sldierVlaue;
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