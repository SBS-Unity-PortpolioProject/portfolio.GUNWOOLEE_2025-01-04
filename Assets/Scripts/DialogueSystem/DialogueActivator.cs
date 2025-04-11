using System;
using System.Collections;
using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueActivator;
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject Timeline;
    [SerializeField] private BattleDialogue _battleDialogue;
    [SerializeField] private Vector2 PlayerPosition;
    public DialogueUI DialogueUI => dialogueUI;
    
    private bool _nextDialogue = false;
    private bool isInPlayer = false;
    public bool _timeline = false;
    public bool _fadeIn, FadeIn = false;
    public bool _fadeOut, FadeOut = false;
    public bool _firstMove = false;
    public bool _afterMove = false;
    public bool _other = false;
    public bool _minusMoiving = false;
    public bool _change = false;
    public bool _change2 = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        isInPlayer = other.CompareTag("Player");
        if (isInPlayer)
        {
            DialogueUI.ShowDialogue(dialogueObject);
        }

        if (isInPlayer && _minusMoiving)
        {
            _playerController._WalkSpeed -= 0.2f;
            _playerController._RunSpeed -= 0.2f;
        }
    }

    private void Update()
    {
        if (dialogueUI.IsOpen) return;
        
        if (dialogueUI._check)
        {
            _other = true;
        }
        
        if (dialogueUI._check && !isInPlayer)
        {
            dialogueUI._check = false;
        }
        
        if (dialogueUI._check && Timeline && isInPlayer)
        {
            _timeline = true;
            Timeline.SetActive(true);
        }
        
        if (dialogueUI._check && isInPlayer)
        {
            StartCoroutine(DestroyDialogue());
        }
        
        if (Player != null)
        {
            if (dialogueUI._dialogueStarted && _firstMove)
            {
                Player.transform.position = PlayerPosition;
                Player.transform.localScale = new Vector3(3, 3, 1);
                _firstMove = false;
            }
        }
        
        if (Player != null)
        {
            if (dialogueUI._check && _afterMove)
            {
                Player.transform.position = PlayerPosition;
                Player.transform.localScale = new Vector3(3, 3, 1);
                _afterMove = false;
            }
        }
        
        if (dialogueUI._check && _fadeIn)
        {
            FadeIn = true;
            _fadeIn = false;
        }
        else if (dialogueUI._check && _fadeOut)
        {
            FadeOut = true;
            _fadeOut = false;
        }

        if (dialogueUI._check && _change)
        {
            _change2 = true;
            _change = false;
        }

        if (_battleDialogue != null)
        {
            if (_battleDialogue._battleClear)
            {
                Timeline.SetActive(true);
            }
        }

        IEnumerator DestroyDialogue()
        {
            yield return new WaitForSeconds(0.25f);
            dialogueUI._check = false;
            Destroy(gameObject);
        }
    }
}