using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent OnNoCollidersRemainEvent = new UnityEvent();
    
    public List<Collider2D> DetectionColliders = new List<Collider2D>();
    
    private Collider2D collider;

    private void awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DetectionColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DetectionColliders.Remove(collision);

        if (DetectionColliders.Count <= 0)
        {
            OnNoCollidersRemainEvent.Invoke();
        }
    }

}
