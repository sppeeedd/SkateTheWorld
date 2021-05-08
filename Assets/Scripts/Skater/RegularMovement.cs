using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;

    private Rigidbody2D skaterRB;
    private Animator animator;
    private DirectionState directionState;
    private MoveState moveState;

    private const float jumpAnimationLength = 1.283f;
    private const string walk = "Walk";
    private const string jump = "Jump";

    private void Start()
    {
        skaterRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveState = MoveState.Idle;
        directionState = DirectionState.Right;
    }

    private void Update()
    {
        Walk();
        Jump();
    }

    private void Walk()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (skaterRB.velocity.x < maxSpeed)
                skaterRB.AddForce(new Vector2(moveForce, 0f));
            if (moveState != MoveState.Jump)
            {
                moveState = MoveState.Walk;
                if (directionState == DirectionState.Left)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    directionState = DirectionState.Right;
                }
                animator.Play(walk);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (skaterRB.velocity.x > -maxSpeed)
                skaterRB.AddForce(new Vector2(-moveForce, 0f));
            if (moveState != MoveState.Jump)
            {
                moveState = MoveState.Walk;
                if (directionState == DirectionState.Right)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    directionState = DirectionState.Left;
                }
                animator.Play(walk);
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && moveState != MoveState.Jump)
        {
            moveState = MoveState.Jump;
            skaterRB.AddForce(new Vector2(0f, jumpForce));

            //animator.Play(jump);

            StartCoroutine(EndJump());
        }
    }

    IEnumerator EndJump()
    {
        yield return new WaitForSeconds(jumpAnimationLength);
        moveState = MoveState.Idle;
    }

    private enum DirectionState
    {
        Right,
        Left
    }
    private enum MoveState
    {
        Idle,
        Walk,
        Jump
    }
}
