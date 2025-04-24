using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallRespawn : MonoBehaviour
{
    private GameObject _player;
    
    private Damageable _damageable;
    
    private bool _isBattle1 = false;
    
    private CapsuleCollider2D _TouchingCollider;
    
    public bool _battleZoneReset = false; 
    
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _damageable = GetComponent<Damageable>();
    }

    private void Update()
    {
        if (_player.transform.position.y < -7)
        {
            OnSpawn();
        }
        else if (!_damageable.IsAlive)
        {
            OnSpawn();
            _battleZoneReset = true;
            _damageable.IsAlive = true;
            StartCoroutine(ResetBattleZone());
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
        Scene currentScene = SceneManager.GetActiveScene();

        if (_isBattle1)
        {
            _player.transform.position = new Vector2(24.63f, 2.6f);
        }
        else if (currentScene.name == "GameScenes4")
        {
            SceneManager.LoadScene("GameScenes4");
        }
        else
        {
            _player.transform.position = SavePoint.lastSavePoint;
        }
    }

    IEnumerator ResetBattleZone()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        _battleZoneReset = false;
    }
}
