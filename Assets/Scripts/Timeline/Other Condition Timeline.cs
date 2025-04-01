using UnityEngine;
using UnityEngine.Playables;

public class OtherConditionTimeline : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private DialogueActivator _dialogueActivator;
    
    public PlayableDirector director;

    private void Update()
    {
        if (_dialogueActivator._other)
        {
            director.Play(); // Dialovue Activator에서 마지막꺼 켜줘야 함
        }
    }
}
