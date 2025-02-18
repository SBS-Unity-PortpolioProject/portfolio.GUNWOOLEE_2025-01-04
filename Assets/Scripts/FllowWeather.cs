using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FllowWeather : MonoBehaviour
{
    [SerializeField] private GameObject _fllowObject;

    [SerializeField] private int _y;
    
    private void Update()
    {
        transform.position = new Vector2(_fllowObject.transform.position.x, _fllowObject.transform.position.y + _y);
    }
}
