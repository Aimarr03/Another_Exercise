using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidBody;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private int maxJump = 2;
    private int currentJumpCount;
    private float currentJumpForce;
    private float y_axis;

    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
    }
    void Start()
    {
        PlayerInput.instance.JumpEvent += Instance_JumpEvent;
        currentJumpCount = maxJump;
        currentJumpForce = jumpForce;
    }

    private void Instance_JumpEvent()
    {
        if (currentJumpCount == 0) return;
        currentJumpCount--;
        playerRigidBody.velocity = Vector2.up * currentJumpForce;
        currentJumpForce *= 0.95f;
    }
    public void ResetJump()
    {
        currentJumpCount = maxJump;
        currentJumpForce = jumpForce;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 movementCoordination = PlayerInput.instance.GetMoveInput();
        //Debug.Log(movementCoordination);
        transform.position += movementSpeed * Time.deltaTime * ConvertVector2ToVector3Normalized(movementCoordination);
        SetVelocity();
    }
    private Vector3 ConvertVector2ToVector3Normalized(Vector2 input)
    {
        y_axis = input.y;
        input.y = 0;
        input = input.normalized;
        return input;
    }
    public Vector2 GetVector2PositionNormalized()
    {
        Vector2 movementCoordination = PlayerInput.instance.GetMoveInput().normalized;
        return movementCoordination;
    }
    private void SetVelocity()
    {
        float y_axis = playerRigidBody.velocity.y;
        playerAnimation.SetMovementOnYAxis(y_axis);
    }

}
