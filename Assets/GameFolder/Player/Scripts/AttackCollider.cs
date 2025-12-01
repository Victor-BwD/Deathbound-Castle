using System;
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
                Characters targetCharacters = collision.GetComponent<Characters>();
                
                if (targetCharacters != null)
                {
                    if(_playerCombo.ComboNumber == 1) {
                        targetCharacters.life -= damage;
                    }
                    if(_playerCombo.ComboNumber == 2) {
                        targetCharacters.life -= damage + 1;
                    }

                    // Notify Keeper if it's a Keeper enemy
                    Keeper.KeeperController keeper = collision.GetComponent<Keeper.KeeperController>();
                    if (keeper != null)
                    {
                        keeper.OnPlayerAttack();
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

