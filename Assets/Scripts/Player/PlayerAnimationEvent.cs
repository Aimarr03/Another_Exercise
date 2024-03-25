using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private PlayerCombat playerCombat;
    void Start()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }

    public void PlayerDoDamage(int index)
    {
        playerCombat.Damage(index);
    }
    
}
