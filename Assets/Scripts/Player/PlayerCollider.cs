using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerAnimation = GetComponentInParent<PlayerAnimation>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.collider.tag.ToString());
        if (collision.collider.CompareTag("Ground"))
        {
            playerMovement.ResetJump();
            playerAnimation.TriggerGroundAnimation();
        }
    }
}
