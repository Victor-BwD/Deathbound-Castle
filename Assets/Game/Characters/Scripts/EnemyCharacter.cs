using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Characters
{
    [SerializeField]
    private int soulValue;
    private bool isDead = false;

    protected override void OnDeath()
    {
        if (isDead) return; 
        isDead = true;

        base.OnDeath();

     
        if (SoulManager.Instance != null)
        {
            SoulManager.Instance.AddSouls(soulValue);
        }
      
        Destroy(gameObject, 2f);
    }
}
