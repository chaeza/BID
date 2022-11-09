using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameMgr : Singleton<GameMgr>
{
    [field: Tooltip("Game MGR Player Input")]

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
}
