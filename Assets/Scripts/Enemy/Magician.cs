using System.Collections;
using UnityEngine;

public class Magician : MonoBehaviour
{
        [SerializeField] private float _speed = 10f;
        [SerializeField] private DetectionZone _playerDetectionZone;
        [SerializeField] private DetectionZone _attackDetectionZone;
        [SerializeField] private GameObject _target;
        // public AudioClip[] _magicianSound;
        private bool AttackCool = true;
        private float _attackCoolDown = 1f;
        private Transform player;
        private bool canDetect = true;
        private bool _attackCheck = false;


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
                  //_magicianSound.PlayOneShot(); 공격 소리
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
        }

        private void Update()
        { 
             if (!IsAlive)
             {
                  Destroy(gameObject);
                  return;
             }
        
             HasTarget = _playerDetectionZone.DetectionColliders.Count > 0;
             
             if (_attackDetectionZone.DetectionColliders.Count > 0 && !_attackCheck)
             {
                  _attackCheck = true;
                  StartCoroutine(Attack());
             }
             else
             {
                  CanAttack = false;
             }
        }
        
        private IEnumerator Attack()
        {
             yield return new WaitForSeconds(1f);
             CanAttack = true;
             yield return new WaitForSeconds(1f);

             if (_rb.transform.localScale.x > 0)
             {
                  // _magicianSound.PlayOneShot(); 순간이동 하는 소리
                  transform.localScale = new Vector3(-5f, 5, 0);
             }
             else if (_rb.transform.localScale.x < 0)
             {
                  // _magicianSound.PlayOneShot(); 순간이동 하는 소리
                  transform.localScale = new Vector3(5f, 5, 0);
             }
             _attackCheck = false;
        }
}












