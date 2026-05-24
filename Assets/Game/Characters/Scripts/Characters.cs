using UnityEngine;

namespace Core.Characters
{
    /// <summary>
    /// Classe base para qualquer entidade com vida (Player, Inimigos)
    /// REFATORADO para usar componentes desacoplados
    /// </summary>
    public class Characters : MonoBehaviour
    {
        [SerializeField] private Transform skin;
        
        private HealthComponent healthComponent;
        private AnimationComponent animationComponent;

        public Transform Skin => skin;
        
        public HealthComponent Health { get; private set; }
        public AnimationComponent Animation { get; private set; }

        protected virtual void Awake()
        {
            // Cache de componentes
            healthComponent = GetComponent<HealthComponent>();
            if (healthComponent == null)
            {
                Debug.LogError($"{gameObject.name}: HealthComponent não encontrado!");
                enabled = false;
                return;
            }

            animationComponent = skin != null ? skin.GetComponent<AnimationComponent>() : null;
            if (animationComponent == null && skin != null)
            {
                Debug.LogWarning($"{gameObject.name}: AnimationComponent não encontrado em {skin.name}");
            }

            // Expor componentes
            Health = healthComponent;
            Animation = animationComponent;

            // Setup de eventos
            healthComponent.OnDeath.AddListener(OnDeath);
            healthComponent.OnDamageReceived.AddListener(OnDamageReceived);
        }

        /// <summary>
        /// Chamado quando HealthComponent dispara OnDeath
        /// Subclasses podem override
        /// </summary>
        protected virtual void OnDeath()
        {
            if (animationComponent != null)
            {
                animationComponent.PlayAnimation("Die", -1);
            }
        }

        /// <summary>
        /// Chamado quando recebe dano
        /// </summary>
        protected virtual void OnDamageReceived(int damageAmount)
        {
            if (animationComponent != null)
            {
                animationComponent.PlayAnimation("PlayerTakeDamage", 1);
            }
        }
    }
}