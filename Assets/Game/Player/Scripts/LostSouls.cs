using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSouls : MonoBehaviour
{
    private int soulAmount;

    public void SetSoulAmount(int amount)
    {
        soulAmount = amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoulManager.Instance.RecoverLostSouls(soulAmount);

            Destroy(gameObject);
        }
    }
}
