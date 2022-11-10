using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class UIMgr : MonoBehaviourPun
{   // Save the object that called the skill cooldown
    [Header("스킬 Icon")]
    [SerializeField] private GameObject[] skillIcon;
    [SerializeField] private GameObject skillIconP;
    [SerializeField] private GameObject itemIconP;
    [SerializeField] private TextMeshProUGUI skillCoolTimeText;
    private GameObject skillUI;
    private GameObject skillDescription;

    public delegate void OnResetCoolTime(int skillNum);
    public event OnResetCoolTime onResetCoolTime;
    private Vector2 createPoint = new Vector2(130,90);


    [PunRPC]
    public void EndGame(string nickName)
    {
        if (PhotonNetwork.NickName == nickName)
        {
            Debug.Log("승리 이미지");
        }
        else
        {
            Debug.Log("패배 이미지 ");
        }
    }
    //  Vector3 IconPos= Camera.main.WorldToScreenPoint(Vector3.zero);
    public void SetSkillIcon(int skillNum)
    {
        skillUI = Instantiate(skillIcon[skillNum], createPoint, Quaternion.identity, GameObject.Find("Canvas").transform);
        skillUI.transform.SetParent(skillIconP.transform);
        //skillDescription = skillUI.transform.GetChild(0).gameObject;
    }

    //Object that called the skill cooldown to the UI manager, cooldown time
    public void SkillCooltime(int time,int skillNum)
    {
        // Change the color of the icon to be dimmed to give it an inactive feel.
        skillUI.GetComponent<RawImage>().color = new Color(160 / 255f, 160 / 255f, 160 / 255f);
        // Change the cool-time text to the max value of the cool-time.
        skillCoolTimeText.text = time.ToString();
        // Execute the cool-time coroutine and wait for the cool-time time.
        StartCoroutine(SkillCooltime_Count(time,skillNum));

    }
    IEnumerator SkillCooltime_Count(int time, int skillNum)
    {
        // Store the received cooldown time in i
        for (int i = time - 1; i >= 0; --i)
        {
            // wait 1 second
            yield return new WaitForSeconds(1f);
            // Decrease cooldown text by -1
            skillCoolTimeText.text = i.ToString();
            yield return null;
        }

        // icon color original position
         skillUI.GetComponent<RawImage>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        // Call ResetCooltime to the object that called the saved UI manager to use the skill again
        if (onResetCoolTime != null) onResetCoolTime(skillNum);
        // instead of disabling text, just print nothing
        skillCoolTimeText.text = " ";
        yield break;
    }
}
