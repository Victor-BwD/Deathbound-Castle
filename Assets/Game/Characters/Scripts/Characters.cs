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

        // COMPONENTES - cada um com responsabilidade clara
        private HealthComponent healthComponent;
        private AnimationComponent animationComponent;

        // LEGACY: Para não quebrar scripts antigos (REMOVER DEPOIS)
        [HideInInspector] public int life;  // Obsoleto, usar Health.CurrentHealth
        public Transform Skin => skin;

        // NOVO: Interface melhor
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

        private void Update()
        {
            // UPDATE SIMPLIFICADO - apenas gerenciar ciclo
            // Lógica de morte foi para HealthComponent -> OnDeath event
            
            // LEGACY SUPPORT: Manter life sincronizado (REMOVER DEPOIS)
            life = healthComponent.CurrentHealth;
        }

        /// <summary>
        /// LEGACY: Mantém compatibilidade com código antigo (DEPRECADO)
        /// Remova isto depois de migrar todos os scripts
        /// </summary>
        public void PlayerTakaDamage(int damage)
        {
            Debug.LogWarning("PlayerTakaDamage() está DEPRECADO! Use Health.TakeDamage() ao invés");
            healthComponent.TakeDamage(damage);
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