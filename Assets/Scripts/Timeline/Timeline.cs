using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{
    [SerializeField] private GameObject _clearSceneUI;
    public PlayableDirector director;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            director.Play();
        }
    }

    public void OnNextStage()
    {
        _clearSceneUI.SetActive(true);
    }
}
