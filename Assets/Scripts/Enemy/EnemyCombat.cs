using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    //For 
    [SerializeField] protected Vector2 AggroAreaDetection;
    [SerializeField] protected Vector2 AttackAreaDetection;
    [SerializeField] protected Vector2 AttackRange;

    [SerializeField] protected LayerMask playerMask;

    [SerializeField] protected float movementSpeed;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float attackSpeedInterval;

    protected float currentAttackSpeed;

    protected int takingHitMax;
    protected bool currentlyPreparingAttack;
    protected bool currentlyAttacking;

    protected Enemy enemy;
    protected Player currentPlayer;
    protected virtual void Awake()
    {
        currentAttackSpeed = attackSpeedInterval;
        currentlyPreparingAttack = false;
        currentlyAttacking = false;
        enemy = GetComponent<Enemy>();
        goblinState = EnemyState.Idle;
    }
    public enum EnemyState
    {
        Idle,
        Agro,
        Attack,
    }
    [SerializeField] protected EnemyState goblinState;

    protected virtual void Update()
    {
        if (enemy.IsDead()) return;
        switch (goblinState)
        {
            case EnemyState.Idle:
                DetectingEnemy();
                break;
            case EnemyState.Agro:
                DetectingEnemy();
                Chase();
                break;
            case EnemyState.Attack:
                AttackDetection();
                PrepareAttack();
                break;
        }
    }
    protected virtual void Chase()
    {
        if (currentPlayer == null && goblinState != EnemyState.Agro) return;
        //Debug.Log("Is On");
        Vector3 playerPosition = currentPlayer.transform.position;
        Vector3 direction = (Vector2)(playerPosition - transform.position).normalized;
        direction.y = 0;
        FlippingSprite(direction);
        enemy.SetMovingAnimator(direction.x);
        transform.position += direction * movementSpeed * Time.deltaTime;
        AttackDetection();
    }
    protected virtual void FlippingSprite(Vector3 input)
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
        foreach (Collider2D currentCollider in collider)
        {
            //Debug.Log(currentCollider.ToString());
            Player player = currentCollider.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                //Debug.Log("Player detected");
                goblinState = EnemyState.Agro;
                currentPlayer = player;
                enemy.TriggerAnimator(Enemy.EnemyTrigger.Chasing, true);
                return true;
            }
        }
        enemy.SetMovingAnimator(0);
        currentPlayer = null;
        goblinState = EnemyState.Idle;

        return false;
    }
    private bool AttackDetection()
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(transform.position, AttackAreaDetection, 0, playerMask);
        foreach (Collider2D currentCollider in collider)
        {
            Player player = currentCollider.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                //Debug.Log("In Attack Range");
                if (goblinState == EnemyState.Agro) ResetAttackTimer();
                goblinState = EnemyState.Attack;
                enemy.SetMovingAnimator(0);
                enemy.TriggerAnimator(Enemy.EnemyTrigger.Chasing, false);
                return true;
            }
        }
        goblinState = EnemyState.Agro;
        enemy.TriggerAnimator(Enemy.EnemyTrigger.Chasing, true);
        return false;
    }
    protected virtual void PrepareAttack()
    {
        if (currentlyAttacking) return;
        if (!currentlyPreparingAttack)
        {
            Debug.Log("Preparing to Attack");
            enemy.TriggerAnimator(Enemy.EnemyTrigger.Preparing);
            currentlyPreparingAttack = true;
            currentlyAttacking = false;
            return;
        }
        currentAttackSpeed -= Time.deltaTime;
        if (currentAttackSpeed <= 0)
        {
            currentlyAttacking = true;
            enemy.TriggerAnimator(Enemy.EnemyTrigger.Attack);
        }
    }
    protected virtual void Attack()
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
    protected virtual void OnDrawGizmosSelected()
    {
        //Gizmos.DrawCube(transform.position, AggroAreaDetection);
        Gizmos.DrawCube(transform.position, AttackAreaDetection);
    }
}
