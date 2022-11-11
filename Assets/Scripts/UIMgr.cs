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
    [SerializeField] private GameObject skillCoolTime;
    [SerializeField] private TextMeshProUGUI skillCoolTimeText;
    private GameObject skillUI;
    private GameObject skillDescription;
    private GameObject[] itemUI = new GameObject[4];
    private GameObject[] itemDescription = new GameObject[4];
    private bool isCanSkillDescription = true;
    private bool setSkillDescription;
    private bool isCanItemDescription = true;
    private bool setItemDescription;


    public delegate void OnResetCoolTime(int skillNum);
    public event OnResetCoolTime onResetCoolTime;
    public delegate void OnSetItemDescription(int skillNum);
    public event OnSetItemDescription onSetItemDescription;
    public delegate void OnSetSkillDescription();
    public event OnSetSkillDescription onSetSkillDescription;
    private Vector2 createPoint = new Vector2(130, 90);
    private Vector2[] itemCreatePoint = { new Vector2(720, 60), new Vector2(885, 60), new Vector2(1045, 60), new Vector2(1200, 60)};

    private void Update()
    {

        if (Input.mousePosition.x > 60 && Input.mousePosition.x < 180 && Input.mousePosition.y > 25 && Input.mousePosition.y < 180)
        {
            isCanSkillDescription = true;
            SetSkillDescription();
        }
        else
        {
            isCanSkillDescription = false;
            SetSkillDescription();
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
            if(itemNum==5)
            {
                if(itemDescription[0]!=null) itemDescription[0].SetActive(false);
                if (itemDescription[1] != null) itemDescription[1].SetActive(false);
                if (itemDescription[2] != null) itemDescription[2].SetActive(false);
                if (itemDescription[3] != null) itemDescription[3].SetActive(false);
                if (onSetItemDescription != null) onSetItemDescription(5);
            }
        }
    }
    public void SetItemIcon(int itemType,int itemNum)
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

    public void SetSkillDescription()
    {
        if (isCanSkillDescription == true)
        {
            if (setSkillDescription == false)
            {
                Debug.Log("true");
                setSkillDescription = true;
                skillDescription.SetActive(true);
                if (onSetSkillDescription != null) onSetSkillDescription();
            }
        }
        else if (setSkillDescription == true)
        {
            Debug.Log("False");
            setSkillDescription = false;
            skillDescription.SetActive(false);
            if (onSetSkillDescription != null) onSetSkillDescription();
        }
    }
    //  Vector3 IconPos= Camera.main.WorldToScreenPoint(Vector3.zero);
    public void SetSkillIcon(int skillNum)
    {
        skillUI = Instantiate(skillIcon[skillNum], createPoint, Quaternion.identity, GameObject.Find("Canvas").transform);
        skillUI.transform.SetParent(skillIconP.transform);
        skillDescription = skillUI.transform.GetChild(0).gameObject;
        skillCoolTime = skillUI.transform.GetChild(1).gameObject;
        skillCoolTimeText = skillCoolTime.GetComponent<TextMeshProUGUI>();
    }

    //Object that called the skill cooldown to the UI manager, cooldown time
    public void SkillCooltime(int time, int skillNum)
    {
        // Change the color of the icon to be dimmed to give it an inactive feel.
        skillUI.GetComponent<RawImage>().color = new Color(160 / 255f, 160 / 255f, 160 / 255f);
        // Change the cool-time text to the max value of the cool-time.
        skillCoolTimeText.text = time.ToString();
        // Execute the cool-time coroutine and wait for the cool-time time.
        StartCoroutine(SkillCooltime_Count(time, skillNum));

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
