using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMoving : MonoBehaviour
{
    private bool intro = false;
    private float fixedDeltaTime;

    public Text text;

    private void Awake()
    {
        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;

    }

    private void Start()
    {
        StartCoroutine(Skip());
    }

    void Update()
    {

        if (intro == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {

                if (Time.timeScale == 1.0f)
                    Time.timeScale = 3.0f;
                else
                    Time.timeScale = 1.0f;
                // Adjust fixed delta time according to timescale
                // The fixed delta time will now be 0.02 real-time seconds per frame
                Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            }
        }
    }

    IEnumerator Skip()
    {
        yield return new WaitForSeconds(14f);
        text.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        intro = true;
    }
}
