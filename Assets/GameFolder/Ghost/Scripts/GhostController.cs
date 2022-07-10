using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private Transform a_point, b_point;
    [SerializeField]private float speedPatrol = 0.6f;
    private Transform skin;
    private bool goRight;

    // Update is called once per frame
    void Update()
    {
        if (goRight)
        {
            skin.localScale = new Vector3(-1, 1, 1);

            if (Vector2.Distance(transform.position, b_point.position) < 0.1f)
            {
                transform.position = a_point.position;
            }

            transform.position = Vector3.MoveTowards(transform.position, b_point.position, speedPatrol * Time.deltaTime);
        }
        else
        {
            skin.localScale = new Vector3(1, 1, 1);

            if (Vector2.Distance(transform.position, a_point.position) < 0.1f)
            {
                transform.position = b_point.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, a_point.position, speedPatrol * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Characters>().life--;
        }
    }
}
