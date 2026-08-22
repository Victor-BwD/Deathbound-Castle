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

        public Transform Skin { get => skin; protected set => skin = value; }
        
        public HealthComponent Health { get; private set; }
        public AnimationComponent Animation { get; private set; }

        protected virtual void Awake()
        {
            // Cache de componentes
            Health = GetComponent<HealthComponent>();
            if (Health == null)
            {
                Debug.LogError($"{gameObject.name}: HealthComponent não encontrado!");
                enabled = false;
                return;
            }

            // AnimationComponent é opcional: nem todo inimigo roteia animação por ele.
            Animation = skin != null ? skin.GetComponent<AnimationComponent>() : null;

            // Setup de eventos
            Health.OnDeath.AddListener(OnDeath);
            Health.OnDamageReceived.AddListener(OnDamageReceived);
        }

        /// <summary>
        /// Chamado quando HealthComponent dispara OnDeath
        /// Subclasses podem override
        /// </summary>
        protected virtual void OnDeath()
        {
            if (Animation != null)
            {
                Animation.PlayAnimation("Die", -1);
            }
        }

        /// <summary>
        /// Chamado quando recebe dano
        /// </summary>
        protected virtual void OnDamageReceived(int damageAmount)
        {
            if (Animation != null)
            {
                Animation.PlayAnimation("PlayerTakeDamage", 1);
            }
        }
    }
}