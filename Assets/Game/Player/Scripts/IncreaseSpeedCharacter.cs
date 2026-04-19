using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseSpeedCharacter : MonoBehaviour
{
    private Image speedUpImage;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerMovement>().IncreaseSpeed();
            speedUpImage = GameObject.Find("SpeedUpActive").GetComponent<Image>();
            speedUpImage.enabled = true;
            

            Destroy(this.gameObject);
        }
    }
}
