using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GoblinCombat : MonoBehaviour
{
    [SerializeField] private Vector2 AggroAreaDetection;
    [SerializeField] private Vector2 AttackAreaDetection;
    [SerializeField] private Vector2 AttackRange;

    [SerializeField] private LayerMask playerMask;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackSpeedInterval;
    private float currentAttackSpeed;
    
    private int takingHitMax;
    private bool currentlyPreparingAttack;
    private bool currentlyAttacking;

    private Goblin goblin;
    private Player currentPlayer;
    private void Awake()
    {
        currentAttackSpeed = attackSpeedInterval;
        currentlyPreparingAttack = false;
        currentlyAttacking = false;
        goblin = GetComponent<Goblin>();
        goblinState = GoblinState.Idle;
    }
    public enum GoblinState
    {
        Idle,
        Agro,
        Attack,
    }
    [SerializeField] private GoblinState goblinState;

    private void Update()
    {
        if (goblin.IsDead()) return;
        if(goblin.GetIsDodging()) return;
        switch (goblinState)
        {
            case GoblinState.Idle:
                DetectingEnemy();
                break;
            case GoblinState.Agro:
                DetectingEnemy();
                Chase();
                break;
            case GoblinState.Attack:
                AttackDetection();
                PrepareAttack();
                break;
        }
    }
    private void Chase()
    {
        if (currentPlayer == null && goblinState != GoblinState.Agro) return;
        //Debug.Log("Is On");
        Vector3 playerPosition = currentPlayer.transform.position;
        Vector3 direction = (Vector2)(playerPosition - transform.position).normalized;
        direction.y = 0;
        FlippingSprite(direction);
        goblin.SetMovingAnimator(direction.x);
        transform.position += direction * movementSpeed * Time.deltaTime;
        AttackDetection();
    }
    private void FlippingSprite(Vector3 input)
    {
        //Using Euler to rotate on the Y axis to flip it more well, which affect the child position as well
        //Check if the direction is either below or above 0 to rotate it.
        if (input.x == 0) return;
        float y_axisRotation = input.x > 0 ? 0 : 180;
        transform.rotation = Quaternion.Euler(0, y_axisRotation, 0);
    }
    private bool DetectingEnemy()
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(transform.position, AggroAreaDetection, playerMask);
        foreach(Collider2D currentCollider in collider)
        {
            //Debug.Log(currentCollider.ToString());
            Player player = currentCollider.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                //Debug.Log("Player detected");
                goblinState = GoblinState.Agro;
                currentPlayer = player;
                goblin.TriggerAnimator(Enemy.EnemyTrigger.Chasing, true);
                return true;
            }
        }
        goblin.SetMovingAnimator(0);
        currentPlayer = null;
        goblinState = GoblinState.Idle;
        
        return false;
    }
    private bool AttackDetection()
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(transform.position, AttackAreaDetection,0, playerMask);
        foreach (Collider2D currentCollider in collider)
        {
            Player player = currentCollider.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                //Debug.Log("In Attack Range");
                if(goblinState == GoblinState.Agro) ResetAttackTimer();
                goblinState = GoblinState.Attack;
                goblin.SetMovingAnimator(0);
                goblin.TriggerAnimator(Enemy.EnemyTrigger.Chasing, false);
                return true;
            }
        }
        goblinState = GoblinState.Agro;
        goblin.TriggerAnimator(Enemy.EnemyTrigger.Chasing, true);
        return false;
    }
    private void PrepareAttack()
    {
        if (currentlyAttacking) return;
        if (!currentlyPreparingAttack)
        {
            Debug.Log("Preparing to Attack");
            goblin.TriggerAnimator(Enemy.EnemyTrigger.Preparing);
            currentlyPreparingAttack = true;
            currentlyAttacking = false;
            return;
        }
        currentAttackSpeed -= Time.deltaTime;
        if (currentAttackSpeed <= 0)
        {
            currentlyAttacking = true;
            goblin.TriggerAnimator(Enemy.EnemyTrigger.Attack);
        }
    }
    private void Attack()
    {
        Debug.Log("Attacking");
        Collider2D[] hitData = Physics2D.OverlapBoxAll(transform.position, AttackRange, 0, playerMask);
        foreach (Collider2D currentCollider in hitData)
        {
            Debug.Log(currentCollider.gameObject.ToString());
            Player player = currentCollider.gameObject.GetComponentInParent<Player>();
            player.TakeDamage(attackDamage);
        }
    }

    public void ResetAttackTimer()
    {
        Debug.Log("Reset Timer");
        currentAttackSpeed = attackSpeedInterval;
        currentlyPreparingAttack = false;
        currentlyAttacking = false;
    }
    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawCube(transform.position, AggroAreaDetection);
        Gizmos.DrawCube(transform.position, AttackAreaDetection);
    }

}
