using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 6;
    [SerializeField]
    private float runSpeed = 10;
    private float moveSpeed;

    private SurvivalStatsManager survivalStatsManager;

    Rigidbody rigidbody;
    Camera viewCamera;
    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
        survivalStatsManager = GetComponent<SurvivalStatsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
        //Run if pressing shift
        var stamina = survivalStatsManager.stamina;
        if (Input.GetKey(KeyCode.LeftShift) && !stamina.IsEmpty())
        {
            moveSpeed = runSpeed;
            stamina.StartDepletion();
            stamina.StopRecovery();
        }
        else
        {
            moveSpeed = walkSpeed;
            stamina.StartRecovery();
            stamina.StopDepletion();
        }
        
    }

    void FixedUpdate()
    {
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }
}
