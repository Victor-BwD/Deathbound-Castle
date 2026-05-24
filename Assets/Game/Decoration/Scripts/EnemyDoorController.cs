using Core.Characters;
using UnityEngine;

public class EnemyDoorController : MonoBehaviour
{
    [SerializeField] private Transform lifebar;
    
    private Characters characters;
    private HealthComponent healthComponent;
    private int previousLife;
    
    private void Start()
    {
        characters = GetComponent<Characters>();
        healthComponent = characters != null ? characters.Health : GetComponent<HealthComponent>();
        if (healthComponent == null)
        {
            enabled = false;
            return;
        }

        previousLife = healthComponent.CurrentHealth;
        healthComponent.OnHealthChanged.AddListener(OnHealthChanged);
        healthComponent.OnDeath.AddListener(OnDeath);
        UpdateLifebar();
    }

    private void OnDestroy()
    {
        if (healthComponent != null)
        {
            healthComponent.OnHealthChanged.RemoveListener(OnHealthChanged);
            healthComponent.OnDeath.RemoveListener(OnDeath);
        }
    }

    private void OnHealthChanged(int currentHealth)
    {
        if (currentHealth != previousLife)
        {
            previousLife = currentHealth;
            if (characters != null && characters.Skin != null)
            {
                characters.Skin.GetComponent<Animator>().Play("DoorEnemy", -1);
            }
        }

        UpdateLifebar();
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

    private void UpdateLifebar()
    {
        if (lifebar == null || healthComponent == null)
        {
            return;
        }

        float maxHealth = healthComponent.MaxHealth > 0 ? healthComponent.MaxHealth : 1f;
        lifebar.localScale = new Vector3(healthComponent.CurrentHealth / maxHealth, 1f, 1f);
    }
}
