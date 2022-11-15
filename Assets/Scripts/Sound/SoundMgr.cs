using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMgr : MonoBehaviour
{
    public AudioMixer audioMixer;   //»ç¿îµå

    public Slider BgmSlider;
    public Slider SfxSlider;


    public void SetBGM()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
    }

    public void SetSFX()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(SfxSlider.value) * 20);
    }
}
