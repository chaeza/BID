using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class UIMgr : MonoBehaviourPun
{   // Save the object that called the skill cooldown
    GameObject cooltimeGameobject = null;
    // Save multiple skill types to this object
    GameObject skillUI = null;

    [Header("ÄðÅ¸ÀÓ")]
    [SerializeField] TextMeshProUGUI cooltimeText;

    //Object that called the skill cooldown to the UI manager, cooldown time
    public void SkillCooltime(GameObject my, int Cool)
    {

        // Save the called object object.
        cooltimeGameobject = my;
        // Change the color of the icon to be dimmed to give it an inactive feel.
        skillUI.GetComponent<Image>().color = new Color(160 / 255f, 160 / 255f, 160 / 255f);
        // Change the cool-time text to the max value of the cool-time.
        cooltimeText.text = Cool.ToString();
        // Execute the cool-time coroutine and wait for the cool-time time.
        StartCoroutine(Cooltime(Cool));

    }
    IEnumerator Cooltime(int Cool)
    {
        // Store the received cooldown time in i
        for (int i = Cool - 1; i >= 0; --i)
        {
            // wait 1 second
            yield return new WaitForSeconds(1f);
            // Decrease cooldown text by -1
            cooltimeText.text = i.ToString();
            yield return null;
        }

        // icon color original position
        skillUI.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        // Call ResetCooltime to the object that called the saved UI manager to use the skill again
        cooltimeGameobject.SendMessage("ResetCooltime", SendMessageOptions.DontRequireReceiver);
        // instead of disabling text, just print nothing
        cooltimeText.text = " ";
        yield break;
    }
}
