using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeperController : MonoBehaviour
{
    
    [SerializeField] private Transform a_point, b_point;
    [SerializeField] private Transform skin;
    private bool goRight;
    private float speedPatrol = 1.1f;
    private Transform keeperRange;
    private Animator receiveSkinAnimator;

    // Start is called before the first frame update
    void Start()
    {
        receiveSkinAnimator = skin.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Caracters>().life <= 0)
        {
            keeperRange.GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            this.enabled = false;
        }

        if (receiveSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("KeeperAttack"))
        {
            return;
        }

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
