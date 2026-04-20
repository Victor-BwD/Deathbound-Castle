using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters
{
    /// <summary>
    /// Componente reutilizável para gerenciar saúde de qualquer entidade
    /// </summary>
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 3;
        private int currentHealth;
        private bool isDead;

        // Events para desacoplamento
        [HideInInspector]
        public UnityEvent<int> OnHealthChanged = new();  // Envia novo health
        [HideInInspector]
        public UnityEvent OnDeath = new();
        [HideInInspector]
        public UnityEvent<int> OnDamageReceived = new();  // Envia dano

        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public float HealthPercent => (float)currentHealth / maxHealth;
        public bool IsDead => isDead;

        private void Start()
        {
            currentHealth = maxHealth;
            isDead = false;
        }
        
        public void TakeDamage(int damage)
        {
            if (isDead || damage <= 0) return;

            currentHealth -= damage;
            OnDamageReceived?.Invoke(damage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                OnDeath?.Invoke();
            }
            else
            {
                OnHealthChanged?.Invoke(currentHealth);
            }
        }
        
        public void Heal(int amount)
        {
            if (isDead || amount <= 0) return;

            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }
        
        public void Reset()
        {
            currentHealth = maxHealth;
            isDead = false;
        }

        // Debug
        public void SetHealth(int amount)
        {
            #if UNITY_EDITOR
            currentHealth = Mathf.Clamp(amount, 0, maxHealth);
            isDead = currentHealth <= 0;
            OnHealthChanged?.Invoke(currentHealth);
            #endif
        }
    }
}