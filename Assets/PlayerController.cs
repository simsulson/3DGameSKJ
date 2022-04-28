using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private Transform player;
    [SerializeField] private Transform cam;
    [SerializeField] private float speed = 4f;

    private float horizontal = 0;
    private float vertical = 0;
    private Vector3 moveDir;
    float gravityY = .0f;

    [SerializeField] private float angleX;
    [SerializeField] private float angleY;

    float rayDistance = 20f;

    void Awake()
    {
        controller = player.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        PlayerMove();
        PlayerRotate();
        PlayerShoot();
    }

    private void PlayerMove()
    {
        if (controller.isGrounded)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gravityY = 6f;
            }
        }
        else
        {
            gravityY += Physics.gravity.y * Time.deltaTime;
        }
        moveDir = new Vector3(horizontal, 0, vertical);

        if (moveDir.magnitude > 1)
        {
            moveDir.Normalize();
        }
        moveDir.y = gravityY;

        speed = 4f;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = 1f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 8f;
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            speed = .0f;
            Debug.Log("움직일 수 없습니다!");
        }

        controller.Move(Quaternion.Euler(new Vector3(0, angleY, 0)) * moveDir * Time.deltaTime * speed);
    }

    private void PlayerRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        angleX -= mouseY;
        angleY += mouseX;

        cam.position = player.position;
        cam.rotation = Quaternion.Euler(angleX, angleY, 0);
    }

    private void PlayerShoot()
    {
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(cam.position, cam.forward);
            
            Debug.Log("발사!");
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.name == "Target")
                {
                    Destroy(hit.collider.gameObject);
                    Debug.Log("명중!");
                }
            }
        }
    }
}
