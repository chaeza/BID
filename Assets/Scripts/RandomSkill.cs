using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkill : MonoBehaviour
{
    //total number of skills
    private int skillNum = 11;

    //skills random
    public int skillRan { get; set; } = 0;
    //Distributing random skills
    public void GetRandomSkill(GameObject player)
    {
        //Random skill number
        skillRan = Random.Range(0, skillNum);
        //skill Test number
        skillRan = 0;
        GameMgr.Instance.uIMgr.SkillIcon(skillRan);
        //if (skillRan == 0) player.AddComponent<StoneField>();
        //else if (skillRan == 1) player.AddComponent<SwordCrash>();
        
        //  else if (skillRan == 9) player.0AddComponent<Skill_BasicMissile>();

        //else
            Debug.Log("Player didn't get any skil");
        //GameMgr.Instance.uIMgr.SkillUI(skillRan);
    }
}
