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

    private int upgradeCost = 1000;

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
            Debug.Log("Arma já foi melhorada.");
            return;
        }

        playerInRange = true;
        Debug.Log("Pressione E para falar com o ferreiro.");
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
            Debug.LogError("Referências necessárias não encontradas!");
            return;
        }

        if (soulManager.GetSoulCount() < upgradeCost)
        {
            Debug.Log($"Almas insuficientes! Necessário: {upgradeCost}, Atual: {soulManager.GetSoulCount()}");
            return;
        }

        soulManager.SpendSouls(upgradeCost);

        attackCollider.UpgradeWeapon(1);

        powerUpActiveObject.enabled = true;

        Debug.Log("Arma melhorada com sucesso!");
    }
}
