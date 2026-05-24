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
        
        [HideInInspector]
        public UnityEvent<int> OnHealthChanged = new();
        [HideInInspector]
        public UnityEvent OnDeath = new();
        [HideInInspector]
        public UnityEvent<int> OnDamageReceived = new();

        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public float HealthPercent => maxHealth > 0 ? (float)currentHealth / maxHealth : 0f;
        public bool IsDead => isDead;

        private void OnValidate()
        {
            if (maxHealth < 1)
            {
                maxHealth = 1;
            }
        }

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
            
            OnHealthChanged?.Invoke(currentHealth);

            if (isDead)
            {
                OnDeath?.Invoke();
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