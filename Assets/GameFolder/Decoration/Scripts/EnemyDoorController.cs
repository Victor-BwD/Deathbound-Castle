using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoorController : MonoBehaviour
{
    [SerializeField] private Transform lifebar;
    
    private Characters characters;
    private int previousLife;
    
    private void Start()
    {
        characters = GetComponent<Characters>();
        previousLife = characters.life;
    }

    private void Update()
    {
        if (previousLife != characters.life)
        {
            previousLife = characters.life;
            characters.skin.GetComponent<Animator>().Play("DoorEnemy", -1);
        }

        if (characters.life <= 0)
        {
            Destroy(gameObject);
        }

        lifebar.localScale = new Vector3((float)characters.life / 10f, 1f, 1f);
    }
}
