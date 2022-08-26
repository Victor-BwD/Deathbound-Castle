using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoorController : MonoBehaviour
{
    private Characters characters;
    private int lifeChange;

    [SerializeField] private Transform lifebar;

    // Start is called before the first frame update
    void Start()
    {
        characters = GetComponent<Characters>();
        lifeChange = GetComponent<Characters>().life;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeChange != GetComponent<Characters>().life)
        {
            lifeChange = GetComponent<Characters>().life;
            characters.skin.GetComponent<Animator>().Play("DoorEnemy", -1);
        }

        if (characters.life <= 0)
        {
            Destroy(transform.gameObject);
        }

        lifebar.localScale = new Vector3((1 * characters.life) / 10f, 1, 1);
    }
}
