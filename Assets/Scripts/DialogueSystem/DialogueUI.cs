using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private GameObject playerBar;
    [SerializeField] private GameObject playerBar2;
    [SerializeField] private DialogueActivator _dialogueActivator;
    public bool _dialogueStarted = false;
    public bool _check = false;
    
    public bool IsOpen { get; private set; }
    
    private ResponseHandler responseHandler;
    private TypeWriterEffect typeWriterEffect;

    private void Start()
    {
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        
        CloseDialogueBox();
    }
    
    public void ShowDialogue(DialogueObject dialogueObject)
    {
        _dialogueStarted = true;
        StartCoroutine(IsOpening());
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }
    
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];

            yield return RunTypeWriterEffect(dialogue); 
            
            textLabel.text = dialogue;
            
            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.Responses != null && dialogueObject.HasResponses) break;

            yield return null; 
            
            yield return new WaitUntil(()=> Input.GetMouseButtonDown(0));
        }
        
        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            _check = true;
            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypeWriterEffect(string dialogue)
    {
        if (_dialogueActivator._change2)
        {
            playerBar2.SetActive(false);

            typeWriterEffect.Run(dialogue, textLabel);

            while (typeWriterEffect.IsRunning)
            {
                yield return null;

                if (Input.GetMouseButtonDown(0))
                {
                    typeWriterEffect.Stop();
                }
            }
        }
        else
        {
            playerBar.SetActive(false);

            typeWriterEffect.Run(dialogue, textLabel);

            while (typeWriterEffect.IsRunning)
            {
                yield return null;

                if (Input.GetMouseButtonDown(0))
                {
                    typeWriterEffect.Stop();
                }
            }
        }
    }

    public void CloseDialogueBox()
    {
        if (_dialogueActivator._change2)
        {
            _dialogueStarted = false;
            IsOpen = false;
            playerBar2.SetActive(true);
            dialogueBox.SetActive(false);
            textLabel.text = string.Empty;
        }
        else
        {
            _dialogueStarted = false;
            IsOpen = false;
            playerBar.SetActive(true);
            dialogueBox.SetActive(false);
            textLabel.text = string.Empty;
        }
    }

    private IEnumerator IsOpening()
    {
        yield return new WaitForSeconds(0.1f);
        IsOpen = true;
    }
}
