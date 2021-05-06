using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScript : MonoBehaviour
{
    public Transform cam;
    public float moveSpeed = 20f;
    public float jumpSpeed = 0.6f;
    public float smoothTurn = 0.1f;
    public float gravity = 5f;
    public float runningSpeed = 10f;
    public Animator animator;
    float vSpeed = 0f;
    float smoothTurnVelocity;
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();  
        //TODO: ENABLE FOR RELEASE
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene("MainMenu");
        }

        if(controller.isGrounded)
        {
            vSpeed = 0;
            if (Input.GetKeyDown("space"))
            {
                vSpeed = jumpSpeed;
            }
        }

        vSpeed -= gravity * Time.deltaTime;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            float newSpeed = moveSpeed;
            animator.SetBool("isWalking", true);
            if (Input.GetKey("left shift"))
            {
                animator.SetBool("isRunning", true);
                //transform.Rotate(0,10,0, Space.Self); TEMP FIX
                //transform.rotation = Quaternion.Euler(0, -10, 0);
                newSpeed = runningSpeed;
            } else {
                animator.SetBool("isRunning", false);
            }
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, smoothTurn);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.y = vSpeed;
            controller.Move(moveDir.normalized * newSpeed * Time.deltaTime);
        } else {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            Vector3 moveDir = new Vector3(0f, vSpeed, 0f).normalized;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        CheckAttack();
    }

    private void CheckAttack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.CrossFade("attack", 0.5f);
        }
    }
}
