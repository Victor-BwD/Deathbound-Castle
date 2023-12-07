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

        private bool goRight;
        private int damage = 1;

        private void Start()
        {
            ghostRenderer = GetComponentInChildren<SpriteRenderer>();
            ghostCollider = GetComponent<CircleCollider2D>();
        }

        void Update()
        {
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


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerHealth>().PlayerTakaDamage(damage);
            }
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

