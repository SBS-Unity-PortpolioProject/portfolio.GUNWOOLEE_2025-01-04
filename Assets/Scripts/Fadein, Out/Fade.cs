using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class Fade : MonoBehaviour
{
    [SerializeField] private DialogueActivator _dialogue;
    
    public Image FadeImage; 
    public float fadeDuration = 1f;

    private void Update()
    {
        if (_dialogue._fadeIn)
        {
            StartCoroutine(fadeIn());
        }
        else if (_dialogue._fadeOut)
        {
            StartCoroutine(fadeOut());
        }
    }

    private void OnEnable()
    {
        StartCoroutine(fadeOut());
    }
    
    IEnumerator fadeIn()
    {
        Color color = FadeImage.color;
        color.a = 0;
        FadeImage.color = color;

        float timer = 0f;
        
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            FadeImage.color = color;
            yield return null;
        }

        color.a = 1;
        FadeImage.color = color;
    }
    
    IEnumerator fadeOut()
    {
        Color color = FadeImage.color;
        color.a = 0;
        FadeImage.color = color;

        float timer = 0f;
        
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timer / fadeDuration);
            FadeImage.color = color;
            yield return null;
        }

        color.a = 0;
        FadeImage.color = color;
    }
}
