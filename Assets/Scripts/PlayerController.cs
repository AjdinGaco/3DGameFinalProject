using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform playerHead;
    public Rigidbody playerBody;
    public float moveSpeed = 1000f;
    public float maxspeed = 100f;
    public float CameraSpeed = 3f;

    public bool grouded = false;
    //Jumping vars
    private bool jumping = false;
    private bool jumpReady = true;
    private float jumpCooldown = 0.20f;
    public float jumpforce = 100f;

    public KeyCode ControlForward = KeyCode.Z, ControlBack = KeyCode.S, ControlRight = KeyCode.D, ControlLeft = KeyCode.Q, ControlJump = KeyCode.Space;
    private bool noinput = false;
    private int CX = 0, CY = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        ReadController();
        CameraLook();
        Movement();
    }




    Vector2 rotation = new Vector2(0, 0);
    private void CameraLook()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        playerHead.transform.eulerAngles = (Vector2)rotation * CameraSpeed;

        //WILL FIX LATER? GAVE UP DUE TO MY OWN LAZYNESS
        playerBody.angularVelocity = new Vector3(0, 0, 0);
        playerBody.transform.rotation = Quaternion.Euler(0, rotation.y * CameraSpeed, 0);

    }
    private void ReadController()
    {
        if (Input.GetKey(ControlForward) && !Input.GetKey(ControlBack))
            CY = 1;
        else if (!Input.GetKey(ControlForward) && Input.GetKey(ControlBack))
            CY = -1;
        else
            CY = 0;

        if (Input.GetKey(ControlLeft) && !Input.GetKey(ControlRight))
            CX = -1;
        else if (!Input.GetKey(ControlLeft) && Input.GetKey(ControlRight))
            CX = 1;
        else
            CX = 0;
        if (Input.GetKeyDown(ControlJump))
            jumping = true;
        else
            jumping = false;

        if (CX == 0 && CY == 0 && !jumping)
            noinput = true;
        else
            noinput = false;
    }
    private void Movement()
    {
        //Better gravity
        playerBody.AddForce(Vector3.down * Time.deltaTime * 10);

        ContraMovement();

        if (jumping && grouded)
            Jump();

        float multiplier = 1;

        if (!grouded)
            multiplier = 0.5f;
        playerBody.AddForce(playerBody.transform.forward * CY * moveSpeed * Time.deltaTime * multiplier);
        playerBody.AddForce(playerBody.transform.right * CX * moveSpeed * Time.deltaTime * multiplier);
    }

    public Vector3 GetVelocity()
    {
        return playerBody.velocity;
    }
    private void ContraMovement()
    {
        if (noinput && grouded)
        {
            playerBody.velocity = playerBody.velocity * 0.9f;
        }

        if (playerBody.velocity.y > 0 && CY < 0)
            playerBody.AddForce(playerBody.transform.forward * Time.deltaTime * CY * 0.5f);
        if (playerBody.velocity.y < 0 && CY > 0)
            playerBody.AddForce(playerBody.transform.forward * Time.deltaTime * CY * 0.5f);

        if (playerBody.velocity.x > 0 && CX < 0)
            playerBody.AddForce(playerBody.transform.right * Time.deltaTime * CX * 0.5f);
        if (playerBody.velocity.x < 0 && CX > 0)
            playerBody.AddForce(playerBody.transform.right * Time.deltaTime * CX * 0.5f);

    }

    private void Jump()
    {
        if (jumpReady && grouded)
        {
            jumpReady = false;
            playerBody.AddForce(Vector2.up * jumpforce * 2f);
            // make it a bit smoother
            playerBody.AddForce(Vector2.up * jumpforce);


            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void ResetJump()
    {
        jumpReady = true;
    }

    public void AddForce(Vector3 AddedForce)
    {
        playerBody.AddForce(AddedForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            grouded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            grouded = false;

    }
}
