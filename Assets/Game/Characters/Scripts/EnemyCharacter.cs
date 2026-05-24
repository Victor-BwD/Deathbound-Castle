using UnityEngine;

namespace Core.Characters
{
    public class EnemyCharacter : Characters
    {
        [SerializeField] private int soulValue = 1;

        protected override void OnDeath()
        {
            base.OnDeath();

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