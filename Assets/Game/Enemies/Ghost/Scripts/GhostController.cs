using Core.Characters;
using Core.Combat;
using Player;
using System.Collections;
using UnityEngine;

namespace Ghost
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(EnemyAttackComponent))]
    public class GhostController : EnemyCharacter
    {
        [SerializeField] private Transform a_point, b_point;
        [SerializeField]private float speedPatrol = 11f;

        private SpriteRenderer ghostRenderer;
        private CircleCollider2D ghostCollider;
        private EnemyAttackComponent attackComponent;

        private bool goRight;

        private void Start()
        {
            ghostRenderer = GetComponentInChildren<SpriteRenderer>();
            ghostCollider = GetComponent<CircleCollider2D>();
            attackComponent = GetComponent<EnemyAttackComponent>();
        }

        void Update()
        {
            if (Health.IsDead)
            {
                return;
            }

            if (goRight)
            {
                Skin.localScale = new Vector3(-1, 1, 1);

                if (Vector2.Distance(transform.position, b_point.position) < 0.1f)
                {
                    StartCoroutine(WaitAndReturn(a_point.position));
                    goRight = false;
                }

                transform.position = Vector3.MoveTowards(transform.position, b_point.position, speedPatrol * Time.deltaTime);
            }
            else
            {
                Skin.localScale = new Vector3(1, 1, 1);

                if (Vector2.Distance(transform.position, a_point.position) < 0.1f)
                {
                    StartCoroutine(WaitAndReturn(b_point.position));
                    StartCoroutine(WaitAndDisappear());
                }

                transform.position = Vector3.MoveTowards(transform.position, a_point.position, speedPatrol * Time.deltaTime);
            }
        }

        protected override void OnDeath()
        {
            ghostRenderer.enabled = false;
            ghostCollider.enabled = false;
            this.enabled = false;

            base.OnDeath();
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
            {
                return;
            }

            if (attackComponent != null)
            {
                attackComponent.DoAttack(collision);
                return;
            }

            var targetHealth = collision.GetComponent<HealthComponent>();
            if (targetHealth != null && !targetHealth.IsDead)
            {
                targetHealth.TakeDamage(1);
            }
        }
    }
}
