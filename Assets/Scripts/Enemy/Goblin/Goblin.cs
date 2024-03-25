using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    private Vector3 currentPlayerAttackDirection;

    [SerializeField] private float dodgeForce;
    [SerializeField] private float dodgeInterval;
    private float currentDodgeInterval;

    private bool canDodge = true;
    private bool isDodging;
    
    protected override void Awake()
    {
        base.Awake();
        isDodging = false;
    }
    public override void TakeDamage(int damage, Player player)
    {
        this.player = player;
        if (canDodge)
        {
            //Debug.Log("Dodge!");
            canDodge = false;
            Debug.Log(canDodge);
            currentDodgeInterval = 0f;
            GetAttackDirection();
            return;
        }
        if (isDodging) return;
        base.TakeDamage(damage, player);
    }
    private void GetAttackDirection()
    {
        //To get direction player attack
        Vector3 playerPosition = player.transform.position;
        Vector3 AttackDirection = (transform.position - playerPosition).normalized;
        float xAxis = Mathf.Sign(AttackDirection.x);
        
        AttackDirection = new Vector3(xAxis, 0, 0);
        currentPlayerAttackDirection = AttackDirection;
        TriggerAnimator(EnemyTrigger.Dodging);
        isDodging = true;
        //canDodge = false;
    }
    //bool for deciding if it reverse direction or not
    public void AddForceMechanism(int input)
    {
        currentPlayerAttackDirection = input > 0 ? currentPlayerAttackDirection * 0.5f : -currentPlayerAttackDirection;
        enemyRigidBody.velocity = currentPlayerAttackDirection * dodgeForce;
    }
    public void SetIsDodging(int input) => isDodging = input > 0;
    public bool GetIsDodging() => isDodging;

    // Update is called once per frame
    void Update()
    {
        if(canDodge) return;
        currentDodgeInterval += Time.deltaTime;
        if(currentDodgeInterval > dodgeInterval)
        {
            canDodge = true;
            currentDodgeInterval = 0f;
        }
    }
    
}
