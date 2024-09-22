using System;
using UnityEngine;

namespace Player {
    public class AttackCollider : MonoBehaviour {
        private PlayerCombo _playerCombo;

        private int damage = 1;

        private void Start()
        {
            _playerCombo = GetComponentInParent<PlayerCombo>();
        }
        
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Enemy")) {
                if(_playerCombo.ComboNumber == 1) {
                    collision.GetComponent<Characters>().life -= damage;
                }
                if(_playerCombo.ComboNumber == 2) {
                    collision.GetComponent<Characters>().life -= damage + 1;
                }
            }
        }

        public void UpgradeWeapon(int additionalDamage)
        {
            damage += additionalDamage;
        }
    }
}

