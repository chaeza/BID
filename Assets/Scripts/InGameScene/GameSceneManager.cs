using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameSceneManager : MonoBehaviour
{
    private void Awake()
    {
        gameSceneLogic = gameObject.AddComponent<GameSceneLogic>();
    }
    private void Start()
    {
        GameMgr.Instance.GameSceneSetting(gameObject);
    }
}