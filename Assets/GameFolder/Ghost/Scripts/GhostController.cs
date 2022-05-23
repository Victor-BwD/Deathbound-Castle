using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public Transform a_point, b_point;
    public Transform skin;
    float speedPatrol = 0.6f;

    bool goRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
            collision.GetComponent<Caracters>().life--;
        }
    }
}
