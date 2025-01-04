using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TypeWriterEffect : MonoBehaviour
{
    [SerializeField] private float typeWriterSpeed = 50f;
    
    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        return StartCoroutine(TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        textLabel.text = string.Empty;
        
        yield return new WaitForSeconds(0.2f);
        
        float t = 0;
        int CharIndex = 0;

        while (CharIndex < textToType.Length)
        {
            t += typeWriterSpeed * Time.deltaTime;
            CharIndex = Mathf.FloorToInt(t);
            CharIndex = Mathf.Clamp(CharIndex, 0, textToType.Length);
            
            textLabel.text  = textToType.Substring(0, CharIndex);
            
            yield return null;
        }
        
        textLabel.text  = textToType;
    }
}
