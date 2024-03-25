using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private string movementConstraint = "movement";
    [SerializeField] private string movementY_AxisConstraint = "movement_y";
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer playerSprite;
    private PlayerMovement playerMovement;

    private float x_axis;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        
    }
    private void Start()
    {
        x_axis = playerMovement.GetVector2PositionNormalized().x;
    }

    private void Update()
    {
        Vector2 movementCoordination = playerMovement.GetVector2PositionNormalized();
        x_axis = movementCoordination.x;
        SetFloatForMovement(x_axis);
        FlipX(x_axis);
    }
    private void FlipX(float input)
    {
        if (input == 0) return;
        float rotation_y = x_axis > 0 ? 0: 180;
        transform.rotation = Quaternion.Euler(0, rotation_y, 0);
    }
    private void SetFloatForMovement(float input)
    {
        float result = input != 0 ? 1 : -1;
        playerAnimator.SetFloat(movementConstraint, result);
    }
    public void SetMovementOnYAxis(float input)
    {
        playerAnimator.SetFloat(movementY_AxisConstraint, input);
        if (input < 0) playerAnimator.SetTrigger("Fall");
    }
    public void TriggerGroundAnimation()
    {
        playerAnimator.SetTrigger("Ground");
    }
}
