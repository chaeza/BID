using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class UIMgr : MonoBehaviourPun
{   // Save the object that called the skill cooldown
    [Header("Skill Icon")]
    [SerializeField] private GameObject[] skillIcon;
    [SerializeField] private GameObject skillSilenceIcon;
    [SerializeField] private GameObject[] itemIcon;
    [SerializeField] private GameObject[] skillIconP;
    [SerializeField] private GameObject[] itemIconP;
    [SerializeField] private GameObject silenceIconP;
    [Header("Ending Images")]
    public GameObject winLogo;
    public GameObject loseLogo;
    [Header("Game UI")]
    public GameObject ESC;
    public GameObject TAB;
    [SerializeField] private Text[] playerNameText;
    [SerializeField] private Text[] playerKillText;
    [SerializeField] private Text[] playerInfoText;
    [SerializeField] private float screenRatioX;
    [SerializeField] private float screenRatioY;
    private int[] KillCount = new int[5];

    [SerializeField] private GameObject blackUI;
    [SerializeField] private Image HPUI;
    private TextMeshProUGUI[] skillCoolTimeText = new TextMeshProUGUI[2];
    private GameObject[] skillCoolTime = new GameObject[2];
    private GameObject[] skillUI = new GameObject[2];
    private GameObject[] skillDescription = new GameObject[2];
    private GameObject[] itemUI = new GameObject[4];
    private GameObject[] itemDescription = new GameObject[4];
    private GameObject[] skillSilence = new GameObject[2];
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

    private void Awake()
    {
        blackUI.SetActive(true);
        skillSilence[0] = Instantiate(skillSilenceIcon, skillIconP[0].transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        skillSilence[1] = Instantiate(skillSilenceIcon, skillIconP[1].transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        skillSilence[0].transform.SetParent(silenceIconP.transform);
        skillSilence[1].transform.SetParent(silenceIconP.transform);
        screenRatioX = Screen.width / 1920f;
        screenRatioY = Screen.height / 1080f;
    }
    private void FixedUpdate()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (GameMgr.Instance.playerInput.Esc == KeyCode.Escape)
        {
            ESC.SetActive(true);
        }
        else if (ESC.activeSelf == true)
        {
            ESC.SetActive(false);
        }
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Tab)
        {
            TAB.SetActive(true);

        }
        else if (TAB.activeSelf == true)
        {
            TAB.SetActive(false);
        }
        if (Input.mousePosition.x > 60 * screenRatioX && Input.mousePosition.x < 180 * screenRatioX && Input.mousePosition.y > 25 * screenRatioY && Input.mousePosition.y < 180 * screenRatioY)
        {
            isCanSkillDescription[0] = true;
            SetSkillDescription(0);
        }
        else
        {
            isCanSkillDescription[0] = false;
            SetSkillDescription(0);
        }
        if (Input.mousePosition.x > 680 * screenRatioX && Input.mousePosition.x < 760 * screenRatioX && Input.mousePosition.y > 25 * screenRatioY && Input.mousePosition.y < 155 * screenRatioY && GameMgr.Instance.inventory.InvetoryCount(0) == false)
        {
            isCanItemDescription = true;
            SetItemDescription(0);
        }
        else if (Input.mousePosition.x > 845 * screenRatioX && Input.mousePosition.x < 915 * screenRatioX && Input.mousePosition.y > 25 * screenRatioY && Input.mousePosition.y < 155 * screenRatioY && GameMgr.Instance.inventory.InvetoryCount(1) == false)
        {
            isCanItemDescription = true;
            SetItemDescription(1);
        }
        else if (Input.mousePosition.x > 1005 * screenRatioX && Input.mousePosition.x < 1080 * screenRatioX && Input.mousePosition.y > 25 * screenRatioY && Input.mousePosition.y < 155 * screenRatioY && GameMgr.Instance.inventory.InvetoryCount(2) == false)
        {
            isCanItemDescription = true;
            SetItemDescription(2);
        }
        else if (Input.mousePosition.x > 1160 * screenRatioX && Input.mousePosition.x < 1240 * screenRatioX && Input.mousePosition.y > 25 * screenRatioY && Input.mousePosition.y < 155 * screenRatioY && GameMgr.Instance.inventory.InvetoryCount(3) == false)
        {
            isCanItemDescription = true;
            SetItemDescription(3);
        }
        else if (setItemDescription == true)
        {
            isCanItemDescription = false;
            SetItemDescription(5);
        }
        if (Input.mousePosition.x > 292 * screenRatioX && Input.mousePosition.x < 404 * screenRatioX && Input.mousePosition.y > 15 * screenRatioY && Input.mousePosition.y < 186 * screenRatioY)
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
    string infoName;
    RawImage c1;
     Color c2;
    public void SetBlackUI()
    {
        c1= blackUI.GetComponent<RawImage>();
        c2 = c1.color;
        StartCoroutine(SetBlackUI_FadeIn());
    }
    public void SetHPUI(float value)
    {
        HPUI.fillAmount= value/100;
    }
    IEnumerator SetBlackUI_FadeIn()
    {
        for(int i = 0; i<100;i++)
        {

            yield return new WaitForSeconds(0.01f);
            c2.a -= 0.01f;
            c1.color = c2;
        }
        blackUI.SetActive(false);

    }
    public void PlayInfoChange(int infoNum, float infoValue)
    {
        if (infoNum == 0)
        {
            infoName = "AttackPower : ";
        }
        else if (infoNum == 1)
        {
            infoName = "AttackRange : ";
        }
        else if (infoNum == 2)
        {
            infoName = "AttackSpeed : ";
        }
        else if (infoNum == 3)
        {
            infoName = "MoveSpeed : ";
        }
        else if (infoNum == 4)
        {
            infoName = "DamageDecrease : ";
        }
        playerInfoText[infoNum].text = infoName + infoValue;
    }

    public void TabUpDate(int PlayerNum, state alive)
    {
        if (alive == state.None)
            playerNameText[PlayerNum].text = PhotonNetwork.PlayerList[PlayerNum].NickName;
        else playerNameText[PlayerNum].color = Color.red;
    }
    public void KillUpDate(int Num)
    {
        KillCount[Num]++;
        playerKillText[Num].text = $"KILL : {KillCount[Num]}";
    }
    public void EndGame(bool win)
    {
        if (win == true)
            winLogo.SetActive(true);
        else if (win == false)
            loseLogo.SetActive(true);
    }
    IEnumerator EndGame_Delay()
    {
        yield return new WaitForSeconds(4f);
        Debug.Log("엔딩화면 딜레이 ");
        // GameMgr.Instance.gameSceneLogic.WinnerEndGame();
    }
    public void SetSilence(bool set)
    {
        skillSilence[0].SetActive(set);
        skillSilence[1].SetActive(set);
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
        itemUI[itemNum] = Instantiate(itemIcon[itemType], itemIconP[itemNum].transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        itemUI[itemNum].transform.SetParent(itemIconP[itemNum].transform);
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
        skillUI[num] = Instantiate(skillIcon[skillNum], skillIconP[num].transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        if (skillUI[num] == null)
            Debug.LogError("널인데요??");
        skillUI[num].transform.SetParent(skillIconP[num].transform);
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
