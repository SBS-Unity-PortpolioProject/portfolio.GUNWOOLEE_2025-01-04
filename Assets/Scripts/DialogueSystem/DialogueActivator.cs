using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject Timeline;
    
    public DialogueUI DialogueUI => dialogueUI;

    private bool isInPlayer = false;
    public bool _timeline = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        isInPlayer = other.CompareTag("Player");
        if (isInPlayer)
        {
            DialogueUI.ShowDialogue(dialogueObject);
        }
    }

    private void Update()
    {
        if (dialogueUI.IsOpen) return;
        
        if (dialogueUI._check && Timeline && isInPlayer)
        {
            _timeline = true;
            Timeline.SetActive(true);
        }
        
        if (dialogueUI._check && isInPlayer)
        {
            isInPlayer = false;
            Destroy(gameObject);
        }
    }
}
