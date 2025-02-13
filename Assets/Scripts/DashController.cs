using System;
using UnityEngine;

public class DashController : MonoBehaviour
{
    private Vector2 fixedPosition;
    private bool isLocked = false;

    private Vector2 SpwanPosition = new Vector2(0, -0.175f);

    void OnEnable()
    {
        fixedPosition = transform.position;
        isLocked = true;
    }

    void Update()
    {
        if (isLocked)
        {
            transform.position = new Vector3(fixedPosition.x, fixedPosition.y, transform.position.z);
        }
    }

    void OnDisable()
    {
        isLocked = false;
    }

    public void DisableGameObject()
    {
        transform.localPosition = SpwanPosition;
        gameObject.SetActive(false);
    }

}