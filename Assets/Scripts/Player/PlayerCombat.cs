using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private LayerMask enemyIndex;
    [SerializeField] private int indexAttack = 0;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private List<AttackData> attackData;
    [Serializable]
    public struct AttackData
    {
        public Vector2 attackRange;
        public float attackAngle;
        public int damage;
    }
    
    public void Damage(int index)
    {
        for(int rotation = 0; rotation < attackData.Count; rotation++)
        {
            if (rotation == index) break;
        }
        Debug.Log("Attack type " + index);
        AttackData currentAttackData = attackData[index];
        Collider2D[] hitData = Physics2D.OverlapBoxAll(attackPoint.position, currentAttackData.attackRange, currentAttackData.attackAngle, enemyIndex);
        foreach(Collider2D hit in hitData)
        {
            Enemy enemy = hit.gameObject.GetComponentInParent<Enemy>();
            if (enemy.IsDead()) continue;
            Player player = GetComponent<Player>();
            enemy.TakeDamage(currentAttackData.damage, player);
        }
        
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null ) return;
        if (indexAttack > attackData.Count || indexAttack < 0) return;
        Gizmos.DrawWireCube(attackPoint.position, attackData[indexAttack].attackRange);
    }
}
