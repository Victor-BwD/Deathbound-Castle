using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private string[] dialogueLines;

    private float textSpeed = 0.1f;
    private int index;
    private bool isPlayerInRange = false;

    [SerializeField]
    private TMP_Text dialogueText;
    [SerializeField]
    private GameObject dialogueBox;

    [SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button noButton;

    public UnityEvent onDialogueCompleteYes;

    public UnityEvent onDialogueCompleteNo;

    void Start()
    {
        dialogueText.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueText.text == dialogueLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
    }

    public void ShowDialogue()
    {
        if (!dialogueBox.activeInHierarchy)
        {
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }

    public void SetPlayerInRange(bool inRange)
    {
        isPlayerInRange = inRange;
        if (inRange)
        {
            ShowDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetPlayerInRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetPlayerInRange(false);
        }
    }

    IEnumerator TypeLine()
    {
        foreach (var c in dialogueLines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void ActivePopUp()
    {
        dialogueBox.SetActive(true);
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        
        yesButton.onClick.AddListener(() => {
            onDialogueCompleteYes?.Invoke();
            CloseDialogueBox();
        });
        
        noButton.onClick.AddListener(() => {
            onDialogueCompleteNo?.Invoke();
            CloseDialogueBox();
        });
    }

    void NextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueText.text = string.Empty;
            index = 0;
            ActivePopUp();
        }
    }

    public void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
    }
}
