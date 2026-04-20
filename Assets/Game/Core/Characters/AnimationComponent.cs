using UnityEngine;

namespace Core.Characters
{
    /// <summary>
    /// Centraliza toda a lógica de animação para evitar GetComponent repetido
    /// </summary>
    public class AnimationComponent : MonoBehaviour
    {
        private Animator animator;
        private string currentAnimationName;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError($"AnimationComponent on {gameObject.name}: Animator não encontrado!");
            }
        }

        /// <summary>
        /// Play animation com cache para evitar mudanças desnecessárias
        /// </summary>
        public void PlayAnimation(string animationName, int layer = 0)
        {
            if (string.IsNullOrEmpty(animationName))
            {
                Debug.LogWarning("AnimationComponent: Nome de animação vazio");
                return;
            }

            // Skip se já está tocando
            if (currentAnimationName == animationName)
                return;

            if (animator != null)
            {
                animator.Play(animationName, layer);
                currentAnimationName = animationName;
            }
        }

        /// <summary>
        /// Set bool parameter (para idle, run, etc)
        /// </summary>
        public void SetBool(string paramName, bool value)
        {
            if (animator != null)
            {
                animator.SetBool(paramName, value);
            }
        }

        /// <summary>
        /// Checka se está em estado específico
        /// </summary>
        public bool IsInState(string stateName, int layer = 0)
        {
            if (animator == null) return false;
            return animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName);
        }

        /// <summary>
        /// Gets normalizado da animação (0-1)
        /// </summary>
        public float GetAnimationNormalizedTime(int layer = 0)
        {
            if (animator == null) return 0f;
            return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime;
        }
    }
}