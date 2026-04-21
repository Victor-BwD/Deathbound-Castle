using Core.Characters;
using Core.Combat;
using Player;
using System.Collections;
using UnityEngine;

namespace Ghost
{
    public class GhostController : MonoBehaviour
    {
        [SerializeField] private Transform a_point, b_point;
        [SerializeField]private float speedPatrol = 11f;
        [SerializeField]private Transform skin;

        private SpriteRenderer ghostRenderer;
        private CircleCollider2D ghostCollider;
        private HealthComponent healthComponent;
        private EnemyAttackComponent attackComponent;

        private bool goRight;

        private void Start()
        {
            ghostRenderer = GetComponentInChildren<SpriteRenderer>();
            ghostCollider = GetComponent<CircleCollider2D>();
            healthComponent = GetComponent<HealthComponent>();
            attackComponent = GetComponent<EnemyAttackComponent>();
            
            attackComponent.SetAttackStrategy(new MeleeAttackStrategy());
            
            if (healthComponent != null)
            {
                healthComponent.OnDeath.AddListener(HandleDeath);
            }
        }

        void Update()
        {
            if (healthComponent != null && healthComponent.IsDead)
            {
                return;
            }

            if (goRight)
            {
                skin.localScale = new Vector3(-1, 1, 1);

                if (Vector2.Distance(transform.position, b_point.position) < 0.1f)
                {
                    StartCoroutine(WaitAndReturn(a_point.position));
                    goRight = false;
                }

                transform.position = Vector3.MoveTowards(transform.position, b_point.position, speedPatrol * Time.deltaTime);
            }
            else
            {
                skin.localScale = new Vector3(1, 1, 1);

                if (Vector2.Distance(transform.position, a_point.position) < 0.1f)
                {
                    StartCoroutine(WaitAndReturn(b_point.position));
                    StartCoroutine(WaitAndDisappear());
                }

                transform.position = Vector3.MoveTowards(transform.position, a_point.position, speedPatrol * Time.deltaTime);
            }
        }



        private void HandleDeath()
        {
            ghostRenderer.enabled = false;
            ghostCollider.enabled = false;
            this.enabled = false;
            Destroy(gameObject, 2f);
        }

        IEnumerator WaitAndReturn(Vector3 point)
        {
            yield return new WaitForSeconds(1f); 

            transform.position = point; 
        }

        IEnumerator WaitAndDisappear()
        {
            yield return new WaitForSeconds(1f);

            ghostRenderer.enabled = false;
            ghostCollider.enabled = false;

            yield return new WaitForSeconds(1f);

            ghostCollider.enabled = true;
            ghostRenderer.enabled = true;
        }
    }
}

