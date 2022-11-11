using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FollowCam : MonoBehaviour
{
    [SerializeField] private float distanceFromPlayerZ;
    [SerializeField] private float distanceFromPlayerY;
    [SerializeField] private float distanceFromPlayerX;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private GameObject rayCamara;
    [SerializeField] private ViewScreen viewScreen;
    [SerializeField] private Vector2 mousePos;
    private Transform playerPos;
    private bool followBool = false;
    private float playerY;
    private float ratioX = 1.3206611570247933884297520661157f;
    private float ratioY = 1.2966942148760330578512396694215f;
    private Vector3 forwardDir = new Vector3(0.64f, 0, -6.7784f).normalized;
    private Vector3 rightDir = new Vector3(-5.5936f, 0, -0.26f).normalized;

    public void SetPlayerPos(Transform player)
    {
        playerPos = player;
        transform.position = playerPos.position + Vector3.forward * distanceFromPlayerZ + Vector3.up * distanceFromPlayerY + Vector3.right * distanceFromPlayerX;
        playerY = playerPos.transform.position.y;
        transform.LookAt(playerPos.position + Vector3.up * 2);
    }
    private void Awake()
    {
        viewScreen = FindObjectOfType<ViewScreen>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        //Debug.Log("mouse x : " + Input.mousePosition.x);
        //Debug.Log("mouse y : " + Input.mousePosition.y);
        //rayCamara.transform.position = playerPos.position + Vector3.up * 30;
        //rayCamara.transform.LookAt(playerPos.position);
        if (Input.GetKey(KeyCode.Space) || followBool == true)
        {
            transform.position = new Vector3(playerPos.position.x, playerY, playerPos.position.z) + Vector3.forward * distanceFromPlayerZ + Vector3.up * distanceFromPlayerY + Vector3.right * distanceFromPlayerX;
        }
        // Click on the mini map
        if (Input.GetKey(KeyCode.Mouse0) && Input.mousePosition.x > 1653 && Input.mousePosition.x < 1837 & Input.mousePosition.y > 32 && Input.mousePosition.y < 243)
        {
            mousePos.x = Input.mousePosition.x - 1623.024f;
            mousePos.y = Input.mousePosition.y - 15.24192f;
            transform.position = new Vector3(547.5f - mousePos.x * ratioX + distanceFromPlayerX, transform.position.y, 508.59f - mousePos.y * ratioY + distanceFromPlayerZ);
        }
        if (Input.GetKey(KeyCode.Y) && followBool == false) followBool = true;
        else if (GameMgr.Instance.playerInput.yKey == KeyCode.Y && followBool == true) followBool = false;
        if (followBool == false)
        {
            // ������
            if (Input.mousePosition.x >= 1890 && viewScreen.transform.localPosition.x <= 90)
            {
                transform.position = transform.position + rightDir * cameraSpeed;
            }
            // ����
            else if (Input.mousePosition.x <= 10 && viewScreen.transform.localPosition.x >= -90)
            {
                transform.position = transform.position - rightDir * cameraSpeed;
            }
            // ����
            if (Input.mousePosition.y >= 1050 && viewScreen.transform.localPosition.y <= 103)
            {
                transform.position = transform.position + forwardDir * cameraSpeed;
            }
            // �Ʒ���
            else if (Input.mousePosition.y <= 5 && viewScreen.transform.localPosition.y >= -103)
            {
                transform.position = transform.position - forwardDir * cameraSpeed;
            }
        }
    }
}
