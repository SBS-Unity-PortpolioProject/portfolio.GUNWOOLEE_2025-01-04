using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private GameObject savePointTrue;
    public static Vector3 lastSavePoint = new Vector3(-66.91f, -1.29f); // 466.16f, 0.67f

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            savePointTrue.SetActive(true);
            lastSavePoint = collision.transform.position; // 좌표 저장
            StartCoroutine(WaitSeconds());
        }
    }

    IEnumerator WaitSeconds()
    {
        float waitTime = 0.1f;
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }
}
