using Core.Services;
using UnityEngine;

namespace Core.Characters
{
    public class EnemyCharacter : Characters
    {
        [SerializeField] private int soulValue = 1;

        protected override void OnDeath()
        {
            base.OnDeath();  // Chama animação "Die"

            var soulManager = SoulManager.Instance;
            if (soulManager != null)
            {
                soulManager.AddSouls(soulValue);
            }

            // Schedule destruction AGORA
            Destroy(gameObject, 2f);
        }
    }
}