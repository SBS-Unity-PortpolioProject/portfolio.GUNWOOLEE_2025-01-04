using Unity.VisualScripting;
using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    private GameObject _player;
    
    private Damageable _damageable;
    
    private bool _isBattle1 = false;
    
    CapsuleCollider2D _TouchingCollider;
    
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _damageable = GetComponent<Damageable>();
    }

    private void Update()
    {
        if (_player.transform.position.y < -7 || !_damageable.IsAlive)
        {
            OnSpawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Battle1")
        {
            _isBattle1 = true;
        }
    }

    public void OnSpawn()
    {
        if (_isBattle1)
        {
            _player.transform.position = new Vector2(24.63f, 2.6f);
        }
        else
        {
            _player.transform.position = SavePoint.lastSavePoint;
        }
    }
}
