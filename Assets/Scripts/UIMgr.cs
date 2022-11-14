using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class UIMgr : MonoBehaviourPun
{   // Save the object that called the skill cooldown
    [Header("½ºÅ³ Icon")]
    [SerializeField] private GameObject[] skillIcon;
    [SerializeField] private GameObject[] itemIcon;
    [SerializeField] private GameObject skillIconP;
    [SerializeField] private GameObject itemIconP;
    [Header("Ending Images")]
    public GameObject winLogo;
    public GameObject loseLogo;

    private TextMeshProUGUI[] skillCoolTimeText = new TextMeshProUGUI[2];
    private GameObject[] skillCoolTime = new GameObject[2];
    private GameObject[] skillUI = new GameObject[2];
    private GameObject[] skillDescription = new GameObject[2];
    private GameObject[] itemUI = new GameObject[4];
    private GameObject[] itemDescription = new GameObject[4];
    private bool[] isCanSkillDescription = new bool[2] { true, true };
    private bool[] setSkillDescription = new bool[2];
    private bool isCanItemDescription = true;
    private bool setItemDescription;


    public delegate void OnResetCoolTime(int skillNum);
    public event OnResetCoolTime onResetCoolTime;
    public delegate void OnSetItemDescription(int skillNum);
    public event OnSetItemDescription onSetItemDescription;
    public delegate void OnSetSkillDescription();
    public event OnSetSkillDescription onSetSkillDescription;
    private Vector2 createPoint = new Vector2(130, 90);
    private Vector2 dashCreatePoint = new Vector2(1500, 60);
    private Vector2[] itemCreatePoint = { new Vector2(720, 60), new Vector2(885, 60), new Vector2(1045, 60), new Vector2(1200, 60) };

    private void Update()
    {

        if (Input.mousePosition.x > 60 && Input.mousePosition.x < 180 && Input.mousePosition.y > 25 && Input.mousePosition.y < 180)
        {
            isCanSkillDescription[0] = true;
            SetSkillDescription(0);
        }
        else
        {
            isCanSkillDescription[0] = false;
            SetSkillDescription(0);
        }
        if (Input.mousePosition.x > 680 && Input.mousePosition.x < 760 && Input.mousePosition.y > 25 && Input.mousePosition.y < 155 && GameMgr.Instance.inventory.InvetoryCount(0) == false)
        {
            isCanItemDescription = true;
            SetItemDescription(0);
        }
        else if (Input.mousePosition.x > 845 && Input.mousePosition.x < 915 && Input.mousePosition.y > 25 && Input.mousePosition.y < 155 && GameMgr.Instance.inventory.InvetoryCount(1) == false)
        {
            isCanItemDescription = true;
            SetItemDescription(1);
        }
        else if (Input.mousePosition.x > 1005 && Input.mousePosition.x < 1080 && Input.mousePosition.y > 25 && Input.mousePosition.y < 155 && GameMgr.Instance.inventory.InvetoryCount(2) == false)
        {
            isCanItemDescription = true;
            SetItemDescription(2);
        }
        else if (Input.mousePosition.x > 1160 && Input.mousePosition.x < 1240 && Input.mousePosition.y > 25 && Input.mousePosition.y < 155 && GameMgr.Instance.inventory.InvetoryCount(3) == false)
        {
            isCanItemDescription = true;
            SetItemDescription(3);
        }
        else if (setItemDescription == true)
        {
            isCanItemDescription = false;
            SetItemDescription(5);
        }
        if (Input.mousePosition.x > 1450 && Input.mousePosition.x < 1550 && Input.mousePosition.y > 25 && Input.mousePosition.y < 155)
        {
            isCanSkillDescription[1] = true;
            SetSkillDescription(1);
        }
        else
        {
            isCanSkillDescription[1] = false;
            SetSkillDescription(1);
        }
    }
    public void EndGame(string winnerNickName)
    {
        if (winnerNickName == PhotonNetwork.NickName)
            winLogo.SetActive(true);
        else
            loseLogo.SetActive(true);
    }

    public void SetItemDescription(int itemNum)
    {
        if (isCanItemDescription == true)
        {
            if (itemNum == 5) return;
            else if (setItemDescription == false)
            {
                Debug.Log("true");
                setItemDescription = true;
                if (itemDescription[itemNum] != null) itemDescription[itemNum].SetActive(true);
                if (onSetItemDescription != null) onSetItemDescription(itemNum);
            }
        }
        else if (setItemDescription == true)
        {
            Debug.Log("False");
            setItemDescription = false;
            if (itemNum == 5)
            {
                if (itemDescription[0] != null) itemDescription[0].SetActive(false);
                if (itemDescription[1] != null) itemDescription[1].SetActive(false);
                if (itemDescription[2] != null) itemDescription[2].SetActive(false);
                if (itemDescription[3] != null) itemDescription[3].SetActive(false);
                if (onSetItemDescription != null) onSetItemDescription(5);
            }
        }
    }
    public void SetItemIcon(int itemType, int itemNum)
    {
        itemUI[itemNum] = Instantiate(itemIcon[itemType], itemCreatePoint[itemNum], Quaternion.identity, GameObject.Find("Canvas").transform);
        itemUI[itemNum].transform.SetParent(itemIconP.transform);
        itemDescription[itemNum] = itemUI[itemNum].transform.GetChild(0).gameObject;
    }
    public void UseItem(int itemNum)
    {
        Destroy(itemUI[itemNum]);
        GameMgr.Instance.inventory.RemoveInventory(itemNum);
    }

    public void SetSkillDescription(int num)
    {
        if (isCanSkillDescription[num] == true)
        {
            if (setSkillDescription[num] == false)
            {
                Debug.Log("true");
                setSkillDescription[num] = true;
                skillDescription[num].SetActive(true);
                if (onSetSkillDescription != null && num == 0) onSetSkillDescription();
            }
        }
        else if (setSkillDescription[num] == true)
        {
            Debug.Log("False");
            setSkillDescription[num] = false;
            skillDescription[num].SetActive(false);
            if (onSetSkillDescription != null && num == 0) onSetSkillDescription();
        }
    }
    //  Vector3 IconPos= Camera.main.WorldToScreenPoint(Vector3.zero);
    public void SetSkillIcon(int skillNum, int num)
    {
        if (num == 1) skillUI[num] = Instantiate(skillIcon[skillNum], dashCreatePoint, Quaternion.identity, GameObject.Find("Canvas").transform);
        else skillUI[num] = Instantiate(skillIcon[skillNum], createPoint, Quaternion.identity, GameObject.Find("Canvas").transform);
        skillUI[num].transform.SetParent(skillIconP.transform);
        skillDescription[num] = skillUI[num].transform.GetChild(0).gameObject;
        skillCoolTime[num] = skillUI[num].transform.GetChild(1).gameObject;
        skillCoolTimeText[num] = skillCoolTime[num].GetComponent<TextMeshProUGUI>();
    }

    //Object that called the skill cooldown to the UI manager, cooldown time
    public void SkillCooltime(int time, int skillNum, int num)
    {
        // Change the color of the icon to be dimmed to give it an inactive feel.
        skillUI[num].GetComponent<RawImage>().color = new Color(160 / 255f, 160 / 255f, 160 / 255f);
        // Change the cool-time text to the max value of the cool-time.
        skillCoolTimeText[num].text = time.ToString();
        // Execute the cool-time coroutine and wait for the cool-time time.
        StartCoroutine(SkillCooltime_Count(time, skillNum, num));

    }
    IEnumerator SkillCooltime_Count(int time, int skillNum, int num)
    {
        // Store the received cooldown time in i
        for (int i = time - 1; i >= 0; --i)
        {
            // wait 1 second
            yield return new WaitForSeconds(1f);
            // Decrease cooldown text by -1
            skillCoolTimeText[num].text = i.ToString();
            yield return null;
        }

        // icon color original position
        skillUI[num].GetComponent<RawImage>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        // Call ResetCooltime to the object that called the saved UI manager to use the skill again
        if (onResetCoolTime != null) onResetCoolTime(skillNum);
        // instead of disabling text, just print nothing
        skillCoolTimeText[num].text = " ";
        yield break;
    }
}
