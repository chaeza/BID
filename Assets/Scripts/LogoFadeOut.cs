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
        for(int i=100; i>0;i++)
        {
            yield return new WaitForSeconds(0.01f);
            tempColor.a -= 0.01f;
            darkHole.color = tempColor;
            yield return null;
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
