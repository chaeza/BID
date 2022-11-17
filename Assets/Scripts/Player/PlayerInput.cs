using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public KeyCode inputKey { get; private set; }
    public KeyCode inputKey2 { get; private set; }

    //Emotion Key
    public KeyCode emotionKey1 { get; private set; }
    public KeyCode emotionKey2 { get; private set; }

    public KeyCode yKey { get; private set; }
    public KeyCode Esc { get; private set; }
    private bool escDown;
    public bool gameStartFaidIn;
    void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        if (gameStartFaidIn == false) return;
        if (Input.GetKeyDown(KeyCode.Y)) yKey = KeyCode.Y;
        else yKey = KeyCode.Alpha0;

        #region Emotion & Feel Eff
        //이모션 및 감정표현 왼쪽 Ctrl, Shift 키
        if (Input.GetKey(KeyCode.LeftControl)) emotionKey1 = KeyCode.LeftControl;
        else if (Input.GetKey(KeyCode.LeftShift)) emotionKey1 = KeyCode.LeftShift;
        else emotionKey1 = KeyCode.Alpha0;
        //감정표현 키 T
        if (Input.GetKey(KeyCode.Alpha1)) emotionKey2 = KeyCode.Alpha1;
        else if(Input.GetKey(KeyCode.Alpha2)) emotionKey2 = KeyCode.Alpha2;
        else if(Input.GetKey(KeyCode.Alpha3)) emotionKey2 = KeyCode.Alpha3;
        else if(Input.GetKey(KeyCode.Alpha4)) emotionKey2 = KeyCode.Alpha4;
        //else if () 로 이모션 추가 
        else emotionKey2 = KeyCode.Alpha0;
        #endregion

        if (Input.GetKey(KeyCode.Mouse1)) inputKey2 = KeyCode.Mouse1;
        else inputKey2 = KeyCode.Alpha0;

        if (Input.GetKey(KeyCode.Mouse0)) inputKey = KeyCode.Mouse0;
        else if (Input.GetKeyDown(KeyCode.Q)) inputKey = KeyCode.Q;
        else if (Input.GetKeyDown(KeyCode.W)) inputKey = KeyCode.W;
        else if (Input.GetKeyDown(KeyCode.E)) inputKey = KeyCode.E;
        else if (Input.GetKeyDown(KeyCode.R)) inputKey = KeyCode.R;
        else if (Input.GetKeyDown(KeyCode.A)) inputKey = KeyCode.A;
        else if (Input.GetKeyDown(KeyCode.S)) inputKey = KeyCode.S;
        else if (Input.GetKeyDown(KeyCode.D)) inputKey = KeyCode.D;
        else if (Input.GetKeyDown(KeyCode.F)) inputKey = KeyCode.F;
        else if (Input.GetKey(KeyCode.Tab)) inputKey = KeyCode.Tab;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) inputKey = KeyCode.Alpha2;
        else if (Input.GetKeyDown(KeyCode.Alpha1)) inputKey = KeyCode.Alpha1;
        else if (Input.GetKeyDown(KeyCode.LeftShift)) inputKey = KeyCode.LeftShift;
        else inputKey = KeyCode.Alpha0;
        if (Input.GetKeyDown(KeyCode.Escape) && escDown == false)
        {
            Esc = KeyCode.Escape;
            escDown = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            escDown = false;
            Esc = KeyCode.Alpha0;
        }
    }
}
