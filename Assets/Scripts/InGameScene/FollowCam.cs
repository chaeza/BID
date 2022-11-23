using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FollowCam : MonoBehaviour
{
    [SerializeField] private float distanceFromPlayerZ;
    [SerializeField] private float distanceFromPlayerY;
    [SerializeField] private float distanceFromPlayerX;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float screenRatioX;
    [SerializeField] private float screenRatioY;
    [SerializeField] private ViewScreen viewScreen;
    [SerializeField] private Vector2 mousePos;
    private Portal[] portals;
    private Transform playerPos;
    private bool followBool = false;
    private float playerY;
    private float ratioX = 0.96794920964835228280278942788686f;
    private float ratioY = 1.0809775369401246252376416055294f;
    private Vector3 forwardDir = new Vector3(0.64f, 0, -6.7784f).normalized;
    private Vector3 rightDir = new Vector3(-5.5936f, 0, -0.26f).normalized;

    public void SetPlayerPos(Transform player)
    {
        playerPos = player;
        transform.position = playerPos.position + Vector3.forward * distanceFromPlayerZ + Vector3.up * distanceFromPlayerY + Vector3.right * distanceFromPlayerX;
        playerY = playerPos.transform.position.y;
        transform.LookAt(playerPos.position + Vector3.up * 2);
    }
    public void SetCameraResetting()
    {
        if (followBool == false)
        {
            followBool = true;
            Invoke("ReSetCameraResetting", 0.1f);
        }
    }
    void ReSetCameraResetting() => followBool = false;

    private void Awake()
    {
        viewScreen = FindObjectOfType<ViewScreen>();
        portals = FindObjectsOfType<Portal>();
        screenRatioX = Screen.width / 1920f;
        screenRatioY = Screen.height / 1080f;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].cameraReSetting = SetCameraResetting;
        }
    }

    private void Update()
    {
        if (GameMgr.Instance.GameState == false) return;

        if (Input.GetKey(KeyCode.Space) || followBool == true)
        {
            transform.position = new Vector3(playerPos.position.x, playerY, playerPos.position.z) + Vector3.forward * distanceFromPlayerZ + Vector3.up * distanceFromPlayerY + Vector3.right * distanceFromPlayerX;
        }
        // Click on the mini map
        if (Input.GetKey(KeyCode.Mouse0) && Input.mousePosition.x > 1627 * screenRatioX && Input.mousePosition.x < 1871 * screenRatioX & Input.mousePosition.y > 9 * screenRatioY && Input.mousePosition.y < 252 * screenRatioY)
        {
            mousePos.x = Input.mousePosition.x / screenRatioX - 1627.772f;
            mousePos.y = Input.mousePosition.y / screenRatioY - 9.326418f;
            transform.position = new Vector3(502.5f - mousePos.x * ratioX + distanceFromPlayerX, transform.position.y, 472.1f - mousePos.y * ratioY + distanceFromPlayerZ);
        }
        if (GameMgr.Instance.playerInput.yKey == KeyCode.Y && followBool == false) followBool = true;
        else if (GameMgr.Instance.playerInput.yKey == KeyCode.Y && followBool == true) followBool = false;
        if (followBool == false)
        {
            // 오른쪽
            if (Input.mousePosition.x >= 1909 * screenRatioX && viewScreen.transform.localPosition.x <= 90)
            {
                transform.position = transform.position + rightDir * cameraSpeed;
            }
            // 왼쪽
            else if (Input.mousePosition.x <= 5 * screenRatioX && viewScreen.transform.localPosition.x >= -90)
            {
                transform.position = transform.position - rightDir * cameraSpeed;
            }
            // 위쪽
            if (Input.mousePosition.y >= 1079 * screenRatioY && viewScreen.transform.localPosition.y <= 103)
            {
                transform.position = transform.position + forwardDir * cameraSpeed;
            }
            // 아래쪽
            else if (Input.mousePosition.y <= 3 * screenRatioY && viewScreen.transform.localPosition.y >= -103)
            {
                transform.position = transform.position - forwardDir * cameraSpeed;
            }
        }
    }
}
