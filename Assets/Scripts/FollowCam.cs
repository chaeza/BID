using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FollowCam : MonoBehaviour
{
    [SerializeField] private float distanceFromPlayerZ;
    [SerializeField] private float distanceFromPlayerY;
    [SerializeField] private float distanceFromPlayerX;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Transform playerPos;
    [SerializeField] private GameObject rayCamara;
    private bool followBool = false;
    private float playerY;



    public void SetPlayerPos(Transform player)
    {
        playerPos = player;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        transform.position = playerPos.position +Vector3.forward * distanceFromPlayerZ + Vector3.up * distanceFromPlayerY+ Vector3.right* distanceFromPlayerX;
        playerY = playerPos.transform.position.y;
        transform.LookAt(playerPos.position + Vector3.up * 2);

    }

    private void Update()
    {
        rayCamara.transform.position = playerPos.position+ Vector3.up * 30;
        rayCamara.transform.LookAt(playerPos.position);
        if (Input.GetKey(KeyCode.Space) || followBool == true)
        {
            transform.position = new Vector3(playerPos.position.x,playerY,playerPos.position.z) + Vector3.forward * distanceFromPlayerZ + Vector3.up * distanceFromPlayerY + Vector3.right * distanceFromPlayerX;
        }
        if (GameMgr.Instance.playerInput.yKey == KeyCode.Y && followBool == false) followBool = true;
        else if (GameMgr.Instance.playerInput.yKey == KeyCode.Y && followBool == true) followBool = false;
        if (followBool == false)
        {
            if (Input.mousePosition.x >= 1890)
            {
                transform.position = transform.position - Vector3.right * cameraSpeed;
            }
            else if (Input.mousePosition.x <= 10)
            {
                transform.position = transform.position + Vector3.right * cameraSpeed;
            }
            if (Input.mousePosition.y >= 1050)
            {
                transform.position = transform.position - Vector3.forward * cameraSpeed;
            }
            else if (Input.mousePosition.y <= 5)
            {
                transform.position = transform.position + Vector3.forward * cameraSpeed;
            }
        }
    }
}
