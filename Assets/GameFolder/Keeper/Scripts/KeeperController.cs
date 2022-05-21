using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeperController : MonoBehaviour
{
    
    public Transform a_point, b_point;
    public bool goRight;
    float speedPatrol = 0.3f;
    public Transform skin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (goRight)
        {
            skin.localScale = new Vector3(1, 1, 1);

            if(Vector2.Distance(transform.position, b_point.position) < 0.1f)
            {
                goRight = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, b_point.position, speedPatrol * Time.deltaTime);
        }
        else
        {
            skin.localScale = new Vector3(-1, 1, 1);

            if (Vector2.Distance(transform.position, a_point.position) < 0.1f)
            {
                goRight = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, a_point.position, speedPatrol * Time.deltaTime);
        }

        
    }
}
