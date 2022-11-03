using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoFadeOut : MonoBehaviour
{
    [SerializeField] private RawImage logoImage;
    [SerializeField] private bool fadeout = false;
    [SerializeField] private bool fadein = true;
    [SerializeField] private bool isPlaying = false;
    [SerializeField] private float fadeTime;


    private void Start()
    {
        //페이드아웃을 원할 때
        if (fadeout == true && isPlaying == false) StartCoroutine(FadeOut());
           if (fadeout == false && fadein == true) StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        logoImage.gameObject.SetActive(true);
        isPlaying = true;
        Color tempColor = logoImage.color;
        tempColor.a = 0f;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeTime;
            logoImage.color = tempColor;

            if (tempColor.a >= 1f) tempColor.a = 1f;

            yield return null;
        }
        fadeout = false;

     
    }
    IEnumerator FadeIn()
    {
        Color tempColor = logoImage.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeTime;
            logoImage.color = tempColor;

            if (tempColor.a <= 0f)
            {
                tempColor.a = 0f;
                logoImage.color = tempColor;
            }

            yield return null;
        }

        logoImage.gameObject.SetActive(false);
        fadein = false;
        isPlaying = false;
    }
}
