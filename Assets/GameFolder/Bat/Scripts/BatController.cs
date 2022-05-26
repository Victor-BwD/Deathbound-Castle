using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float attackTime;

    // Start is called before the first frame update
    void Start()
    {
        attackTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) > 0.2f)
        {
            attackTime = 0;
            transform.position = Vector2.MoveTowards(transform.position, player.position, 0.7f * Time.deltaTime);
        }
        else
        {
            attackTime += Time.deltaTime;
            if(attackTime >= 1)
            {
                attackTime = 0;
                player.GetComponent<Caracters>().life--;
            }
        }
        
    }
}
