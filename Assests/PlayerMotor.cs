using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMotor : NetworkBehaviour
{
    private CharacterController controller;
    public GameObject player;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpheight = 3f;

    public FloatingJoystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        //MobMove();
    }
    public void ProcessMove(Vector2 input)
    {
        
        Vector3 moveDirection = Vector3.zero;

        
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        if (moveDirection != Vector3.zero)
        {
            //Debug.Log("is moving");
            player.GetComponent<Animator>().SetBool("run", true);
        }
        else
        {
            player.GetComponent<Animator>().SetBool("run", false);
        }


        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime; 
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
    }


    //public void MobMove()
    //{
    //    if (!IsOwner) return;
    //    Vector3 moveDirection = Vector3.zero;


    //    moveDirection.x = joystick.Horizontal;
    //    moveDirection.z = joystick.Vertical;
    //    if (moveDirection != Vector3.zero)
    //    {
    //        //Debug.Log("is moving");
    //        player.GetComponent<Animator>().SetBool("run", true);
    //    }
    //    else
    //    {
    //        player.GetComponent<Animator>().SetBool("run", false);
    //    }


    //    controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
    //    playerVelocity.y += gravity * Time.deltaTime;
    //    if (isGrounded && playerVelocity.y < 0)
    //        playerVelocity.y = -2f;
    //    controller.Move(playerVelocity * Time.deltaTime);
    //}


    public void jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpheight * -3f * gravity);
        }
    }
}
