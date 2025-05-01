using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]private AudioClip _audioClip;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void Click()
    {
        _audioSource.PlayOneShot(_audioClip);
    }
}
