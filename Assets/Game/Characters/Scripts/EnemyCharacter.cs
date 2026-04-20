using UnityEngine;

namespace Core.Characters
{
    /// <summary>
    /// Versão específica para inimigos
    /// Adiciona comportamento de drop de souls
    /// </summary>
    public class EnemyCharacter : Characters
    {
        [SerializeField] private int soulValue = 1;

        protected override void OnDeath()
        {
            base.OnDeath();  // Chama animação "Die"

            // NOVO: Usar ServiceLocator ao invés de FindObjectOfType
            var soulManager = FindObjectOfType<SoulManager>();  // TODO: Usar ServiceLocator depois
            if (soulManager != null)
            {
                soulManager.AddSouls(soulValue);
            }

            // Schedule destruction AGORA
            Destroy(gameObject, 2f);
        }
    }
}