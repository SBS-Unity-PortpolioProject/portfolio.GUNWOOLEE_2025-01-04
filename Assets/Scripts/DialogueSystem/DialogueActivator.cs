using System;
using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject Timeline;
    [SerializeField] private Vector2 PlayerPosition;
    public DialogueUI DialogueUI => dialogueUI;

    private bool _nextDialogue = false;
    private bool isInPlayer = false;
    public bool _timeline = false;
    public bool _fadeIn = false;
    public bool _fadeOut = false;
    public bool _move = false;
    public bool _other = false;
    public bool _minusMoiving = false;


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

        if (dialogueUI._check && _move)
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
    }
}