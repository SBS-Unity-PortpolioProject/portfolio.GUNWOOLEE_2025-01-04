using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private GameObject savePointTrue;
    public static Vector3 lastSavePoint = new Vector3(-3.96f, -1.32f); // 기본값 설정

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
