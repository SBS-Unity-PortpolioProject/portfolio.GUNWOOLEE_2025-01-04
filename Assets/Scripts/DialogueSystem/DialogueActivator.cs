using System;
using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
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
    public bool _fadeIn = false;
    public bool _fadeOut = false;
    public bool _firstMove = false;
    public bool _afterMove = false;
    public bool _other = false;
    public bool _minusMoiving = false;
    public bool _change = false;
    public bool _change2 = false;
    public bool _nextStage = false;
    public bool _next = false;
    public bool _summonDialogue = false;

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

        if (dialogueUI.dialogueStarted && _firstMove)
        {
            Debug.Log(123);
            _other = true;
            Player.transform.position = PlayerPosition;
        }

        if (dialogueUI._check && _afterMove)
        {
            _other = true;
            Player.transform.position = PlayerPosition;
        }

        if (dialogueUI._check && _fadeIn)
        {
            _other = true;
        }
        else if (dialogueUI._check && _fadeOut)
        {
            _other = true;
        }

        if (dialogueUI._check && _change)
        {
            _change2 = true;
        }

        if (dialogueUI._check && _nextStage)
        {
            _next = true;
        }
        
        if (_battleDialogue != null)
        {
            if (_battleDialogue._battleClear)
            {
                Timeline.SetActive(true);
            }
        }
    }
}