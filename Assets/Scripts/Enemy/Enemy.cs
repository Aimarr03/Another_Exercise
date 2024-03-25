using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected Player player;
    public enum EnemyTrigger
    {
        Hurt,
        Dead,
        Preparing,
        Attack,
        Idle,
        Move,
        Recharging,
        Chasing,
        Dodging,
        Blocking,
        Diving
    }
    [Serializable]
    public struct EnemyTriggerData
    {
        public EnemyTrigger trigger;
        public string AnimatorConstraint;
    }
    public List<EnemyTriggerData> enemyAnimationTrigger;
    [SerializeField] protected int health;
    [SerializeField] protected LayerMask deadLayer;

    protected Rigidbody2D enemyRigidBody;
    protected BoxCollider2D boxCollider;
    protected HealthSystem healthSystem;

    protected bool isDead;
    protected virtual void Awake()
    {
        healthSystem = new HealthSystem(health);
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
        isDead = false;
    }

    public virtual void TakeDamage(int damage, Player player)
    {
        isDead = healthSystem.TakeDamage(damage);
        if(isDead)
        {
            string animatorConstraint = GetAnimationConstraint(EnemyTrigger.Dead);
            animator.SetBool(animatorConstraint, true);
            //Debug.Log("Enemy Die");

            enabled = false;
        }
        else
        {
            //Debug.Log("Current Health " + healthSystem.CurrentHealth());
            string animatorConstraint = GetAnimationConstraint(EnemyTrigger.Hurt);
            animator.SetTrigger(animatorConstraint);
        }   
    }
    public void TriggerAnimator(EnemyTrigger enemyTrigger) => animator.SetTrigger(GetAnimationConstraint(enemyTrigger));
    public void TriggerAnimator(EnemyTrigger enemyTrigger, bool input) => animator.SetBool(GetAnimationConstraint(enemyTrigger), input);
    public void SetMovingAnimator(float input)
    {
        float moving = input != 0 ? 1 : -1;
        string animatorConstraint = GetAnimationConstraint(EnemyTrigger.Move);
        animator.SetFloat(animatorConstraint, moving);
    }

    public string GetAnimationConstraint(EnemyTrigger trigger)
    {
        foreach(EnemyTriggerData data in enemyAnimationTrigger)
        {
            if (data.trigger == trigger) return data.AnimatorConstraint;
        }
        return "";
    }
    public bool IsDead() => isDead;
    public Rigidbody2D GetRigidBody() => enemyRigidBody;
}
