using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : Enemy
{
    
    public override void TakeDamage(int damage, Player player)
    {
        base.TakeDamage(damage, player);
        if(isDead) enemyRigidBody.gravityScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidBody.gravityScale = 0f;    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Debug.Log("Flying Eye is on the ground");
            animator.SetBool("IsGround", true);
        }
    }
}
