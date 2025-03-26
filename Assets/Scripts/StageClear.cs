using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClear : MonoBehaviour
{
    [SerializeField] private DialogueActivator _dialogueActivator;
    [SerializeField] private ClearSceneUI clearSceneUI;

    private float DurationTime = 3;

    private void Update()
    {
        if (_dialogueActivator._next)
        {
            StartCoroutine(NextStage());
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && clearSceneUI != null)
        {
            clearSceneUI.gameObject.SetActive(true);
        }
    }

    private IEnumerator NextStage()
    {
        float timer = 0;

        while (timer < DurationTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        
        SceneManager.LoadScene("GameScene2");
    }
}
