using Core.Characters;
using GameFolder.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private Text heartCountText;
        private HealthComponent healthComponent;
        private AudioPlayer audioPlayer;

        private void Start()
        {
            healthComponent = GetComponent<HealthComponent>();
            audioPlayer = GetComponent<AudioPlayer>();

            if (healthComponent == null)
            {
                Debug.LogError("PlayerHealthUI: HealthComponent não encontrado!");
                return;
            }
            
            healthComponent.OnDamageReceived.AddListener(OnPlayerDamaged);
            healthComponent.OnHealthChanged.AddListener(OnHealthChanged);
            healthComponent.OnDeath.AddListener(OnPlayerDeath);
            
            OnHealthChanged(healthComponent.CurrentHealth);
        }
        
        private void OnHealthChanged(int newHealth)
        {
            if (heartCountText != null)
            {
                heartCountText.text = "x" + newHealth;
            }
        }
        
        private void OnPlayerDamaged(int damageAmount)
        {
            var animationComponent = GetComponentInChildren<AnimationComponent>();
            if (animationComponent != null)
            {
                animationComponent.PlayAnimation("PlayerTakeDamage", 1);
            }
            
            if (audioPlayer != null && SoundManager.Instance != null)
            {
                SoundManager.Instance.Play(audioPlayer.damageSound);
            }
        }

        private void OnPlayerDeath()
        {
            var animationComponent = GetComponentInChildren<AnimationComponent>();
            if (animationComponent != null) animationComponent.PlayAnimation("Die", 1);
        }

        private void OnDestroy()
        {
            // Limpar listeners para evitar memory leak
            if (healthComponent != null)
            {
                healthComponent.OnDamageReceived.RemoveListener(OnPlayerDamaged);
                healthComponent.OnHealthChanged.RemoveListener(OnHealthChanged);
            }
        }
    }
}

