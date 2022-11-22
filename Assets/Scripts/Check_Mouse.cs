using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Check_Mouse : MonoBehaviour
{
    public TMPro.TextMeshProUGUI screen_width;
    public TMPro.TextMeshProUGUI screen_height;
    public TMPro.TextMeshProUGUI mouse_x;
    public TMPro.TextMeshProUGUI mouse_y;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        screen_width.text = Screen.width.ToString();
        screen_height.text = Screen.height.ToString();
        mouse_x.text = Input.mousePosition.x.ToString();
        mouse_y.text = Input.mousePosition.y.ToString();

    }
}
