using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float maxHeight = 9.5f;
    [SerializeField]
    private float minHeight = 9f;
    [SerializeField]
    private float growthRate = 0.025f;
    [SerializeField]
    private float fallRate = 0.010f;
    private PlayerController playerController;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            PlayerRelativeMovement();
        }
        MouseRelativeMovement();
    }

    void PlayerRelativeMovement()
    {
        Vector3 playerPosition = player.transform.position;
        playerPosition.y = transform.position.y;

        transform.position = playerPosition;
        if (playerController.isRunning)
        {
            //Increase ortographic size
            float size = Mathf.Clamp(GetComponent<Camera>().orthographicSize + growthRate, minHeight, maxHeight);
            GetComponent<Camera>().orthographicSize = size;
        }
        else
        {
            float size = Mathf.Clamp(GetComponent<Camera>().orthographicSize - fallRate, minHeight, maxHeight);
            GetComponent<Camera>().orthographicSize = size;
        }
    }

    void MouseRelativeMovement()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
        Vector3 playerPos = player.transform.position;

        // Slightly moves the camera towards the mouse position
        Vector3 cameraPos = Vector3.Lerp(transform.position, mousePos, 0.025f);
        //Smooth
        cameraPos.x = Mathf.Clamp(cameraPos.x, playerPos.x - 1.5f, playerPos.x + 1.5f);
        cameraPos.z = Mathf.Clamp(cameraPos.z, playerPos.z - 1.5f, playerPos.z + 1.5f);
        cameraPos.y = transform.position.y;
        transform.position = cameraPos;

    }

}