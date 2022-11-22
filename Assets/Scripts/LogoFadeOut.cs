using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoFadeOut : MonoBehaviour
{
    [SerializeField] private RawImage logoImage;
    [SerializeField] private RawImage darkHole;

    [SerializeField] private bool fadeout = false;
    [SerializeField] private bool fadein = true;
    [SerializeField] private bool isPlaying = false;
    [SerializeField] private float fadeTime;


    private void Start()
    {
        //페이드아웃을 원할 때
        if (fadeout == true && isPlaying == false) StartCoroutine(FadeOut(logoImage));
           if (fadeout == false && fadein == true) StartCoroutine(FadeIn(logoImage));
    }

    public void DarkHoleFadeOut()
    {
        Debug.Log(" 사ㅣㄹ해");
        StartCoroutine(DarkHole());

    }
    IEnumerator DarkHole()
    {
        Color tempColor = darkHole.color;
        for (int i = 90; i > 0; i--)
        {
            yield return new WaitForSeconds(0.02f);
            tempColor.a -= 0.01f;
            darkHole.color = tempColor;
            //yield return null;
        }
        darkHole.gameObject.SetActive(false);
    }
    Image[] lobby;
    Text[] lobbyText;
    RawImage rawImage;
    public void LobbyFadeIn(GameObject obj)
    {
        rawImage= obj.GetComponentInChildren<RawImage>();
        lobby = obj.GetComponentsInChildren<Image>();
        lobbyText = obj.GetComponentsInChildren<Text>();
        StartCoroutine(Lobby(obj));

    }
    IEnumerator Lobby(GameObject obj)
    {

        Color[] color1 = new Color[lobby.Length];
        Color[] color2 = new Color[lobbyText.Length];
        Color color3 = new Color();

        for(int i = 0; i<color1.Length;i++)
        {
            color1[i] = lobby[i].color;
            color2[i] = lobbyText[i].color;
            color3 = rawImage.color;
            color1[i].a =0f;
            color2[i].a =0f;
            color3.a =0f;
            lobby[i].color = color1[i];
            lobbyText[i].color = color2[i];
            rawImage.color = color3;

        }
        for (int i = 100; i > 0; i--)
        {
            yield return new WaitForSeconds(0.015f);
            for (int j = 0; j < color1.Length; j++)
            {
                color1[j].a += 0.01f;
                color2[j].a += 0.01f;
                color3.a += 0.01f;
                lobby[j].color = color1[j];
                lobbyText[j].color = color2[j];
                rawImage.color = color3;
            }
        }
        for (int j = 0; j < color1.Length; j++)
        {
            color1[j].a = 1f;
            color2[j].a = 1f;
            color3.a = 1f;
            lobby[j].color = color1[j];
            lobbyText[j].color = color2[j];
            rawImage.color = color3;
        }
    }
    IEnumerator FadeOut(RawImage ri)
    {
        
        ri.gameObject.SetActive(true);
        isPlaying = true;
        Color tempColor = ri.color;
        tempColor.a = 0f;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeTime;
            ri.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }
        fadeout = false;

     
    }
    IEnumerator FadeIn(RawImage ri)
    {
        Color tempColor = ri.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeTime;
            ri.color = tempColor;

            if (tempColor.a <= 0f)
            {
                tempColor.a = 0f;
                ri.color = tempColor;
            }

            yield return null;
        }

        ri.gameObject.SetActive(false);
        fadein = false;
        isPlaying = false;
    }
}
