using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class FollowCam : MonoBehaviour
{
    [SerializeField] float distanceFromPlayerZ = 3f;
    [SerializeField] float distanceFromPlayerY = 15f;
    [SerializeField] float cameraSpeed = 0.2f;
    [SerializeField] private Transform playerPos;
    bool followBool = false;


    public void PlayerStart(Transform player)
    {
        playerPos = player;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        transform.position = playerPos.position - Vector3.forward * distanceFromPlayerZ + Vector3.up * distanceFromPlayerY;
        transform.LookAt(playerPos.position + Vector3.up * 2);

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) || followBool == true)
        {
            transform.position = playerPos.position - Vector3.forward * distanceFromPlayerZ + Vector3.up * distanceFromPlayerY;
            transform.LookAt(playerPos.position + Vector3.up * 2);
        }
        if (GameMgr.Instance.playerInput.yKey == KeyCode.Y && followBool == false) followBool = true;
        else if (GameMgr.Instance.playerInput.yKey == KeyCode.Y && followBool == true) followBool = false;
        if (followBool == false)
        {
            //Full HD ±âÁØ 1920/1080
            if (Input.mousePosition.x >= 1890)//&& transform.position.x <= 50)
            {
                transform.position = transform.position + Vector3.right * cameraSpeed;
            }
            if (Input.mousePosition.x <= 10)//&& transform.position.x >= -50)
            {
                transform.position = transform.position - Vector3.right * cameraSpeed;
            }
            if (Input.mousePosition.y >= 1050)// && transform.position.z <= 50)
            {
                transform.position = transform.position + Vector3.forward * cameraSpeed;
            }
            if (Input.mousePosition.y <= 5)// && transform.position.z >= -50)
            {
                transform.position = transform.position - Vector3.forward * cameraSpeed;
            }
        }
    }
}
