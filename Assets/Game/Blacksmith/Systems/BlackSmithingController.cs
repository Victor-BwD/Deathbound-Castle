using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmithingController : MonoBehaviour
{
    private bool playerInRange = false;

    [SerializeField]
    private DialogueManager dialogueManager;

    [SerializeField]
    private Image powerUpActiveObject;

    private AttackCollider attackCollider;
    private SoulManager soulManager;

    private readonly int upgradeCost = 1000;

    private void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = GetComponent<DialogueManager>();
        }

        attackCollider = FindObjectOfType<AttackCollider>();
        soulManager = FindObjectOfType<SoulManager>();

        if (powerUpActiveObject == null)
        {
            powerUpActiveObject = GameObject.Find("PowerUpActive")?.GetComponent<Image>();
        }

        if (dialogueManager != null)
        {
            dialogueManager.onDialogueCompleteYes.AddListener(UpgradeWeapon);
            dialogueManager.onDialogueCompleteNo.AddListener(() => Debug.Log("Upgrade recusado"));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (powerUpActiveObject != null && powerUpActiveObject.enabled)
        {
            return;
        }

        playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void UpgradeWeapon()
    {
        if (soulManager == null || attackCollider == null || powerUpActiveObject == null)
        {
            return;
        }

        if (soulManager.GetSoulCount() < upgradeCost)
        {
            return;
        }

        soulManager.SpendSouls(upgradeCost);

        attackCollider.UpgradeWeapon(1);

        powerUpActiveObject.enabled = true;
    }
}
