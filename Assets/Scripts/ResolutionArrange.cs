using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionArrange : MonoBehaviour
{
    private void Start()
    {
        //Fixed Resolution at first
        SetResolution(); 
    }

    // Resolution Arrange Function 
    public void SetResolution()
    {
        //set user's Option setting value
        int setWidth = 1920; 
        int setHeight = 1080;
        //Device setting value
        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        //Use SetResolution function in Screen Class
        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        //if user's resolution setting width is smaller then Device resolution  setting width
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        {
            //new Width calculatere
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else // if user's resolution setting height is bigger then Device resolution  setting height
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }
}
