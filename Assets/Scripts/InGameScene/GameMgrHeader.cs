using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG_Indicator;

public partial class GameMgr : Singleton<GameMgr>
{
    [field: Tooltip("Game MGR Player Input")]

    [Tooltip("Game MGR GameSceneLogic")]
    [field: SerializeField]
    public GameSceneLogic gameSceneLogic { get; private set; } = null;

    [Tooltip("Game MGR playerInput")]
    [field: SerializeField]
    public PlayerInput playerInput { get; private set; } = null;

    [Tooltip("Game MGR followCam")]
    [field: SerializeField]
    public FollowCam followCam { get; private set; } = null;

    [Tooltip("Game MGR uIMgr")]
    [field: SerializeField]
    public UIMgr uIMgr { get; private set; } = null;

    [Tooltip("Game MGR randomSkill")]
    [field: SerializeField]
    public RandomSkill randomSkill { get; private set; } = null;
    [Tooltip("Game MGR randomItem")]
    [field: SerializeField]
    public RandomItem randomItem { get; private set; } = null;
    [Tooltip("Game MGR itempSpawner")]
    [field: SerializeField]
    public ItemSpawner itemSpawner { get; private set; } = null;
    [Tooltip("Game MGR inventory")]
    [field: SerializeField]
    public Inventory inventory { get; private set; } = null;
    [Tooltip("Game MGR codeExample")]
    [field: SerializeField]
    public CodeExample codeExample { get; private set; } = null;
    [Tooltip("Game MGR resourceData")]
    [field: SerializeField]
    public ResourceData resourceData { get; private set; } = null;
    [Tooltip("Game MGR potalSystem")]
    [field: SerializeField]
    public PotalSystem potalSystem { get; private set; } = null;

}
