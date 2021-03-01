using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Enemy, Boss
}

public class PlayerMovement : MonoBehaviour
{
    // Update is called once per frame
    public float movSpeed;
    public float jumpHeight;
    public Animator animator;
    //public Rigidbody rb;
    public CharacterController controller;
    Vector3 movement;
    public float gravityValue;

    public event Action<EnemyType> OnEncountered;

    public void HandleUpdate()
    {
        bool groundedPlayer = controller.isGrounded;

        if (groundedPlayer && movement.y < 0)
        {
            movement.y = 0f;
        }
        movement.x = Input.GetAxisRaw("Horizontal") * movSpeed;
        movement.z = Input.GetAxisRaw("Vertical") * movSpeed;

        if (groundedPlayer && Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jump", true);
            groundedPlayer = false;
            movement.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        movement.y += gravityValue * Time.deltaTime;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.z);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (groundedPlayer)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Grounded", true);
        }
        else
        {
            //animator.SetBool("Jump", true);
            animator.SetBool("Grounded", false);
        }

        controller.Move(movement * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            movement.x = 0;
            movement.z = 0;
            animator.SetFloat("Speed", 0);
            collision.gameObject.SetActive(false);
            OnEncountered(EnemyType.Enemy);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            movement.x = 0;
            movement.z = 0;
            animator.SetFloat("Speed", 0);
            collision.gameObject.SetActive(false);
            OnEncountered(EnemyType.Boss);
        }
    }
}
