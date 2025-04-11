using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject _ClearSceneUI;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _ClearSceneUI.SetActive(true);
        }
    }
}
