using Core.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFolder.Scripts
{
    public class ChangeScene : MonoBehaviour
    {
        [Tooltip("Se definido, a troca de fase só acontece depois que esse HealthComponent morrer.")]
        [SerializeField] private HealthComponent requiredBossHealth;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player"))
            {
                return;
            }

            if (requiredBossHealth != null && !requiredBossHealth.IsDead)
            {
                return;
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}