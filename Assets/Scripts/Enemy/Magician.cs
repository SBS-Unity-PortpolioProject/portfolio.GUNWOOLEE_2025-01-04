using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Magician : MonoBehaviour
{
        [SerializeField] private float _speed = 10f;
        [SerializeField] private DetectionZone _playerDetectionZone;
        [SerializeField] private DetectionZone _attackDetectionZone;
        [SerializeField] private GameObject _target;
        
        private Transform player;
        private SpriteRenderer spriteRenderer;

        public bool _hasTarget = false;
        
        public bool HasTarget
        {
                get { return _hasTarget; }
                private set { _hasTarget = value; }
        }

        private bool _canAttack = false;

        public bool CanAttack
        {
                get { return _canAttack; }
                private set
                {
                        _canAttack = value;
                        _animator.SetBool(AnimationStrings.CanAttack, value);
                }
        }
        
        public bool IsAlive => _animator.GetBool(AnimationStrings.IsAlive);

        private Rigidbody2D _rb;
        private Animator _animator;
        private Damageable _damageable;

        private void Awake()
        {
                _rb = GetComponent<Rigidbody2D>();
                _animator = GetComponent<Animator>();
                _damageable = GetComponent<Damageable>();
                spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
                if (!IsAlive) return;
        
                HasTarget = _playerDetectionZone.DetectionColliders.Count > 0;
                CanAttack = _attackDetectionZone.DetectionColliders.Count > 0;
        }
        void OnTriggerStay2D(Collider2D other)
        {
                if (other.CompareTag("Player"))
                {
                        player = other.transform;

                        if (player.position.x < transform.position.x)
                        {
                                spriteRenderer.flipX = false;
                        }
                        else
                        {
                                spriteRenderer.flipX = true;
                        }
                }
        }

        void OnTriggerExit2D(Collider2D other)
        {
                if (other.CompareTag("Player"))
                {
                        player = null;
                }
        }
}
