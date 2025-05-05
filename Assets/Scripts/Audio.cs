using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    public AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _audioSource.PlayOneShot(_audioClip);
    }
}
