using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (player.transform.position.y < -7)
        {
            player.transform.position = new Vector3(-3.96f, -1.32f, 0f);
        }
    }
}
