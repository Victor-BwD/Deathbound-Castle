using System;
using Core.Characters;
using GameFolder.Scripts;
using UnityEngine;

namespace Player {
    public class AttackCollider : MonoBehaviour {
        private PlayerCombo _playerCombo;
        private static int currentDamage = 1;
        private int damage = 1;

        private void Start()
        {
            _playerCombo = GetComponentInParent<PlayerCombo>();
            damage = currentDamage;
        }
        
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Enemy")) {
                HealthComponent healthComponent = collision.GetComponent<HealthComponent>();
                
                if (healthComponent != null)
                {
                    int damageAmount = damage;
                    
                    if(_playerCombo.ComboNumber == 1) {
                        damageAmount = damage;
                    }
                    if(_playerCombo.ComboNumber == 2) {
                        damageAmount = damage + 1;
                    }

                    // ✅ NOVO: Chamar o método correto de dano
                    healthComponent.TakeDamage(damageAmount);

                    IAttackable attackableEnemy = collision.GetComponent<IAttackable>();
                    if (attackableEnemy != null)
                    {
                        attackableEnemy.OnPlayerAttack(transform.position);
                    }
                }
            }
        }

        public void UpgradeWeapon(int additionalDamage)
        {
            damage += additionalDamage;
            currentDamage = damage;
        }
    }
}

