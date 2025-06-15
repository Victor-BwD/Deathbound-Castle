using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSmithingController : MonoBehaviour
{
    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        playerInRange = true;
    }
}
