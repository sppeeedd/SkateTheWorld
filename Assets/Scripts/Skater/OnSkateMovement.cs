using System.Collections;
using UnityEngine;

public class OnSkateMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;

    private Rigidbody2D skaterRB;
    private Animator animator;
    private DirectionState directionState;
    private MoveState moveState;

    private float jumpLength;
    private const string idle = "Idle";
    private const string ride = "Ride";
    private const string jump = "Ollie";

    private void Start()
    {
        skaterRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveState = MoveState.Idle;
        directionState = DirectionState.Right;

        jumpLength = 2 * jumpForce * 0.02f / 9.81f;
    }

    private void FixedUpdate()
    {
        Ride();
    }

    private void Update()
    {
        Ollie();
        Stop();

        if (transform.localScale.x >= 0)
            directionState = DirectionState.Right;
        else directionState = DirectionState.Left;
    }

    private void Ride()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (skaterRB.velocity.x < maxSpeed)
                skaterRB.AddForce(new Vector2(moveForce, 0f));

            if (moveState != MoveState.Jump)
            {
                moveState = MoveState.Ride;
                if (directionState == DirectionState.Left)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    directionState = DirectionState.Right;
                }
                animator.Play(ride);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (skaterRB.velocity.x > -maxSpeed)
                skaterRB.AddForce(new Vector2(-moveForce, 0f));

            if (moveState != MoveState.Jump)
            {
                moveState = MoveState.Ride;
                if (directionState ==  DirectionState.Right)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    directionState = DirectionState.Left;
                }
                animator.Play(ride);
            }
        }
    }

    private void Ollie()
    {
        if (Input.GetKeyDown(KeyCode.Space) && moveState != MoveState.Jump)
        {
            moveState = MoveState.Jump;
            skaterRB.AddForce(new Vector2(0f, jumpForce));

            animator.Play(jump);

            StartCoroutine(EndJump());
        }
    }

    private void Stop()
    {
        if (moveState != MoveState.Jump)
        {
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                animator.Play(idle);
            }
        }
    }

    IEnumerator EndJump()
    {
        yield return new WaitForSeconds(jumpLength);
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
        Ride,
        Jump
    }
}