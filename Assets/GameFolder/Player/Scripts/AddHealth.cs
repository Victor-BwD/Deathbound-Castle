using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealth : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.GetComponent<Characters>().life <= 5)
            {
                col.GetComponent<Characters>().life++;
                Destroy(this.gameObject);
            }
        }
    }
}
