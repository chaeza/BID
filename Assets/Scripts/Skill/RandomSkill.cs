using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    //total number of skills
    private int skillType = 1;
    //skills random
    public int skillRan { get; private set; } = 0;
    //Distributing random skills
    public void GetRandomSkill(GameObject player)
    {
        //Random skill number
        skillRan = Random.Range(1, skillType+1);
        GameMgr.Instance.uIMgr.SetSkillIcon(skillRan);
        if (skillRan == 1) player.AddComponent<Projectile_EnergyShoot>();
        else
           Debug.Log("Player didn't get any skill");
    }
}
