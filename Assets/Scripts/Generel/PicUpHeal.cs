using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicUpHeal : MonoBehaviour
{
    public int HealthRestore = 20;
    public Vector3 SpinRotation = new Vector3(0, 180, 0);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable)
        {
            if (damageable.GetHeal(HealthRestore))
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Update()
    {
        transform.eulerAngles += SpinRotation * Time.deltaTime;
    }
    
}
