using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.position;
            playerPosition.y = transform.position.y;

            transform.position = playerPosition;
        }
    }
}