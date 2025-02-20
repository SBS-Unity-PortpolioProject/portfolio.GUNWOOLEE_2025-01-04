using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    private GameObject player;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (player.transform.position.y < -7)
        {
            OnSpawn();
        }
    }

    public void OnSpawn()
    {
        player.transform.position = SavePoint.lastSavePoint;
    }
}
