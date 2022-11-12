using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    //total number of skills
    private int skillType = 5;
    //skills random
    public int skillRan { get; private set; } = 0;
    //Distributing random skills
    public void GetRandomSkill(GameObject player)
    {
        //Random skill number
        skillRan = Random.Range(2, skillType+1);
       // skillRan = 4;
        GameMgr.Instance.uIMgr.SetSkillIcon(skillRan,0);
     //   if (skillRan == 1) player.AddComponent<TestSkill>();
        if (skillRan == 2) player.AddComponent<Immediate_BloodField>();
        else if (skillRan == 3) player.AddComponent<Projectile_Bash>();
        else if (skillRan == 4) player.AddComponent<Immediate_FallTheRain>();
        else if (skillRan == 5) player.AddComponent<Projectile_MysticArrow>();
        else if (skillRan == 6) player.AddComponent<Projectile_EnergyShoot>();
        else
            Debug.Log("Player didn't get any skill");
    }
}
