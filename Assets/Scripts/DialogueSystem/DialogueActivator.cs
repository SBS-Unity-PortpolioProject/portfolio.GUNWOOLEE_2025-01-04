using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;

    private bool isInPlayer = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        isInPlayer = other.CompareTag("Player");
        if (isInPlayer)
        {
            DialogueUI.ShowDialogue(dialogueObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isInPlayer)
        {
            isInPlayer = false;
            Destroy(gameObject);
        }
    }
    
    private void FixedUpdate()
    {
        if (dialogueUI.IsOpen) return;
    }
}
