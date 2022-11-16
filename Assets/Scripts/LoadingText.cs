using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    public Text[] text;
    public RawImage[] image;

    int ran;
    int imageRan;

    private void Start()
    {
        ran = Random.Range(0, text.Length);
        imageRan = Random.Range(0, image.Length);

        text[ran].gameObject.SetActive(true);
        image[imageRan].gameObject.SetActive(true);


    }
}
