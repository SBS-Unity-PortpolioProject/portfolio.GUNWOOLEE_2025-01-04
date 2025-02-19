using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _settingScreen;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    
    private void OnEnable()
    {
        continueButton.onClick.AddListener(ContinueGame);
        exitButton.onClick.AddListener(ExitGame);
        Time.timeScale = 0;
    }

    void ContinueGame()
    {
        _settingScreen.SetActive(false);
        Time.timeScale = 1;
    }

    void ExitGame()
    {
        StartCoroutine(FadeIn());
        Time.timeScale = 1;
    }

    IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        color.a = 0;
        fadeImage.color = color;

        float timer = 0f;
        
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        
        color.a = 1;
        fadeImage.color = color;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("FirstScenes");
    }
}