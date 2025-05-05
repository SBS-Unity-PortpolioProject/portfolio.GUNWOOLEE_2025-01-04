using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject clearScene;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private GameObject playerBar;
    [SerializeField] private GameObject playerBar2;
    [SerializeField] public List<DialogueActivator> _dialogueActivatorList;
    public DialogueActivator _currentDialogueActivator;
    public AudioSource _audioSource;
    public bool _dialogueStarted = false;
    public bool _check = false;
    
    public bool IsOpen { get; private set; }
    
    private ResponseHandler responseHandler;
    private TypeWriterEffect typeWriterEffect;
    public int _dialogueCounter = 0;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        
        CloseDialogueBox();
    }
    
    public void ShowDialogue(DialogueObject dialogueObject)
    {
        _audioSource.Play();

        if (_dialogueActivatorList != null && _dialogueActivatorList.Count > 1)
        {
            _currentDialogueActivator = _dialogueActivatorList[_dialogueCounter];
        }
        
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
            StartCoroutine(AddDialogueList());
            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypeWriterEffect(string dialogue)
    {
        if (_currentDialogueActivator._change2)
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

            if (typeWriterEffect.IsRunning)
            {
                _audioSource.Play();
            }
            
            while (typeWriterEffect.IsRunning)
            {
                yield return null;
                
                if (Input.GetMouseButtonDown(0))
                {
                    _audioSource.Stop();
                    typeWriterEffect.Stop();
                }
            }
            _audioSource.Stop();
        }
    }

    public void CloseDialogueBox()
    {
        if (playerBar2 != null && _currentDialogueActivator != null)
        {
            if (_currentDialogueActivator._change2)
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

    private IEnumerator AddDialogueList()
    {
        if (_dialogueActivatorList != null && _dialogueActivatorList.Count > 1)
        {
            _dialogueActivatorList.Remove(_currentDialogueActivator);
            _currentDialogueActivator = _dialogueActivatorList[_dialogueCounter];
            _currentDialogueActivator.gameObject.SetActive(true);
            _check = true;
        }
        else if (_dialogueActivatorList != null && _dialogueActivatorList.Count == 1)
        {
            if (clearScene != null)
            {
                clearScene.SetActive(true);
            }
            _dialogueActivatorList.Remove(_currentDialogueActivator);
            _check = true;
        }
        else
        {
            _check = true;
        }
        yield return null;
    }
}