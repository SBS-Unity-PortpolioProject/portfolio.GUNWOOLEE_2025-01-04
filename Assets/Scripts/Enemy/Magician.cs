using System.Collections;
using UnityEngine;

public class Magician : MonoBehaviour
{
        [SerializeField] private float _speed = 10f;
        [SerializeField] private DetectionZone _playerDetectionZone;
        [SerializeField] private DetectionZone _attackDetectionZone;
        [SerializeField] private GameObject _target;
        [SerializeField] private float detectionCooldown = 5f;
        private bool AttackCool = true;
        private float _attackCoolDown = 1f;
        private Transform player;
        private bool canDetect = true;


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
                        //if (AttackCool)
                        //{
                        //        AttackCool = false;
                        //        _animator.SetBool(AnimationStrings.CanAttack, value);
                        //        StartCoroutine(EnumerableAttackCool()); 실행 암됌
                        //}
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
        }

        private void Update()
        {
                if (!IsAlive)
                {
                        gameObject.SetActive(false);
                        return;
                }
        
                HasTarget = _playerDetectionZone.DetectionColliders.Count > 0;
                CanAttack = _attackDetectionZone.DetectionColliders.Count > 0;
        }
        void OnTriggerStay2D(Collider2D other)
        {
                if (canDetect && other.CompareTag("Player"))
                {
                        player = other.transform;
                
                        if (player.position.x < transform.position.x)
                        {
                                transform.localScale = new Vector3(-5, 5, 1);
                                canDetect = false;
                                StartCoroutine(EnableDetectionAfterDelay());
                        }
                        else
                        {
                                transform.localScale = new Vector3(5, 5, 1);
                                canDetect = false;
                                StartCoroutine(EnableDetectionAfterDelay());
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
        
        IEnumerator EnableDetectionAfterDelay()
        {
                yield return new WaitForSeconds(detectionCooldown);
                canDetect = true;
        }

        // IEnumerator EnumerableAttackCool()
        // {
        //         yield return new WaitForSeconds(_attackCoolDown);
        //         AttackCool = true;
        // }                                                            실행 안됌
}
