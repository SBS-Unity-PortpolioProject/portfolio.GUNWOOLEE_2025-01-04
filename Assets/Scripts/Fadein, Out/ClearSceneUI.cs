using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearSceneUI : MonoBehaviour
{
    public Image gameOverImage; 
    public float fadeDuration = 1f;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

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
        
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
