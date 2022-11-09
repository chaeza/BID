using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    //total number of skills
    private int skillNum = 1;
    //skills random
    public int skillRan { get; private set; } = 0;
    //Distributing random skills
    public void GetRandomSkill(GameObject player)
    {
        //Random skill number
        skillRan = Random.Range(0, skillNum);
        GameMgr.Instance.uIMgr.SetSkillIcon(skillRan);
        if (skillRan == 0) player.AddComponent<TestSkill>();
        else
           Debug.Log("Player didn't get any skill");
    }
}
