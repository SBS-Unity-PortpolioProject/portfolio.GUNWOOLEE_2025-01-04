using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeathSceneUI : MonoBehaviour
{
    public Image gameOverImage; 
    public float fadeDuration = 1f;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Color color = gameOverImage.color;
        color.a = 0;
        gameOverImage.color = color;

        float timer = 0f;
        
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            gameOverImage.color = color;
            yield return null;
        }

        color.a = 1;
        gameOverImage.color = color;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("FirstScenes");
    }
    
}
